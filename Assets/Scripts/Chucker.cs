using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Chucker : MonoBehaviour
{
    static int ammo = 5;
    [SerializeField]
    private GameObject prefab;
    [SerializeField]
    private bool sceneChange;

    float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0.1f;
        sceneChange = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ammo);
        timer -= Time.deltaTime;
        if (timer <= 0 && ammo > 0)
        {
            timer = 0.1f;
            Vector3 position = transform.position + transform.forward * 2;
            Projectile.SpawnProjectile(prefab, position, transform.rotation, 20, new Vector3(20, 0, 0), 0.1f);
            ammo--;
        }
    }
}
