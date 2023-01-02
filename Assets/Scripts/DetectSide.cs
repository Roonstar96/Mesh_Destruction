/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectSide : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject cube;
    [SerializeField] private Plane plane;
    private List<Vector3> _Positive = new List<Vector3>();
    private List<Vector3> _Negative = new List<Vector3>();

    private List<Vector3> _Vertices = new List<Vector3>();
    public Vector3[] vertices;  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Plane")
        {
            CheckVertexPosition(plane);
        }
    }

    private void CheckVertexPosition(Plane plane)
    {
        var checkmesh = cube.GetComponent<MeshFilter>().mesh;
        var vertex = checkmesh.vertices;
   

        vertices = new Vector3[6];

        for (int i = 0; i < vertices.Length; i ++)
        {
            _Vertices.Add(vertex);

        }

        plane.GetSide(vertex);
    }

    public bool GetSide(Vector3 point)
    {
        return true;  
    }
}*/
