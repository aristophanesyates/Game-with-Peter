using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chucker : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.1f;
            Vector3 position = transform.position + transform.forward * 2;
            Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
        }
    }
}
