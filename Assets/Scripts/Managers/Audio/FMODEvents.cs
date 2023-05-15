using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    #region Singleton Pattern
    public static FMODEvents Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Debug.LogError("More then one FMOD Events in Scene.");
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Variables

    [field: Header("Test Sound")]
    [field: SerializeField] public EventReference testSound { get; private set; }

    #endregion

}
