using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [SerializeField] private GameObject shooter;
    [SerializeField] private float blastRadius;
    [SerializeField] private float blastPower;
   
    private Collider[] collidersHit;
    public LayerMask blastLayers;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.CompareTag("Pieces"))
        {
            ExplodeObjects(collision.contacts[0].point);
        }
    }

    private void ExplodeObjects(Vector3 blastPoint)
    {
        collidersHit = Physics.OverlapSphere(blastPoint, blastRadius, blastLayers);

        foreach (Collider colHit in collidersHit)
        {
            if (colHit.GetComponent<Rigidbody>() == null)
            {
                colHit.GetComponent<BoxCollider>().isTrigger = false;
                colHit.gameObject.AddComponent<Rigidbody>();

                colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
                colHit.GetComponent<Rigidbody>().AddExplosionForce(blastPower, blastPoint, blastRadius, 1, ForceMode.Impulse);
            }


            //TODO: Fix issue with objects still flying everywhere when hit. 
            //try to do somthing involving the velocity of the bullet
            else if (colHit.GetComponent<Rigidbody>() == true)
            {
                colHit.GetComponent<Rigidbody>().velocity = shooter.transform.forward * 5;
                colHit.GetComponent<Rigidbody>().AddExplosionForce((blastPower / 10), blastPoint, (blastRadius / 10), 1, ForceMode.Impulse);
            }
        }
    }

}
