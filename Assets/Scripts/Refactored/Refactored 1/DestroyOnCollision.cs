using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject collisionObject;
    //public GameObject child;

    public delegate void CollisionAction();
    public static event CollisionAction onCollisionAction;

    private void Update()
    {
        if (transform.childCount <= 0)
        {
            Destroy(collisionObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Floor")
        {
            onCollisionAction?.Invoke();
            collisionObject.transform.DetachChildren();
        }
    }
}
