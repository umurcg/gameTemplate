
using UnityEngine;

namespace CorePublic.LevelDesignComponents
{
    public class GridLayout3D : MonoBehaviour
    {
        [SerializeField] public Vector3 _cellSize = new Vector3(1, 1, 1);
        [SerializeField] public Vector3 _spacing = new Vector3(0.5f, 0.5f, 0.5f);
        [SerializeField] public bool _fixedColumn = false;
        [SerializeField] public bool _fixedRow = false;
        [SerializeField] public bool _fixedLayer = false;
        [SerializeField] public int _columnCount = 1;
        [SerializeField] public int _rowCount = 1;
        [SerializeField] public int _layerCount = 1;
        [SerializeField] public Color _gizmoColor = Color.white;
        [SerializeField] public bool _showGizmos = true;

        private Vector3 _gridSize;
    
        [SerializeField] public bool destroyOnPlay = true;
    
        private void Awake()
        {
            if (destroyOnPlay)
            {
                Destroy(this);
            }
        }

        private void OnValidate()
        {
            UpdateGrid();
        }
    
        public void UpdateGrid()
        {
            int childCount = transform.childCount;
            _gridSize = CalculateGridSize(childCount);

            Vector3 gridCenter = new Vector3(
                (_gridSize.x - 1) * (_cellSize.x + _spacing.x) / 2,
                (_gridSize.y - 1) * (_cellSize.y + _spacing.y) / 2,
                (_gridSize.z - 1) * (_cellSize.z + _spacing.z) / 2
            );

            for (int i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                Vector3 newPosition = Vector3.Scale(GetGridIndex(i), _cellSize + _spacing) - gridCenter;
                child.localPosition = newPosition;
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(child);
#endif
            }
        }

        private Vector3 CalculateGridSize(int count)
        {
            int x, y, z;

            if (_fixedColumn && _fixedRow)
            {
                x = _columnCount;
                y = _rowCount;
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (_fixedColumn && _fixedLayer)
            {
                x = _columnCount;
                z = _layerCount;
                y = Mathf.CeilToInt(count / (float)(x * z));
            }
            else if (_fixedRow && _fixedLayer)
            {
                y = _rowCount;
                z = _layerCount;
                x = Mathf.CeilToInt(count / (float)(y * z));
            }
            else if (_fixedColumn)
            {
                x = _columnCount;
                y = Mathf.CeilToInt(Mathf.Pow(count / (float)x, 1 / 2f));
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (_fixedRow)
            {
                y = _rowCount;
                x = Mathf.CeilToInt(Mathf.Pow(count / (float)y, 1 / 2f));
                z = Mathf.CeilToInt(count / (float)(x * y));
            }
            else if (_fixedLayer)
            {
                z = _layerCount;
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
            if (!_showGizmos) return;

            Vector3 gridCenter = new Vector3(
                (_gridSize.x - 1) * (_cellSize.x + _spacing.x) / 2,
                (_gridSize.y - 1) * (_cellSize.y + _spacing.y) / 2,
                (_gridSize.z - 1) * (_cellSize.z + _spacing.z) / 2
            );

            Gizmos.color = _gizmoColor;

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    for (int z = 0; z < _gridSize.z; z++)
                    {
                        Vector3 cellCenter = Vector3.Scale(new Vector3(x, y, z), _cellSize + _spacing) - gridCenter;
                        Gizmos.DrawWireCube(transform.position + cellCenter, _cellSize);
                    }
                }
            }
        }
    }
}
