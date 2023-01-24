using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//SUMMARY: This script works in the same way as the VoxelDestruct3 script with the only difference being 
// the new peices are created from the parent and rescaled, instead of being new prefabs
public class VoxelPieces : MonoBehaviour
{
    //NOTE: All variables for the object being destroyed, its debris
    // & other variables for instantiating the debris in the correct position
    // as well as adjusting the debirs ehaviour depending on its material
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;
    [SerializeField] private MeshRenderer mesh;

    //[SerializeField] private bool isBrick;
    //[SerializeField] private bool isConcrete;
    //[SerializeField] private bool isGlass;
    //[SerializeField] private bool isWood;

    private float ScaleFactor;

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
        //NOTE: the script sees what the material of the object is, based on that
        //the new piecs are rescaled, for when they are hit by a bullet and 
        //destroyed further.

        parent = gameObject;
        newPiece = gameObject;

        Debug.Log("Current game object: " + parent + newPiece);

        mesh = parent.GetComponent<MeshRenderer>();

        Debug.Log("Material: " + mesh.material.name);

        if (mesh.material.name == "material_brickTEX (Instance)")
        {
            newPiece.transform.localScale = new Vector3(0.4f, 0.1f, 0.1f);
        }
        if(mesh.material.name == "material_concreteTEX (Instance)")
        {
            newPiece.transform.localScale = new Vector3(0.5f, 0.15f, 0.05f);
        }
        if(mesh.material.name == "material_glassTEX (Instance)")
        {
            newPiece.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        if(mesh.material.name == "material_woodTEX (Instance)")
        {
            newPiece.transform.localScale = new Vector3(0.5f, 0.15f, 0.05f);
        }

        Debug.Log("Current scale: " + newPiece.transform.localScale.x + newPiece.transform.localScale.y + newPiece.transform.localScale.z);
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

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
    private void ReplaceParent()
    {
        for (float x = 0; x <= rowX; x += newScaleX)
        {
            for (float y = 0; y <= rowY; y += newScaleY)
            {
                for (float z = 0; z <= rowZ; z += newScaleZ)
                {
                    Debug.Log(x + y + z);
                    CreatePieces(x, y, z);
                }
            }
        }
        Destroy(parent);
    }

    //NOTE: Similar to the CreatePieces funciton in VoxelDestruction3 this function istead
    // add DestroyPiece script then removes the VoxelDestruction3 script from the cloned object
    // and reduces their size based on their material
    private void CreatePieces(float x, float y, float z)
    {
        GameObject pieces;
        pieces = GameObject.Instantiate(newPiece, parent.transform.position, parent.transform.rotation);
        pieces.transform.position = transform.position + new Vector3(
            (newScaleX + x) - pScaleX,
            (newScaleY + y) - pScaleY,
            (newScaleZ + z) - pScaleZ);
        Component vp = pieces.GetComponent<VoxelPieces>();
        Debug.Log("Component 2 to destroy: " + vp);
        Destroy(vp);
        pieces.AddComponent<DestroyPieces>();

        pieces.SetActive(true);
    }
}
