
using System;
using UnityEngine;

namespace Helpers
{
    public class Grid2D : MonoBehaviour
    {
        [SerializeField] private Vector2 bounds;
        [SerializeField] private int numberOfRow = 3;
        [SerializeField] private int numberOfColumn = 3;

        [HideInInspector] public bool[] cells;
        public int numberOfFilledCells;
        public bool IsFilled => cells.Length == numberOfFilledCells;

        public int TotalNumberOfCells => numberOfRow * numberOfColumn;

        [SerializeField] private bool showGrid;
        

        private void Awake()
        {
            cells = new bool[numberOfColumn * numberOfRow];
        }
        
        public int CalculateCellIndexWorld(Vector3 worldPos)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            localPos.y = 0;
            return CalculateCellIndexLocal(localPos);
        }

        public int CalculateCellIndexLocal(Vector3 localPos)
        {
            float deltaX = bounds.x / numberOfColumn;
            float deltaY = bounds.y / numberOfRow;

            Vector3 minPos = new Vector3(-bounds.x / 2, 0, -bounds.y / 2);
            Vector3 maxPos = new Vector3(bounds.x / 2, 0, bounds.y / 2);

            if (localPos.x < minPos.x || localPos.z < minPos.z || localPos.x > maxPos.x || localPos.z > maxPos.z)
            {
                return -1;
            }

            Vector3 startPos = new Vector3(-bounds.x / 2, 0, -bounds.y / 2);
            Vector3 normalizedPos = localPos - startPos;

            int column = Mathf.FloorToInt(normalizedPos.x / deltaX);
            int row = Mathf.FloorToInt(normalizedPos.z / deltaY);

            return row * numberOfRow + column;
        }

        public Vector3 FillCell()
        {
            if (IsFilled) return Vector3.zero;
            for (int index = 0; index < cells.Length; index++)
            {
                bool cell = cells[index];
                if (!cell)
                {
                    cells[index] = true;
                    return CalculateWorldPosition(index);
                }
            }

            return Vector3.zero;
        }

        [ContextMenu("Test Position")]
        public void TestPosition(int index)
        {
            var pos = CalculateWorldPosition(index);
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = pos;
        }

        public Vector3 CalculateLocalPosition(int index)
        {
            int row = index / numberOfColumn;
            int column = index % numberOfColumn;
            return CalculateLocalPosition(row, column);
        }

        public Vector3 CalculateLocalPosition(int row, int column)
        {
            float deltaX = bounds.x / numberOfColumn;
            float deltaY = bounds.y / numberOfRow;
            Vector3 startPos = new(-bounds.x / 2 + deltaX / 2, 0, -bounds.y / 2 + deltaY / 2);
            Vector3 center = startPos + new Vector3(deltaX * column, 0, deltaY * row);
            return center;
        }


        public Vector3 CalculateWorldPosition(int index)
        {
            return transform.TransformPoint(CalculateLocalPosition(index));
        }

        [ContextMenu("Calculate World Position")]
        public Vector3 CalculateWorldPosition(int row, int column)
        {
            return CalculateWorldPosition(row * numberOfColumn + column);
        }

        [ContextMenu("Convert Index To Coordinates")]
        public void ConvertIndexToCoordinates(int index, out int row, out int column)
        {
            row = index / numberOfColumn;
            column = index % numberOfColumn;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!showGrid) return;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, new Vector3(bounds.x, 0.1f, bounds.y));
            UnityEditor.Handles.matrix = transform.localToWorldMatrix;

            float deltaX = bounds.x / numberOfColumn;
            float deltaY = bounds.y / numberOfRow;


            for (int row = 0; row < numberOfRow; row++)
            {
                for (int column = 0; column < numberOfColumn; column++)
                {
                    int index = column + row * numberOfColumn;
                    var center = CalculateLocalPosition(index);

                    Gizmos.DrawWireCube(center, new Vector3(deltaX * .95f, 0.1f, deltaY * .95f));
                    var style = new GUIStyle();
                    style.normal.textColor = Color.white;
                    style.fontSize = 10;
                    UnityEditor.Handles.Label(center, $"{row},{column}", style);
                }
            }
        }
#endif
    }
}
