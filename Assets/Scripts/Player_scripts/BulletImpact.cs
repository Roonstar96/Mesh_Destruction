using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script is responsible for appliyng a force to certain objects to simulate an explosion
//of a larger object. it also takes into account the objects condition and the bullets velocity to 
//determine the effects of the 'explosion'
public class BulletImpact : MonoBehaviour
{
    [Header("GameObject variables")]
    [SerializeField] private GameObject shooter;
    [SerializeField] private GameObject pieces;
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private int destroyTime;


    [Header("Blast settings")]
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
        //Debug.Log("Speed: " + bullet.velocity);
    }

    //TODO: Bet the bullets speed, have it so the collision detections only works at a certain speed and above
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (collision.gameObject.GetComponent<Rigidbody>() == null)
            {
                Debug.Log("Has no RigidBody");
                ExplodeBigObjects(collision.contacts[0].point);
            }

            else if (collision.gameObject.GetComponent<Rigidbody>() == true)
            {
                if (collision.gameObject.GetComponent<DestroyPieces>() == true)
                {
                    Debug.Log("Has RigidBody & DP script");
                    ExplodeSmallObjects(collision.contacts[0].point);
                }
                else
                {
                    Debug.Log("Has RigidBody");
                    ExplodeIntoSmallObjects(collision.contacts[0].point, collision.collider);
                }
            }
        }

       // Old code, less efficient than ust checking the layer
       /*if (collision.gameObject.transform.root.CompareTag("Pieces"))
        {   
            if (collision.gameObject.GetComponent<Rigidbody>() == null)
            {
                Debug.Log("Has no RigidBody");
                ExplodeBigObjects(collision.contacts[0].point);
            }

            else if (collision.gameObject.GetComponent<Rigidbody>() == true)
            {
                if (collision.gameObject.GetComponent<DestroyPieces>() == true)
                {
                    Debug.Log("Has RigidBody & DP script");
                    ExplodeSmallObjects(collision.contacts[0].point);
                }
                else
                {
                    Debug.Log("Has RigidBody");
                    ExplodeIntoSmallObjects(collision.contacts[0].point, collision.collider);
                }
            }
        }*/
    }

    //NOTE: This function only effects a single object hit, instead of multiple
    private void ExplodeIntoSmallObjects(Vector3 blastPoint, Collider colHit)
    {
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce(blastPower, blastPoint, blastRadius, blastPower, ForceMode.Impulse);

            colHit.gameObject.AddComponent<VoxelPieces>();
            Destroy(this.gameObject);
    }

    //NOTE: This funcitons is similar to the one abouve except adds a new script to the peices,
    //that break them down even smaller. The blast power and readius is also reduced 
    private void ExplodeBigObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);
        foreach (Collider colHit in collidersHit)
        {
            colHit.gameObject.AddComponent<Rigidbody>();
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce(
                blastPower, 
                blastPoint, 
                blastRadius, 
                1, 
                ForceMode.Impulse);
            Destroy(this.gameObject);
        }
    }

    //NOTE: Just like the above functions, this one is only called if the piece hit is the
    //smallest it can get. This prevents an infinite loop of pieces instatiating 
    private void ExplodeSmallObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);
        foreach (Collider colHit in collidersHit)
        {
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce((blastPower), blastPoint, (blastRadius), 1, ForceMode.Impulse);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestroyPiece()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
