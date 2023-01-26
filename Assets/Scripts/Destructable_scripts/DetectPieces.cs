using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPieces : MonoBehaviour
{
    [Header("Object variables")]
    [SerializeField] private GameObject _piece;
    [SerializeField] private Collider _pieceCol;

    [Header("Collision variables")]
    //[SerializeField] private List <Collider> otherCol;
    [SerializeField] private Collider[] _otherCol;
    [SerializeField] private LayerMask _layers;
    [SerializeField] public int i = 0;

    private void Update()
    {
        ObjectIsColliding();
    }
    private void ObjectIsColliding()
    {
        _otherCol = Physics.OverlapBox(_piece.transform.position, _piece.transform.localScale / 2, Quaternion.identity, _layers);

        for (i = 0; i < _otherCol.Length; i++)
        {
            Debug.Log("Points Colliding: " + _otherCol[i].name);
            i++;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_piece.transform.position, _piece.transform.localScale);
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        _otherCol = Physics.OverlapBox(_piece.transform.position, _piece.transform.localScale / 2, Quaternion.identity, _layers);

        for (i = 0; i < _otherCol.Length; i++)
        {
            Debug.Log("Points Colliding: " + _otherCol[i].name);
            i++;
        }

        foreach(Collider colContact in collision)
        {
            print("Points Colliding: " + collision.contacts.point);
        }
    }*/
}
