using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDestruct3 : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;
    [SerializeField] private float newScale;
    //[SerializeField] private int row;

    [SerializeField] public float rowX;
    [SerializeField] public float rowY;
    [SerializeField] public float rowZ;

    [SerializeField] public float newScaleX;
    [SerializeField] public float newScaleY;
    [SerializeField] public float newScaleZ;
    //[SerializeField] private float pieceScaleFactor;

    private void Start()
    {
        rowX = parent.transform.localScale.x;
        rowY = parent.transform.localScale.y;
        rowZ = parent.transform.localScale.z;

        newScaleX = newPiece.transform.localScale.x;
        newScaleY = newPiece.transform.localScale.y;
        newScaleZ = newPiece.transform.localScale.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            ReplaceParent();
        }
    }

    private void ReplaceParent()
    {
        parent.SetActive(false);

        for (float x = 0; x < rowX; x+= newScale)
        {
            for (float y = 0; y < rowY; y+= newScale)
            {
                for (float z = 0; z < rowZ; z += newScale)
                {
                    CreatePieces(x, y, z);
                }
            }
        }

        /*for (int x = 0; x < row; x++)
        {
            for (int y = 0; y < row; y++)
            {
                for (int z = 0; z < row; z++)
                {
                    CreatePieces(x, y, z);
                }
            }
        }*/
    }

    private void CreatePieces(float x, float y, float z)
    {
        GameObject pieces;
        pieces = GameObject.Instantiate(newPiece, parent.transform.position, Quaternion.identity);
        //pieces = GameObject.CreatePrimative(PrimitiveType.Cube); 

        pieces.transform.position = transform.position + new Vector3(newScale + x, newScale + y, newScale + z);
        pieces.transform.localScale = new Vector3(newScale, newScale, newScale);

        pieces.AddComponent<MeshRenderer>();
        //pieces.AddComponent<Rigidbody>();

        pieces.SetActive(true);
    }
}
