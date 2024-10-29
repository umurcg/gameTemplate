
using CorePublic.Helpers;
using UnityEngine;

namespace CorePublic.LevelDesignComponents
{
    public class GridLayout3D :  LayoutController
    {
        public Vector3 cellSize = new Vector3(1, 1, 1);
        public Vector3 spacing = new Vector3(0.5f, 0.5f, 0.5f);
        public bool fixedColumn = false;
        public bool fixedRow = false;
        public bool fixedLayer = false;
        public int columnCount = 1;
        public int rowCount = 1;
        public int layerCount = 1;
        [SerializeField] public Color gizmoColor = Color.white;
        [SerializeField] public bool showGizmos = true;

        private Vector3 _gridSize;
    
        public override void UpdateChildrenPosition()
        {
            int childCount = transform.childCount;
            _gridSize = CalculateGridSize(childCount);

            Vector3 gridCenter = new Vector3(
                (_gridSize.x - 1) * (cellSize.x + spacing.x) / 2,
                (_gridSize.y - 1) * (cellSize.y + spacing.y) / 2,
                (_gridSize.z - 1) * (cellSize.z + spacing.z) / 2
            );

            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                Vector3 newPosition = Vector3.Scale(GetGridIndex(i), cellSize + spacing) - gridCenter;
                child.localPosition = newPosition;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(child);
#endif
            }
        }

        private Vector3 CalculateGridSize(int count)
        {
            int x, y, z;

            if (fixedColumn && fixedRow)
            {
                x = columnCount;
                y = rowCount;
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (fixedColumn && fixedLayer)
            {
                x = columnCount;
                z = layerCount;
                y = Mathf.CeilToInt(count / (float)(x * z));
            }
            else if (fixedRow && fixedLayer)
            {
                y = rowCount;
                z = layerCount;
                x = Mathf.CeilToInt(count / (float)(y * z));
            }
            else if (fixedColumn)
            {
                x = columnCount;
                y = Mathf.CeilToInt(Mathf.Pow(count / (float)x, 1 / 2f));
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (fixedRow)
            {
                y = rowCount;
                x = Mathf.CeilToInt(Mathf.Pow(count / (float)y, 1 / 2f));
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (fixedLayer)
            {
                z = layerCount;
                x = Mathf.CeilToInt(Mathf.Pow(count / (float)z, 1 / 2f));
                y = Mathf.CeilToInt(count / (float)(x * z));
            }
            else
            {
                x = Mathf.CeilToInt(Mathf.Pow(count, 1 / 3f));
                y = x;
                z = x;
            }

            return new Vector3(x, y, z);
        }

        private Vector3 GetGridIndex(int index)
        {
            int x = index % (int)_gridSize.x;
            int y = (index / (int)_gridSize.x) % (int)_gridSize.y;
            int z = (index / (int)_gridSize.x) / (int)_gridSize.y;

            return new Vector3(x, y, z);
        }
    
        private void OnDrawGizmos()
        {
            if (!showGizmos) return;

            Vector3 gridCenter = new Vector3(
                (_gridSize.x - 1) * (cellSize.x + spacing.x) / 2,
                (_gridSize.y - 1) * (cellSize.y + spacing.y) / 2,
                (_gridSize.z - 1) * (cellSize.z + spacing.z) / 2
            );

            Gizmos.color = gizmoColor;

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        Vector3 cellCenter = Vector3.Scale(new Vector3(x, y, z), cellSize + spacing) - gridCenter;
                        Gizmos.DrawWireCube(transform.position + cellCenter, cellSize);
                    }
                }
            }
        }
    }
}
