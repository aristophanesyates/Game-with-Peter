using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private PlayerInputs pInput;

    [Header("Movement Variables")]
    private Vector3 axisRotation = new Vector2(0,0);
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] [Range(1, 10)] private float JumpHeight;
    [SerializeField, Range(0, 10)] private int MaxAirJumps;
    private Collider playerCollider;
    private float distToGround;
    private int jumpPhase;

    [Header("Player Slope & Step Variables")]
    [SerializeField, Range(0f, 90f)] private float maxGroundAngle = 25f;
    [SerializeField, Range(0f, 100f)] private float MaxSnapSpeed = 100f;
    [SerializeField, Range(0f, 2f)] private float probeDistance = 1f;
    [SerializeField] LayerMask probeMask = -1;
    private int groundContactCount;
    private bool isGrounded => groundContactCount > 0;
    private float minGroundDotProduct;
    private int stepsSinceLastGrounded, stepsSinceLastJump;
    private Vector3 contactNormal;

    [Header("Camera Variables")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] [Min(0.01f)] private float interpolationTime;
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private bool invertY = true;
    float mouseX, mouseY;

    [Header("Interaction Events")]
    [SerializeField] private InteractionEvent fireEvent;
    [SerializeField] private InteractionEvent reloadEvent;
    [SerializeField] private InteractionEvent jumpEvent;
    #endregion

    #region Initialisation

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pInput.Runtime.Fire.performed += ctx => Chuck();
        pInput.Runtime.Reload.performed += ctx => Reload();
        pInput.Runtime.Jump.performed += ctx => Jump();
    }
    void Awake()
    {
        pInput = new PlayerInputs();

        playerCollider = GetComponentInChildren<Collider>();
        distToGround = playerCollider.bounds.extents.y;

        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }
    void OnEnable()
    {
        pInput.Runtime.Enable();
    }
    void OnDisable()
    {
        pInput.Runtime.Disable();
    }

    #endregion

    #region Fixed/Update
    void Update()
    {
        MouseLook();
    }
    void FixedUpdate()
    {
        UpdateState();
        FixedMouseLook();
        Move();
        ClearState();
    }
    #endregion

    #region Custom Runtime Methods

    #region Mouse Methods

    private void MouseLook()
    {
        Vector2 mouseVec = pInput.Runtime.Look.ReadValue<Vector2>();
        mouseX = Mathf.Lerp(mouseX, mouseVec.x * turnSpeed, Time.deltaTime * (1f / interpolationTime));
        mouseY = Mathf.Lerp(mouseY, mouseVec.y * turnSpeed, Time.deltaTime * (1f / interpolationTime));
    }
    private void FixedMouseLook()
    {
        axisRotation.x = axisRotation.x + mouseX;
        axisRotation.y = Mathf.Clamp(axisRotation.y + mouseY, -90, 90);

        transform.rotation = Quaternion.Euler(0, axisRotation.x, 0);
        if (!invertY)
        {
            playerCamera.transform.localRotation = Quaternion.Euler(-axisRotation.y, 0, 0);
        }
        else
        {
            playerCamera.transform.localRotation = Quaternion.Euler(axisRotation.y, 0, 0);
        }
    }

    #endregion

    #region Movement Methods

    private void Move()
    {
        Vector2 mov = pInput.Runtime.Movement.ReadValue<Vector2>() * walkSpeed * Time.fixedDeltaTime;
        Vector3 v = transform.forward * mov.y + transform.right * mov.x;
        rb.velocity = new Vector3(v.x, rb.velocity.y, v.z);   
    }

    private bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 6)
        {
            return false;
        }
        float speed = rb.velocity.magnitude;
        if(speed > MaxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, probeDistance, probeMask))
        {
            return false;
        }
        if (hit.normal.y < minGroundDotProduct)
        {
            return false;
        }
        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(rb.velocity, hit.normal);
        if (dot > 0f)
        {
            rb.velocity = (rb.velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    private void Jump()
    {
        if (isGrounded || jumpPhase < MaxAirJumps)
        {
            stepsSinceLastJump = 0;
            jumpPhase += 1;
            // Contact vector check for air jumps
            if (contactNormal == Vector3.zero)
            {
                contactNormal = Vector3.up;
            }
            rb.velocity += contactNormal * JumpHeight;
            jumpEvent.Raise();
        }
    }

    #endregion

    #region Action Methods

    private void Chuck()
    {
        fireEvent.Raise();
    }
    private void Reload()
    {
        reloadEvent.Raise();
    }

    #endregion

    #region Collision Methods
    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount++;
                contactNormal += normal;
            }
        }
    }

    #endregion

    #region Misc Methods

    private void UpdateState()
    {
        stepsSinceLastGrounded++;
        stepsSinceLastJump++;
        if (isGrounded || SnapToGround())
        {
            stepsSinceLastGrounded = 0;
            jumpPhase = 0;
            if(groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    private void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    #endregion

    #endregion
}
