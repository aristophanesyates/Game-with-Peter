using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        //pInput.Runtime.Look.performed+= ctx => LookAround(ctx.r)
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
        float mouseX = Input.GetAxisRaw("Mouse X") * turnSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * turnSpeed * Time.deltaTime;
        axisRotation.x = axisRotation.x + mouseX;
        axisRotation.y = Mathf.Clamp(axisRotation.y + mouseY, -90, 90);

        transform.rotation = Quaternion.Euler(0, axisRotation.x, 0);
        if (invertY)
        {
            camera.transform.localRotation = Quaternion.Euler(-axisRotation.y, 0, 0);
        }
        else
        {
            camera.transform.localRotation = Quaternion.Euler(axisRotation.y, 0, 0);
        }
        Vector2 mov = pInput.Runtime.Movement.ReadValue<Vector2>() * walkSpeed * Time.deltaTime;
        transform.position += transform.forward * mov.y + transform.right * mov.x;
    }
}
