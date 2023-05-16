using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int health = 3;
    private GameObject lastStruckBy;
    public void Struck(int dmg, GameObject source)
    {
        if (lastStruckBy == source)
        {
            return;
        }
        health -= dmg;
        if (health < 0)
        {
            Die();
        }
        Debug.Log($"{gameObject.name}: Ow! Now I only have {health} health!");
        lastStruckBy = source;
    }
    public void Die()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);
            if (t.GetComponent<Projectile>())
            {
                t.SetParent(null);
                t.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        Debug.Log(gameObject.name + ": Blech!");
        Destroy(gameObject);
    }
}
