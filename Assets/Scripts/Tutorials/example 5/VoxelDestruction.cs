using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelDestruction : MonoBehaviour
{
    [SerializeField] private GameObject mesh;
    [SerializeField] private float scaleX;
    [SerializeField] private float scaleY;
    [SerializeField] private float scaleZ;

    float width;
    float height;
    float depth;

    private void Start()
    {
        scaleX = mesh.gameObject.transform.localScale.x;
        scaleY = mesh.gameObject.transform.localScale.y;
        scaleZ = mesh.gameObject.transform.localScale.z;

        width = transform.localScale.x;
        height = transform.localScale.y;
        depth = transform.localScale.z;

        gameObject.GetComponent<MeshRenderer>().enabled = false;
        mesh.gameObject.GetComponent<Transform>().localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        GameObject[] placeHolder = GameObject.FindGameObjectsWithTag("PlaceHolder");

        for (int i = 0; i < placeHolder.Length; i++)
        {
            placeHolder[i].SetActive(false);
        }

        for (float x = 0; x < width; x += scaleX)
        {
            for (float y = 0; x < height; y += scaleY)
            {
                for (float z = 0; z < depth; z += scaleZ)
                {
                    CreatePieces(x, y, z);
                }
            }
        }
    }

    private void CreatePieces(float x, float y, float z)
    {
        GameObject rubble;
        rubble = GameObject.CreatePrimitive(PrimitiveType.Cube);

        rubble.transform.position = transform.position + new Vector3(width * x, height * y, depth * z);
        rubble.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        rubble.AddComponent<Rigidbody>();
        //rubble.GetComponent<Rigidbody>().mass = rubbleMass;

        /*Vector3 vec = transform.position;

        GameObject pieces = (GameObject)Instantiate(mesh, vec + new Vector3(x, y, z), Quaternion.identity);
        pieces.gameObject.GetComponent<MeshRenderer>().material = gameObject.GetComponent<MeshRenderer>().material;*/
    }
}
