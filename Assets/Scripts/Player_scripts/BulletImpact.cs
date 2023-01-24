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
        //Debug.Log("Speed: " + bullet.velocity);
    }

    //TODO: Bet the bullets speed, have it so the collision detections only works at a certain speed and above
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Pieces"))
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
                    //StartCoroutine(ExplodeIntoSmallObjects(collision.collider));
                    StartCoroutine(ExplodeIntoSmallObjects(collision.contacts[0].point));
                }
            }
        }
    }

    //NOTE: This fucntion adds forces to objects in a set radius from the bullets impact
    private IEnumerator ExplodeIntoSmallObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);

        foreach (Collider colHit in collidersHit)
        {
            //Component dp = colHit.GetComponent<DestroyPieces>();
            //Destroy(dp);
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce(blastPower, blastPoint, blastRadius, 1, ForceMode.Impulse);

            yield return new WaitForSeconds(0.75f);
            colHit.gameObject.AddComponent<VoxelPieces>();
            Destroy(this.gameObject);
        }
    }

    //NOTE: This funcitons is similar to the one abouve except adds a new script to the peices,
    //that break them down even smaller. The blast power and readius is also reduced 
    private void ExplodeBigObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);
        foreach (Collider colHit in collidersHit)
        {
            colHit.gameObject.AddComponent<Rigidbody>();
            //colHit.gameObject.AddComponent<DestroyPieces>();
            colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
            colHit.GetComponent<Rigidbody>().AddExplosionForce(blastPower, blastPoint, blastRadius, 1, ForceMode.Impulse);
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
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
