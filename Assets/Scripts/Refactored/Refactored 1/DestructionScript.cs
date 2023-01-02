using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour
{
    [SerializeField] public int sliceCount = 1;
    [SerializeField] public float explosionForce = 10;

    private bool setEdge = false;
    private Vector3 edgeVertices = Vector3.zero;
    private Vector2 edgeUVs = Vector3.zero;
    private Plane slicePlane = new Plane();

    void OnEnable()
    {
        DestroyOnCollision.onCollisionAction += DestroyObject;
    }

    public void OnDestroy()
    {
        DestroyOnCollision.onCollisionAction -= DestroyObject;
    }

    private void DestroyObject()
    {

        var Mesh = GetComponent<MeshFilter>().mesh;
        Mesh.RecalculateBounds();

        var objectDebris = new List<MeshPiecesScript> ();
        var subDebris = new List<MeshPiecesScript>();

        var mainObject = new MeshPiecesScript()
        {
            UVs = Mesh.uv,
            Vertices = Mesh.vertices,
            Normals = Mesh.normals,
            bounds = Mesh.bounds,

            Triangles = new int[Mesh.subMeshCount][]
        };

        for (int i = 0; i < Mesh.subMeshCount; i++)
        {
            mainObject.Triangles[i] = Mesh.GetTriangles(i);
        }

        objectDebris.Add(mainObject);

        for (int x = 0; x < sliceCount; x++)
        {
            for(int i = 0; i < objectDebris.Count; i++)
            {
                var bounds = objectDebris[i].bounds;
                bounds.Expand(0.5f);

                var plane = new Plane(UnityEngine.Random.onUnitSphere, new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                                                                                   UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                                                                                   UnityEngine.Random.Range(bounds.min.z, bounds.max.z)));
                subDebris.Add(CreateNewMesh(objectDebris[i], plane, true));
                subDebris.Add(CreateNewMesh(objectDebris[i], plane, false));
            }

            objectDebris = new List<MeshPiecesScript>(subDebris);
            subDebris.Clear();
        }

        for (var i = 0; i < objectDebris.Count; i++)
        {
            objectDebris[i].NewGameObject(this);
            objectDebris[i].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(objectDebris[i].bounds.center * explosionForce, transform.position);
        }

        Destroy(gameObject);

    }

    private MeshPiecesScript CreateNewMesh(MeshPiecesScript original, Plane plane, bool left)
    {
        var meshPieces = new MeshPiecesScript() { };
        var ray1 = new Ray();
        var ray2 = new Ray();

        for (var i = 0; i < original.Triangles.Length; i++)
        {
            var triangles = original.Triangles[i];
            setEdge = false;

            for (var j = 0; j < triangles.Length; j = j + 3)
            {
                var sideA = plane.GetSide(original.Vertices[triangles[j]]) == left;
                var sideB = plane.GetSide(original.Vertices[triangles[j + 1]]) == left;
                var sideC = plane.GetSide(original.Vertices[triangles[j + 2]]) == left;

                var sideCount = (sideA ? 1 : 0) +
                                (sideB ? 1 : 0) +
                                (sideC ? 1 : 0);
                if (sideCount == 0)
                {
                    continue;
                }
                if (sideCount == 3)
                {
                    meshPieces.AddNewTriangle(i,
                                         original.Vertices[triangles[j]], original.Vertices[triangles[j + 1]], original.Vertices[triangles[j + 2]],
                                         original.Normals[triangles[j]], original.Normals[triangles[j + 1]], original.Normals[triangles[j + 2]],
                                         original.UVs[triangles[j]], original.UVs[triangles[j + 1]], original.UVs[triangles[j + 2]]);
                    continue;
                }

                //cut points
                var singleIndex = sideB == sideC ? 0 : sideA == sideC ? 1 : 2;

                ray1.origin = original.Vertices[triangles[j + singleIndex]];
                var dir1 = original.Vertices[triangles[j + ((singleIndex + 1) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray1.direction = dir1;
                plane.Raycast(ray1, out var enter1);
                var lerp1 = enter1 / dir1.magnitude;

                ray2.origin = original.Vertices[triangles[j + singleIndex]];
                var dir2 = original.Vertices[triangles[j + ((singleIndex + 2) % 3)]] - original.Vertices[triangles[j + singleIndex]];
                ray2.direction = dir2;
                plane.Raycast(ray2, out var enter2);
                var lerp2 = enter2 / dir2.magnitude;

                //first vertex = ancor
                AddEdge(i,
                        meshPieces,
                        left ? plane.normal * -1f : plane.normal,
                        ray1.origin + ray1.direction.normalized * enter1,
                        ray2.origin + ray2.direction.normalized * enter2,
                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                if (sideCount == 1)
                {
                    meshPieces.AddNewTriangle(i,
                                        original.Vertices[triangles[j + singleIndex]],
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        original.Normals[triangles[j + singleIndex]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        original.UVs[triangles[j + singleIndex]],
                                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 2) % 3)]], lerp2));

                    continue;
                }

                if (sideCount == 2)
                {
                    meshPieces.AddNewTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UVs[triangles[j + ((singleIndex + 1) % 3)]],
                                        original.UVs[triangles[j + ((singleIndex + 2) % 3)]]);
                    meshPieces.AddNewTriangle(i,
                                        ray1.origin + ray1.direction.normalized * enter1,
                                        original.Vertices[triangles[j + ((singleIndex + 2) % 3)]],
                                        ray2.origin + ray2.direction.normalized * enter2,
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.Normals[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector3.Lerp(original.Normals[triangles[j + singleIndex]], original.Normals[triangles[j + ((singleIndex + 2) % 3)]], lerp2),
                                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 1) % 3)]], lerp1),
                                        original.UVs[triangles[j + ((singleIndex + 2) % 3)]],
                                        Vector2.Lerp(original.UVs[triangles[j + singleIndex]], original.UVs[triangles[j + ((singleIndex + 2) % 3)]], lerp2));
                    continue;
                }
            }
        }

        meshPieces.ListToArray();

        return meshPieces;
    }

    private void AddEdge(int subMesh, MeshPiecesScript meshPieces, Vector3 normal, Vector3 vertex1, Vector3 vertex2, Vector2 uv1, Vector2 uv2)
    {
        if (!setEdge)
        {
            setEdge = true;
            edgeVertices = vertex1;
            edgeUVs = uv1;
        }
        else
        {
            slicePlane.Set3Points(edgeVertices, vertex1, vertex2);

            meshPieces.AddNewTriangle(subMesh,
                                edgeVertices,
                                slicePlane.GetSide(edgeVertices + normal) ? vertex1 : vertex2,
                                slicePlane.GetSide(edgeVertices + normal) ? vertex2 : vertex1,
                                normal,
                                normal,
                                normal,
                                edgeVertices,
                                uv1,
                                uv2);
        }
    }


    public class MeshPiecesScript
    {
        public Vector3[] Vertices;
        public int[][] Triangles;
        public Vector3[] Normals;
        public Vector2[] UVs;

        public GameObject gameObject;
        public Bounds bounds = new Bounds();

        private List<Vector3> _Vertices = new List<Vector3>();
        private List<List<int>> _Triangles = new List<List<int>>();
        private List<Vector3> _Normals = new List<Vector3>();
        private List<Vector2> _UVs = new List<Vector2>();

        public MeshPiecesScript()
        {

        }

        public void AddNewTriangle(int mesh, Vector3 v1, Vector3 v2, Vector3 v3,
                                    Vector3 n1, Vector3 n2, Vector3 n3,
                                    Vector2 uv1, Vector2 uv2, Vector2 uv3)
        {
            _Vertices.Add(v1);
            _Triangles[mesh].Add(_Vertices.Count);
            _Vertices.Add(v2);
            _Triangles[mesh].Add(_Vertices.Count);
            _Vertices.Add(v3);
            _Triangles[mesh].Add(_Vertices.Count);

            _Normals.Add(n1);
            _Normals.Add(n2);
            _Normals.Add(n3);

            _UVs.Add(uv1);
            _UVs.Add(uv2);
            _UVs.Add(uv3);

            bounds.min = Vector3.Min(bounds.min, v1);
            bounds.max = Vector3.Min(bounds.max, v1);

            bounds.min = Vector3.Min(bounds.min, v2);
            bounds.max = Vector3.Min(bounds.max, v2);

            bounds.min = Vector3.Min(bounds.min, v3);
            bounds.max = Vector3.Min(bounds.max, v3);
        }

        public void ListToArray()
        {
            Vertices = _Vertices.ToArray();
            Triangles = new int[_Triangles.Count][];
            Normals = _Normals.ToArray();
            UVs = _UVs.ToArray();

            for (int i = 0; i < _Triangles.Count; i++)
            {
                Triangles[i] = _Triangles[i].ToArray();
            }
        }

        public void NewGameObject(DestructionScript obj)
        {

            gameObject = new GameObject(obj.name);

            gameObject.transform.position = obj.transform.position;
            gameObject.transform.rotation = obj.transform.rotation;
            gameObject.transform.localScale = obj.transform.localScale;

            var newMesh = new Mesh();
            newMesh.name = obj.GetComponent<MeshFilter>().mesh.name;

            newMesh.vertices = Vertices;
            newMesh.normals = Normals;
            newMesh.uv = UVs;
            bounds = newMesh.bounds;

            for (int i = 0; i < Triangles.Length; i++)
            {
                newMesh.SetTriangles(Triangles[i], i, true);
            }

            MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.materials = obj.GetComponent<MeshRenderer>().materials;

            MeshFilter filter = gameObject.AddComponent<MeshFilter>();
            filter.mesh = newMesh;

            Rigidbody rigidbody = gameObject.AddComponent<Rigidbody>();

            var collider = gameObject.AddComponent<MeshCollider>();
            collider.convex = true;

            var destroyMesh = gameObject.AddComponent<DestructionScript>();
            destroyMesh.sliceCount = obj.sliceCount;
            destroyMesh.explosionForce = obj.explosionForce;

        }

    }


}
