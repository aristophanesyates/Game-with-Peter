using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Windows;

public class PickupKnife : MonoBehaviour
{
    private PlayerInputs pInput;
    [SerializeField]
    private float armLength = 5;
    [SerializeField] private InteractionEvent knifeTake;
    public void Pickup()
    {
        RaycastHit[] hitInfoArr = Physics.RaycastAll(transform.position, transform.forward * armLength);
        for (int i = 0; i < hitInfoArr.Length; i++)
        {
            Collider hit = hitInfoArr[i].collider;
            if (hit)
            {
                Debug.Log("Hit " + hit.gameObject.name);
                Projectile knife = hit.gameObject.GetComponent<Projectile>();
                if (!knife)
                {
                    knife = hit.gameObject.GetComponentInParent<Projectile>();
                }
                if (knife)
                {
                    Debug.Log("Success");
                    if (knife.nullified)
                    {
                        Destroy(knife.gameObject);
                        knifeTake.Raise();
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pInput.Runtime.Take.performed += ctx => Pickup();
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
        
    }
}
