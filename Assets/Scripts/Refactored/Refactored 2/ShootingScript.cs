using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int fireRate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject bullet = (GameObject)Instantiate(projectile, shootPoint.transform.position, Quaternion.identity);
            bullet.gameObject.GetComponent<Rigidbody>().velocity = shooter.transform.forward * fireRate;
        }
    }
}
