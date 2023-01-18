using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPieces : MonoBehaviour
{
    //SUMMARY: Destroys a the smallest piece from a destroyed object after 5 seconds
    // helps with performance
    void Awake()
    {
        StartCoroutine(DestroyPiece());
    }
    IEnumerator DestroyPiece()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
