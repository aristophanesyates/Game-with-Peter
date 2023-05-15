using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TempController : MonoBehaviour
{
    #region Variables
    private PlayerInputs pInputs;

    #endregion

    #region Init

    private void Awake()
    {
        pInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        pInputs.Runtime.Enable();
    }

    private void OnDisable()
    {
        pInputs.Runtime.Disable();
    }

    private void Start()
    {
        pInputs.Runtime.TestAudio.performed += ctx => Notify();
        pInputs.Runtime.Movement.performed += ctx => Movement(ctx.ReadValue<Vector2>());
    }

    #endregion

    #region Test Region

    private void Notify()
    {
        Debug.Log("Test!");
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSound, this.transform.position);
    }

    private void Movement(Vector2 moveVector)
    {
        Debug.Log("Moving" + moveVector);
    }

    #endregion
}
