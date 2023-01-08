using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDestruct3 : MonoBehaviour
{

    //NOTE: All variables for the object being destroyed, its debris
    // & other variables for instantiating the debris in the correct position 

    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;
    //[SerializeField] private float newScale;
    //[SerializeField] private int row;

    [SerializeField] public float rowX;
    [SerializeField] public float rowY;
    [SerializeField] public float rowZ;

    [SerializeField] public float newScaleX;
    [SerializeField] public float newScaleY;
    [SerializeField] public float newScaleZ;

    [SerializeField] private float pScaleX;
    [SerializeField] private float pScaleY;
    [SerializeField] private float pScaleZ;
    //[SerializeField] private float pieceScaleFactor;


    //NOTE: define the varibles for instantiaing the pieces ones parent obejct is destroyed
    private void Start()
    {
        rowX = parent.transform.localScale.x;
        rowY = parent.transform.localScale.y;
        rowZ = parent.transform.localScale.z;

        newScaleX = newPiece.transform.localScale.x;
        newScaleY = newPiece.transform.localScale.y;
        newScaleZ = newPiece.transform.localScale.z;

        pScaleX = (parent.transform.localScale.x) / 2;
        pScaleY = (parent.transform.localScale.y) / 2;
        pScaleZ = (parent.transform.localScale.z) / 2;
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

        for (float x = 0; x < rowX; x+= newScaleX)
        {
            for (float y = 0; y < rowY; y+= newScaleY)
            {
                for (float z = 0; z < rowZ; z += newScaleZ)
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

        pieces.transform.position = transform.position + new Vector3(
            (newScaleX + x) - pScaleX, 
            (newScaleY + y) - pScaleY, 
            (newScaleZ + z) - pScaleZ);
        //pieces.transform.localScale = new Vector3(newScaleX, newScaleY, newScaleZ);

        pieces.AddComponent<MeshRenderer>();
        //pieces.AddComponent<Rigidbody>();

        pieces.SetActive(true);
    }
}
