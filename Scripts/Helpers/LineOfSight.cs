using System.Collections.Generic;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class LineOfSight : MonoBehaviour
    {
        public enum UpdateMode
        {
            Start,
            Update,
            FixedUpdate,
            Manual
        }
        
        public UpdateMode updateMode;
        
        public LayerMask obstacleLayer; // The layer to use for obstacles
        public float arcRadius = 5.0f; // The radius of the arc
        public float arcWidth = 90.0f; // The width of the arc in degrees
        public float arcStep = 2f;
        public Material arcMaterial; // The material to use for the arc

        private Mesh _arcMesh; // The mesh used for the arc
        private MeshFilter _meshFilter; // The mesh filter component
        private MeshRenderer _meshRenderer; // The mesh renderer component

        private Vector3[] _arcVertices; // The vertices of the arc
        private int[] _arcTriangles; // The triangles of the arc
        


        // Start is called before the first frame update
        void Start()
        {
            // Create the mesh for the arc
            _arcMesh = new Mesh();
            _meshFilter = gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = arcMaterial;
            
            if (updateMode == UpdateMode.Start)
            {
                UpdateMesh();
            }
        }

        private void Update()
        {
            if (updateMode == UpdateMode.Update)
            {
                UpdateMesh();
            }
        }
        
        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate)
            {
                UpdateMesh();
            }
        }

        // Update is called once per frame
        public void UpdateMesh()
        {
            float angle = 0;
            angle = (angle + 360.0f) % 360.0f;

            // Calculate the start and end angles of the arc
            float startAngle = angle - arcWidth / 2.0f;
            float endAngle = angle + arcWidth / 2.0f;

            // Generate the vertices and triangles of the arc mesh
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            vertices.Add(Vector3.zero);


            for (float a = startAngle; a <= endAngle; a += arcStep)
            {
                float radius = arcRadius;

                //Raycast for obstacle check. If raycast hits obstacle, set radius to distance to obstacle
                RaycastHit hit;
                Vector3 direction = Quaternion.AngleAxis(a, Vector3.up) * transform.forward;
                if (Physics.Raycast(transform.position, direction, out hit, arcRadius, obstacleLayer))
                {
                    radius = hit.distance;
                }

                float rad = a * Mathf.Deg2Rad;
                float x = Mathf.Sin(rad) * radius;
                float z = Mathf.Cos(rad) * radius;
                vertices.Add(new Vector3(x, 0.01f, z));
            }

            int numVertices = vertices.Count;
            for (int i = 1; i < numVertices - 1; i++)
            {
                triangles.Add(0);
                triangles.Add(i);
                triangles.Add(i + 1);
            }


            _arcVertices = vertices.ToArray();
            _arcTriangles = triangles.ToArray();
            _arcMesh.vertices = _arcVertices;
            _arcMesh.triangles = _arcTriangles;
            _arcMesh.RecalculateNormals();
            _meshFilter.mesh = _arcMesh;
        }
    }
}