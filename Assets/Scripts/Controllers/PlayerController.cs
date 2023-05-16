using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private float walkSpeed = 1;
    [SerializeField]
    private float turnSpeed = 1;
    [SerializeField]
    private bool invertY = true;
    private Vector3 axisRotation = new Vector2(0,0);
    private PlayerInputs pInput;
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    [Min(0.01f)] float mouseInputInterpolationTime;
    float mouseX, mouseY;
    [SerializeField]
    private InteractionEvent fireEvent;
    #endregion

    #region Initialisation
    private void Chuck()
    {
        fireEvent.Raise();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        pInput.Runtime.Fire.performed += ctx => Chuck();
    }
    void Awake()
    {
        pInput = new PlayerInputs();
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
        mouseX = Mathf.Lerp(mouseX, mouseVec.x * turnSpeed, Time.deltaTime * (1f / mouseInputInterpolationTime));
        mouseY = Mathf.Lerp(mouseY, mouseVec.y * turnSpeed, Time.deltaTime * (1f / mouseInputInterpolationTime));
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
        rb.velocity = transform.forward * mov.y + transform.right * mov.x;
    }
    private void oldMove()
    {
        Vector2 mov = pInput.Runtime.Movement.ReadValue<Vector2>() * walkSpeed;
        if (mov.magnitude == 0)
        {
            return;
        }
        mov = mov + mov.normalized * Vector2.Dot(mov, new Vector2(rb.velocity.x, rb.velocity.y));
        rb.velocity = transform.forward * mov.y + transform.right * mov.x;
    }

    #endregion

    #endregion
}
