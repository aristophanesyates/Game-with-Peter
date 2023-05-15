using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, Vector3 velocity, Vector3 spin)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(velocity, ForceMode.VelocityChange);
        projScript.rb.AddTorque(spin, ForceMode.VelocityChange);
    }
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, float velocity, Vector3 spin)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(proj.transform.forward, ForceMode.VelocityChange);
        projScript.rb.AddTorque(spin, ForceMode.VelocityChange);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
