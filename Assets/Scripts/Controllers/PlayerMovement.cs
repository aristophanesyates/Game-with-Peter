using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Camera camera;
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
    // Start is called before the first frame update
    void Start()
    {
        
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
    // Update is called once per frame
    void Update()
    {
        MouseLook();
        Move();
    }
    void FixedUpdate()
    {
    }
    private void MouseLook()
    {
        Vector2 mouseVec = pInput.Runtime.Look.ReadValue<Vector2>() * turnSpeed * Time.deltaTime;
        float mouseX = mouseVec.x;
        float mouseY = mouseVec.y;
        axisRotation.x = axisRotation.x + mouseX;
        axisRotation.y = Mathf.Clamp(axisRotation.y + mouseY, -90, 90);

        transform.rotation = Quaternion.Euler(0, axisRotation.x, 0);
        if (!invertY)
        {
            camera.transform.localRotation = Quaternion.Euler(-axisRotation.y, 0, 0);
        }
        else
        {
            camera.transform.localRotation = Quaternion.Euler(axisRotation.y, 0, 0);
        }
    }
    private void Move()
    {
        Vector2 mov = pInput.Runtime.Movement.ReadValue<Vector2>() * walkSpeed;
        if (mov.magnitude == 0)
        {
            return;
        }
        mov = mov + mov.normalized * Vector2.Dot(mov, new Vector2(rb.velocity.x, rb.velocity.y));
        rb.velocity = transform.forward * mov.y + transform.right * mov.x;
    }
}
