using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, Vector3 velocity, Vector3 spin, float scale = 1)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        proj.transform.localScale = new Vector3(scale, scale, scale);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(velocity, ForceMode.VelocityChange);
        projScript.rb.AddTorque(spin, ForceMode.VelocityChange);
    }
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, float speed, Vector3 spin, float scale = 1)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        proj.transform.localScale = new Vector3(scale, scale, scale);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(proj.transform.forward * speed, ForceMode.VelocityChange);
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
