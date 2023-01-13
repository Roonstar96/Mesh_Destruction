using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SUMMARY: This script works in the same way as the VoxelDestruct3 script with the only difference being 
// the new peices are created from the parent and rescaled, instead of being new prefabs
public class Voxel_pieces : MonoBehaviour
{
    //NOTE: All variables for the object being destroyed, its debris
    // & other variables for instantiating the debris in the correct position
    // as well as adjusting the debirs ehaviour depending on its material
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;

    [SerializeField] private bool isBrick;
    [SerializeField] private bool isConcrete;
    [SerializeField] private bool isGlass;
    [SerializeField] private bool isWood;

    private float ScaleFactor;
    //private int hitCount;

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

    private void Awake()
    {
        //NOTE: the scripts sees what the material of the object is, based on that
        //the new piecs are rescaled, for when they are hit by a bullet and 
        //destroyed further.
        if (isBrick)
        {
            ScaleFactor = 2;
        }

        if (isConcrete)
        {
            ScaleFactor = 3f;
        }

        if (isGlass)
        {
            ScaleFactor = 4f;
        }

        if (isWood)
        {
            ScaleFactor = 2f;
        }

        newPiece = parent;
        newPiece.transform.localScale = parent.transform.localScale / ScaleFactor;

        //hitCount = 0;

    }
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
        //ReplaceParent();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {

            ReplaceParent();
            //FinalDestruction();
        }
    }

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
    private void ReplaceParent()
    {

        for (float x = 0; x < rowX; x += newScaleX)
        {
            for (float y = 0; y < rowY; y += newScaleY)
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


    //NOTE: The OnValidate is used to make sure that only 1 matirial is selected for an object.
    // All tick boxes are untiecked except the first one that was selected
    private void OnValidate()
    {
        if (isBrick)
        {
            isConcrete = false;
            isGlass = false;
            isWood = false;
        }

        if (isConcrete)
        {
            isBrick = false;
            isGlass = false;
            isWood = false;
        }

        if (isGlass)
        {
            isBrick = false;
            isConcrete = false;
            isWood = false;
        }

        if (isWood)
        {
            isBrick = false;
            isConcrete = false;
            isGlass = false;
        }
    }

    //NOTE: completely destroys the object once hit by a bullet
    void FinalDestruction()
    {

    }
}
