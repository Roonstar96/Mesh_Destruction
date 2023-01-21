using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script is responsible for generating mulitple small prefabs to replace and
//simulate a larger object, so that it breaks into pieces when it is dedtroyed. 
public class VoxelDestruct3 : MonoBehaviour
{
    //NOTE: All variables for the object being destroyed, its debris
    // & other variables for instantiating the debris in the correct position
    // as well as adjusting the debirs ehaviour depending on its material
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;

    //NOTE: The dimensions of the object being destroyed
    private float rowX;
    private float rowY;
    private float rowZ;

    //NOTE: The scale of the peices of the destroyed object.
    //Top 3 variables are for incramenting the for loops
    //The bottom 3 variables are for making sure that the new objects, spawn in the correct position
    private float newScaleX;
    private float newScaleY;
    private float newScaleZ;

    private float pScaleX;
    private float pScaleY;
    private float pScaleZ;

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
        ReplaceParent();
    }

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
    private void ReplaceParent()
    {
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
        Destroy(parent);
    }

    private void CreatePieces(float x, float y, float z)
    {
        GameObject pieces;
        pieces = GameObject.Instantiate(newPiece, parent.transform.position, Quaternion.identity);

        pieces.transform.position = transform.position + new Vector3(
            (newScaleX + x) - pScaleX, 
            (newScaleY + y) - pScaleY, 
            (newScaleZ + z) - pScaleZ);

        pieces.SetActive(true);
    }
}

