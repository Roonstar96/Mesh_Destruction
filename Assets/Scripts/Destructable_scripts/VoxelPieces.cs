using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SUMMARY: This script works by creating new pieces from the parent but have rescaled.
//The amount they are rescaled by is deterimined by the material of the parent object  
public class VoxelPieces : MonoBehaviour
{
    [Header("Voxel Piece varables")]
    [SerializeField] private GameObject _parent;
    [SerializeField] private GameObject _newPiece;
    [SerializeField] private MeshRenderer _mesh;

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

    //[SerializeField] private bool isBrick;
    //[SerializeField] private bool isConcrete;
    //[SerializeField] private bool isGlass;
    //[SerializeField] private bool isWood;
    //private float ScaleFactor;

    private void Awake()
    {
        //NOTE: The pieces scale will change depending on its type,
        // this is to simulate how objects made of these may break in real life (to a degree)

        _parent = gameObject;
        _newPiece = gameObject;

        Debug.Log("Current game object: " + _parent + _newPiece);

        if (gameObject.tag == "Brick")
        {
            _newPiece.transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);
        }
        if (gameObject.tag == "Concrete")
        {
            _newPiece.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
        if (gameObject.tag == "Glass")
        {
            _newPiece.transform.localScale = new Vector3(0.1f, 0.1f, 0.025f);
        }
        if (gameObject.tag == "Wood")
        {
            _newPiece.transform.localScale = new Vector3(0.5f, 0.15f, 0.05f);
        }

        Debug.Log("Current scale: " + _newPiece.transform.localScale.x + _newPiece.transform.localScale.y + _newPiece.transform.localScale.z);

        /*_mesh = _parent.GetComponent<MeshRenderer>();
        Debug.Log("Material: " + _mesh.material.name);
        if (_mesh.material.name == "material_brickTEX (Instance)")
        {
            _newPiece.transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);
        }
        if(_mesh.material.name == "material_concreteTEX (Instance)")
        {
            _newPiece.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        }
        if(_mesh.material.name == "material_glassTEX (Instance)")
        {
            _newPiece.transform.localScale = new Vector3(0.1f, 0.1f, 0.025f);
        }
        if(_mesh.material.name == "material_woodTEX (Instance)")
        {
            _newPiece.transform.localScale = new Vector3(0.5f, 0.15f, 0.05f);
        }*/
    }

    //NOTE: define the varibles for instantiaing the pieces ones parent obejct is destroyed
    private void Start()
    {
        _rowX = _parent.transform.localScale.x;
        _rowY = _parent.transform.localScale.y;
        _rowZ = _parent.transform.localScale.z;

        _newScaleX = _newPiece.transform.localScale.x;
        _newScaleY = _newPiece.transform.localScale.y;
        _newScaleZ = _newPiece.transform.localScale.z;

        _pScaleX = (_parent.transform.localScale.x) / 2;
        _pScaleY = (_parent.transform.localScale.y) / 2;
        _pScaleZ = (_parent.transform.localScale.z) / 2;
        ReplaceParent();
    }

    //SUMMARY: Which each loop, the CreatePieces functino is called to spawn the pieces prefab
    // to replace the origianl object when is being destroyed
    private void ReplaceParent()
    {
        for (float x = 0; x <= _rowX; x += _newScaleX)
        {
            for (float y = 0; y <= _rowY; y += _newScaleY)
            {
                for (float z = 0; z <= _rowZ; z += _newScaleZ)
                {
                    Debug.Log(x + y + z);
                    CreatePieces(x, y, z);
                }
            }
        }
        Destroy(_parent);
    }

    //NOTE: Similar to the CreatePieces funciton in VoxelDestruction3 this function istead
    // add DestroyPiece script then removes the VoxelDestruction3 script from the cloned object
    // and reduces their size based on their material.
    private void CreatePieces(float x, float y, float z)
    {
        GameObject pieces;
        pieces = GameObject.Instantiate(_newPiece, _parent.transform.position, _parent.transform.rotation);
        pieces.transform.position = transform.position + new Vector3(
            (_newScaleX + x) - _pScaleX,
            (_newScaleY + y) - _pScaleY,
            (_newScaleZ + z) - _pScaleZ);
        pieces.GetComponent<Rigidbody>().velocity = _parent.GetComponent<Rigidbody>().velocity;
        Component vp = pieces.GetComponent<VoxelPieces>();
        Debug.Log("Component 2 to destroy: " + vp);
        Destroy(vp);
        pieces.AddComponent<DestroyPieces>();

        pieces.SetActive(true);
    }
}
