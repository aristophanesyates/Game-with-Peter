using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private int health = 3;
    private GameObject lastStruckBy;
    private void Update()
    {
    }
    public void Struck(int dmg, GameObject source)
    {
        if (lastStruckBy == source)
        {
            return;
        }
        health -= dmg;
        lastStruckBy = source;
        if (health < 0)
        {
            Die();
        }
        Debug.Log($"{gameObject.name}: Ow! Now I only have {health} health!");
    }
    public void Die()
    {
        StartCoroutine(AwaitDeath());
        Debug.Log(gameObject.name + ": Blech!");
    }
    IEnumerator AwaitDeath()
    {
        yield return new WaitForSeconds(0.1f);
        List<Transform> children = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].GetComponent<Projectile>())
            {
                children[i].SetParent(null);
                children[i].GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
