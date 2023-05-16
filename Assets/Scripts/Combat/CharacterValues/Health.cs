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
        Debug.Log(gameObject.name + ": Ow!");
        lastStruckBy = source;
    }
    public void Die()
    {
        Debug.Log(gameObject.name + ": Blech!");
        Destroy(gameObject);
    }
}
