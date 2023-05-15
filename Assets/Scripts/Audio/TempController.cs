using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TempController : MonoBehaviour
{
    #region Test Region

    public void Notify()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.testSound, this.transform.position);
    }

    #endregion
}
