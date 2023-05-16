using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField, Range(0.15f, 5)] private float LegLength;
    private Collider playerCollider;
    private float distToGround;


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
        FixedMouseLook();
        Move();
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
        var v = transform.forward * mov.y + transform.right * mov.x;
        rb.velocity = new Vector3(v.x, rb.velocity.y, v.z);
    }

    private bool GroundedCheck()
    {
        return Physics.Raycast(transform.position + (transform.up * 0.05f), -Vector3.up, distToGround + LegLength);
    }

    private void Jump()
    {
        // If the player is in the air, do nothing;
        if (!GroundedCheck())
        {
            return;
        }
        else
        {
            rb.velocity = transform.up * JumpHeight;
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

    #endregion
}
