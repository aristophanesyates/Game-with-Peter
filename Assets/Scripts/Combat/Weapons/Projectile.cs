using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public bool nullified;
    Vector3 lastVelocity;
    void Awake()
    {
        nullified = false;
        lastVelocity = Vector3.zero;
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
            nullified = true;
            return;
        }
        Vector3 strikeVector = lastVelocity;
        Debug.Log(other.name + " was struck by a knife!");
        Health healthObject = other.GetComponentInParent<Health>(); // recursively checks game object and parents for Health Script. If we don't do this.
        if (healthObject)
        {
            other = healthObject.gameObject;
            Debug.Log("Calling " + other.name + "'s Struck() function. Base Damage: " + 2);
            healthObject.Struck(2, gameObject);
        }
        nullified = true;
        gameObject.transform.rotation = Quaternion.LookRotation(strikeVector.normalized, transform.up);
        gameObject.transform.SetParent(other.transform);
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lastVelocity = rb.velocity;
    }
}
