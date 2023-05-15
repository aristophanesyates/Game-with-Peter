using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TempController : MonoBehaviour
{
    #region Variables
    private PlayerInputs inputs;

    #endregion

    #region Init

    private void Awake()
    {
        inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        inputs.Runtime.TestAudio.Enable();
    }

    private void OnDisable()
    {
        inputs.Runtime.TestAudio.Disable();
    }

    private void Start()
    {
        inputs.Runtime.TestAudio.performed += ctx => Notify();
    }

    #endregion

    #region Test Region

    private void Notify()
    {
        Debug.Log("Test!");
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSound, this.transform.position);
    }

    #endregion
}
