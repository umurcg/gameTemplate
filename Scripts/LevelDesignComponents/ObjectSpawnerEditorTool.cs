
using UnityEngine;

namespace Helpers
{
    [AddComponentMenu("*Reboot/LevelDesign/Object Spawner Editor Tool")]
    public class ObjectSpawnerEditorTool : MonoBehaviour
    {
        public enum Pattern
        {
            Line,
            Square,
            Circle,
            Disc,
            Spiral,
            Cube
        }
        
        [SerializeField] private Pattern builderPattern = Pattern.Line;
        [SerializeField] private Vector3 patternCenter;
        [SerializeField] private float spawnScaleFactor = 1;
        [SerializeField] private GameObject Prefab;
        
        [Header("Shape Parameters")]
        [SerializeField] private int numberOfBallsLine = 10;
        public Vector3 lineDirection = Vector3.right;
        public float lineDistance = 10f;

        public int numberOfBallsCircle = 10;
        public float radiusCircle = 10;
        public float arcAngleCircle = 360f;

        public int numberOfBallsOfOuterCircle = 10;
        public float radiusDisc = 10f;
        public float arcAngleDisc = 360f;
        public int numberOfCircles;

        public float widthSquare = 10f;
        public float heightSquare = 10f;
        public int numberOfWidthBallSquare = 10;
        public int numberOfHeightBallSquare = 10;

        public int numberOfBallsSpiral = 10;
        public float startRadius = 0.5f;
        public float growFactor = 0.1f;
        public float delta = 0.2f;
        public bool constantDistanceBetweenNodes;

        public float widthCube = 10f;
        public float heightCube = 10f;
        public float depthCube = 10f;
        public int numberOfWidthBallCube = 10;
        public int numberOfHeightBallCube = 10;
        public int numberOfDepthBallCube = 10;

        private void OnDrawGizmos()
        {
            if (Prefab == null) return;
            
            Bounds bounds = Prefab.GetComponent<Renderer>().bounds;
            bounds.size *= spawnScaleFactor;

            Gizmos.matrix = transform.localToWorldMatrix;

            switch (builderPattern)
            {
                // Remaining code inside OnDrawGizmos() is unchanged...
            }
        }

        [ContextMenu("Create Pattern")]
        public void CreatePattern()
        {
            switch (builderPattern)
            {
                // Remaining code inside CreatePattern() is unchanged...
            }
        }

        // All other methods remain the same as in the original script, but [Button] attributes are removed...
        
        [ContextMenu("Remove Last Pattern")]
        public void RemoveLastPattern()
        {
            if (transform.childCount == 0)
                return;
            var lastPattern = transform.GetChild(transform.childCount - 1);
            if (lastPattern != null)
                DestroyImmediate(lastPattern.gameObject);
        }

        // Utility methods for creating patterns are unchanged...
    }
}
