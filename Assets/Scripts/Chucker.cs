using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Chucker : InteractionEventListener
{
    private PlayerInputs pInput;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private InteractionEvent fireEvent;
    // Start is called before the first frame update
    private void Chuck()
    {
        fireEvent.Raise();
    }
    public void ThrowKnife()
    {
        Vector3 position = transform.position + transform.forward * 2;
        Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
    }
    void Start()
    {
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
}
