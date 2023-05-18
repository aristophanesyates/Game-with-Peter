using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    private PlayerInputs pInput;

    [Header("Movement Variables")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] [Range(1, 10)] private float JumpHeight;
    [SerializeField, Range(0, 10)] private int MaxAirJumps;
    [SerializeField, Range(0f, 2f)] private float probeDistance = 1f;
    [SerializeField] LayerMask probeMask = -1, stairsMask = -1;
    private Vector3 axisRotation = new Vector2(0, 0);
    private int jumpPhase;

    [Header("Player Slope & Step Variables")]
    [SerializeField, Range(0f, 90f)] private float maxGroundAngle = 25f;
    [SerializeField, Range(0f, 90f)] private float maxStairsAngle = 50f;
    [SerializeField, Range(0f, 100f)] private float MaxSnapSpeed = 100f;
    private int groundContactCount, steepContactCount;
    private bool isGrounded => groundContactCount > 0;
    private bool isSteep => steepContactCount > 0;
    private float minGroundDotProduct, minStairsDotProduct;
    private int stepsSinceLastGrounded, stepsSinceLastJump;
    private Vector3 contactNormal, steepNormal;

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

        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        minStairsDotProduct = Mathf.Cos(maxStairsAngle * Mathf.Deg2Rad);
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
        Debug.Log(CheckSteepContacts());
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
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 30)
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
        if (hit.normal.y < GetMinDotProduct(hit.collider.gameObject.layer))
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

    private bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
            {
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        return false;
    }

    private void Jump()
    {
        Vector3 jumpDirection;
        if (contactNormal == Vector3.zero)
        {
            contactNormal = Vector3.up;
        }
        if(isGrounded)
        {
            jumpDirection = contactNormal;
        }
        else if (isSteep)
        {
            jumpDirection = steepNormal;
            jumpPhase = 0;
        }
        else if (MaxAirJumps > 0 && jumpPhase <= MaxAirJumps)
        {
            if (jumpPhase == 0) 
            {
                jumpPhase = 1;
            }
            jumpDirection = contactNormal;
        }
        else
        {
            return;
        }
        stepsSinceLastJump = 0;
        jumpPhase += 1;
        float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * JumpHeight);
        float alignedSpeed = Vector3.Dot(rb.velocity, jumpDirection);
        if (alignedSpeed > 0f)
        {
            jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        }
        rb.velocity += jumpDirection * jumpSpeed;
        jumpEvent.Raise();
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
        float minDot = GetMinDotProduct(collision.gameObject.layer);
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                groundContactCount++;
                contactNormal += normal;
            }

            if (normal.y > -0.01f)
            {
                steepContactCount++;
                steepNormal += normal;
            }
        }
    }

    #endregion

    #region Misc Methods

    private void UpdateState()
    {
        stepsSinceLastGrounded++;
        stepsSinceLastJump++;
        if (isGrounded || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1) 
            {
                jumpPhase = 0;
            }
            if (groundContactCount > 1) 
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
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }

    private float GetMinDotProduct(int layer)
    {
        return (stairsMask & (1 << layer)) == 0 ?
            minGroundDotProduct : minStairsDotProduct;
    }

    #endregion

    #endregion
}
