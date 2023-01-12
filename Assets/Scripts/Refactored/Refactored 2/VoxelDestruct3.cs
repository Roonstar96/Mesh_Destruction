using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDestruct3 : MonoBehaviour
{

    //SUMMARY: All variables for the object being destroyed, its debris
    // & other variables for instantiating the debris in the correct position
    // as well as adjusting the debirs ehaviour depending on its material
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;

    [SerializeField] private bool isBrick;
    [SerializeField] private bool isConcrete;
    [SerializeField] private bool isGlass;
    [SerializeField] private bool isWood;

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

    //NOTE: The OnValidate is used to make sure that only 1 matirial is selected for an object.
    // All tick boxes are untiecked except the first one that was selected
    private void OnValidate()
    {
        int countTrue = 0;
        int firstTrue = 0;
        int i = 0;

        bool[] boolChecker = new bool[4] { isBrick, isConcrete, isGlass, isWood };

        while (i < boolChecker.Length)
        {
            if (boolChecker[i] == true)
            {
                countTrue += 1;

                if (firstTrue == 0)
                {
                    firstTrue = i;
                }
            }

            if (countTrue > 1)
            {
                boolChecker[0] = false;
                boolChecker[1] = false;
                boolChecker[2] = false;
                boolChecker[3] = false;
                boolChecker[firstTrue] = true;
                Debug.Log("First to be selected: " + (firstTrue + 1));
                Debug.Log("Only one can be selected at a time");

                break;
            }

            i++;
        }
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
        ReplaceParent();
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            ReplaceParent();
        }
    }*/

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
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
