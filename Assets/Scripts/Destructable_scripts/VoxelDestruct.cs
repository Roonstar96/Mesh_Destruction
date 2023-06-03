using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script is responsible for generating voxels using small prefabs to replace and
//simulate a larger object, so that it breaks into pieces when it is dedtroyed. 
public class VoxelDestruct : MonoBehaviour
{
    [Header("Voxel Piece variables")]
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject newPiece;

    [Header("Parent object dimensions")]
    [SerializeField] private float _rowX;
    [SerializeField] private float _rowY;
    [SerializeField] private float _rowZ;

    [Header("Parent object scale and piece scale variables")]
    [SerializeField] private float _pScaleX;
    [SerializeField] private float _pScaleY;
    [SerializeField] private float _pScaleZ;
    [SerializeField] private float _newScaleX;
    [SerializeField] private float _newScaleY;
    [SerializeField] private float _newScaleZ;

    //NOTE: define the varibles for instantiaing the pieces ones parent obejct is destroyed
    private void Start()
    {
        _rowX = parent.transform.localScale.x;
        _rowY = parent.transform.localScale.y;
        _rowZ = parent.transform.localScale.z;
        _pScaleX = (parent.transform.localScale.x) / 2;
        _pScaleY = (parent.transform.localScale.y) / 2;
        _pScaleZ = (parent.transform.localScale.z) / 2;

        _newScaleX = newPiece.transform.localScale.x;
        _newScaleY = newPiece.transform.localScale.y;
        _newScaleZ = newPiece.transform.localScale.z;

        ReplaceParent();
    }

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
    private void ReplaceParent()
    {
        for (float x = 0; x < _rowX; x+= _newScaleX)
        {
            for (float y = 0; y < _rowY; y+= _newScaleY)
            {
                for (float z = 0; z < _rowZ; z += _newScaleZ)
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
            (_newScaleX + x) - _pScaleX, 
            (_newScaleY + y) - _pScaleY, 
            (_newScaleZ + z) - _pScaleZ);

        pieces.SetActive(true);
    }
}

