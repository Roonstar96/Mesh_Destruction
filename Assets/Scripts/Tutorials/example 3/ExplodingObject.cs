using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingObject : MonoBehaviour
{
    [SerializeField] int rubbleInRow;
    [SerializeField] float rubbleSize;
    [SerializeField] float rubbleMass;
    [SerializeField] int explosionForce;
    [SerializeField] int explosionRadius;
    [SerializeField] float explosionUpward;

    float rubblePivotDistance;
    Vector3 rubblePivot;

    private void Start()
    {
        rubblePivotDistance = rubbleSize * rubbleInRow / 2;
        rubblePivot = new Vector3(rubblePivotDistance, rubblePivotDistance, rubblePivotDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            Explode();
        }       
    }

    public void Explode()
    {
        gameObject.SetActive(false);

        for (int x = 0; x < rubbleInRow; x++)
        {
            for (int y = 0; y < rubbleInRow; y++)
            {
                for (int z = 0; z < rubbleInRow; z++)
                {
                    CreateRubble(x, y, z);
                }
            }
        }

        Vector3 explosionPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
        
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    public void CreateRubble(int x, int y, int z)
    {
        GameObject rubble;
        rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);

        rubble.transform.position = transform.position + new Vector3(rubbleSize * x, rubbleSize * y, rubbleSize * z) - rubblePivot;
        rubble.transform.localScale = new Vector3(rubbleSize, rubbleSize, rubbleSize);

        rubble.AddComponent<Rigidbody>();
        rubble.GetComponent<Rigidbody>().mass = rubbleMass;
    }
}
