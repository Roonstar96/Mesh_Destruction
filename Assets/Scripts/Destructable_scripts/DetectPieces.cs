using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script is responsible for simulating the integrity of a broken model. The script detects
//all of the surounding objects and put into an array. When the array gets bellow a certain number the
//object this script is attached to with have a rigid body attached so it falls
public class DetectPieces : MonoBehaviour
{
    [Header("Object variables")]
    [SerializeField] private GameObject _piece;
    [SerializeField] private Collider _pieceCol;

    [Header("Collision variables")]
    [SerializeField] private int _arrayMin;
    [SerializeField] private Collider[] _otherCol;
    [SerializeField] private LayerMask _layers;

    private void Awake()
    {
        if (gameObject.tag == "Brick")
        {
            _arrayMin = 6;
        }
        if (gameObject.tag == "Concrete")
        {
            _arrayMin = 5;
        }
        if (gameObject.tag == "Glass")
        {
            _arrayMin = 6;
        }
        if (gameObject.tag == "Wood")
        {
            _arrayMin = 4;
        }
    }

    private void Update()
    {
        ObjectIsColliding();
        //DropObject();
    }
    private void ObjectIsColliding()
    {
        _otherCol = Physics.OverlapBox(
            _piece.transform.position, 
            _piece.transform.localScale / 2, 
            Quaternion.identity, 
            _layers);

        for (int i = 0; i < _otherCol.Length; i++)
        {
            Debug.Log("Points Colliding: " + _otherCol[i].name);
            i++;
        }
        StartCoroutine(DropObject());
    }

    private IEnumerator DropObject()
    {
        if (_otherCol.Length <= _arrayMin )
        {
            yield return new WaitForSeconds(0.7f);
            gameObject.AddComponent<Rigidbody>();
        }
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_piece.transform.position, _piece.transform.localScale);
    }*/
}
