using Knife.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeThrow : InteractionEventListener
{
    [SerializeField]
    private GameObject prefab;
    // Start is called before the first frame update
    public void ThrowKnife()
    {
        Vector3 position = transform.position + transform.forward * 2;
        Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
    }
    public void ThrowTwoKnives()
    {
        Vector3 position = transform.position + transform.forward * 2;
        Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
        position = transform.position + transform.forward * 2 + transform.up;
        Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
    }
}
