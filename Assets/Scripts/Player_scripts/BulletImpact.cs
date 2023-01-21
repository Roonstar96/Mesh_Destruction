using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script is responsible for appliyng a force to certain objects to simulate an explosion
//of a larger object. it also takes into account the objects condition and the bullets velocity to 
//determine the effects of the 'explosion'
public class BulletImpact : MonoBehaviour
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject pieces;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private float blastRadius;
    [SerializeField] private float blastPower;

    private Collider[] collidersHit;
    private Vector3 speed;
    public LayerMask blastLayers;

    void Awake()
    {
        bullet = GetComponent<Rigidbody>();
        speed = bullet.velocity; 
        StartCoroutine(DestroyPiece());
    }

    private void Update()
    {
        Debug.Log("Speed: " + bullet.velocity);
    }

    //TODO: Bet the bullets speed, have it so the collision detections only works at a certain speed and above
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Pieces"))
        {
            if (speed.x >= 20)
            {

            }
            if (collision.gameObject.GetComponent<Rigidbody>() == null)
            {
                ExplodeObjects(collision.contacts[0].point);
            }

            else if (collision.gameObject.GetComponent<Rigidbody>() == true)
            {
                ExplodeSmallObjects(collision.contacts[0].point);
            }
        }
    }

    //NOTE: This fucntion adds forces to objects in a set radius from the bullets impact
    private void ExplodeObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);

        foreach (Collider colHit in collidersHit)
        {
            colHit.gameObject.AddComponent<Rigidbody>();

            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce(blastPower, blastPoint, blastRadius, 1, ForceMode.Impulse);
        }
    }

    //NOTE: This funcitons is similar to the one abouve except adds a new script to the peices,
    //that break them down even smaller. The blast power and readius is also reduced 
    private void ExplodeSmallObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);

        foreach (Collider colHit in collidersHit)
        {
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward;
            colHit.GetComponent<Rigidbody>().AddExplosionForce((blastPower / 100), blastPoint, (blastRadius / 10), 1, ForceMode.Impulse);

            colHit.gameObject.AddComponent<VoxelPieces>();

        }
    }

    IEnumerator DestroyPiece()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
