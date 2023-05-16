using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    private bool nullified;
    void Awake()
    {
        nullified = false;
    }
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, Vector3 velocity, Vector3 localSpin, float scale = 1)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        proj.transform.localScale = new Vector3(scale, scale, scale);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(velocity, ForceMode.VelocityChange);
        projScript.rb.AddRelativeTorque(localSpin, ForceMode.VelocityChange);
    }
    public static void SpawnProjectile(GameObject prefab, Vector3 position, Quaternion orientation, float speed, Vector3 localSpin, float scale = 1)
    {
        GameObject proj = Instantiate<GameObject>(prefab, position, orientation);
        proj.transform.localScale = new Vector3(scale, scale, scale);
        Projectile projScript = proj.GetComponent<Projectile>();
        projScript.rb.AddForce(proj.transform.forward * speed, ForceMode.VelocityChange);
        projScript.rb.AddRelativeTorque(localSpin, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (nullified)
        { 
            return;
        }
        GameObject other = collision.gameObject;
        if (other.tag == "Impenetrable")
        {
            return;
        }
        Vector3 strikeVector = rb.velocity.normalized;
        Debug.Log(other.name + " was struck by a knife!");
        gameObject.transform.rotation *= Quaternion.FromToRotation(transform.forward, strikeVector);
        gameObject.transform.SetParent(other.transform);
        rb.isKinematic = true;
        Health healthObject = other.GetComponent<Health>();
        if (healthObject)
        {
            Debug.Log("Calling " + other.name + "'s Struck() function. Base Damage: " + 2);
            healthObject.Struck(2, gameObject);
        }
        nullified = true;
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
