using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chucker : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    float timer = 1;
    // Start is called before the first frame update
    void Start()
    {
        timer = 1000;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Projectile.SpawnProjectile(prefab, transform.position, transform.rotation, 10, Vector3.zero);
            timer = 1;
        }
    }
}
