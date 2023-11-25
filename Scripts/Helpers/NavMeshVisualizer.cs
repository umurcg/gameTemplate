namespace Helpers
{
    using UnityEngine;
    using UnityEngine.AI;

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class NavMeshVisualizer : MonoBehaviour
    {
        [SerializeField] private NavmeshBaker baker;
        private MeshFilter _meshFilter;


        void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            // baker.OnNavMeshBaked += UpdateNavMeshVisual;
        }

        private void UpdateNavMeshVisual()
        {
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();

            Mesh mesh = new Mesh
            {
                vertices = triangulation.vertices,
                triangles = triangulation.indices,
            };


            Vector2[] uvs = new Vector2[triangulation.vertices.Length];
            for (int i = 0; i < uvs.Length; i++)
            {
                uvs[i] = new Vector2(triangulation.vertices[i].x, triangulation.vertices[i].z);
            }

            mesh.uv = uvs;

            var normals = new Vector3[triangulation.vertices.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                normals[i] = Vector3.up;
            }

            mesh.normals = normals;

            _meshFilter.mesh = mesh;
        }
    }
}