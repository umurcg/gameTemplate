using CorePublic.LevelDesignComponents;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    [CustomEditor(typeof(GridLayout3D))]
    public class GridLayout3DEditor : UnityEditor.Editor
    {
        private int _previousChildCount;

        private void OnEnable()
        {
            _previousChildCount = ((GridLayout3D)target).transform.childCount;
        }

        public override void OnInspectorGUI()
        {
            GridLayout3D gridLayout = (GridLayout3D)target;
            EditorGUI.BeginChangeCheck();

            gridLayout._cellSize = EditorGUILayout.Vector3Field("Cell Size", gridLayout._cellSize);
            gridLayout._spacing = EditorGUILayout.Vector3Field("Spacing", gridLayout._spacing);

            // Constraints
            EditorGUILayout.LabelField("Constraints");
            EditorGUI.indentLevel++;

            EditorGUI.BeginDisabledGroup(gridLayout._fixedRow && gridLayout._fixedLayer);
            gridLayout._fixedColumn = EditorGUILayout.Toggle("Fixed Column", gridLayout._fixedColumn);
            if (gridLayout._fixedColumn)
            {
                gridLayout._columnCount = EditorGUILayout.IntField("Column Count", gridLayout._columnCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout._fixedColumn && gridLayout._fixedLayer);
            gridLayout._fixedRow = EditorGUILayout.Toggle("Fixed Row", gridLayout._fixedRow);
            if (gridLayout._fixedRow)
            {
                gridLayout._rowCount = EditorGUILayout.IntField("Row Count", gridLayout._rowCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout._fixedColumn && gridLayout._fixedRow);
            gridLayout._fixedLayer = EditorGUILayout.Toggle("Fixed Layer", gridLayout._fixedLayer);
            if (gridLayout._fixedLayer)
            {
                gridLayout._layerCount = EditorGUILayout.IntField("Layer Count", gridLayout._layerCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;

            gridLayout._gizmoColor = EditorGUILayout.ColorField("Gizmo Color", gridLayout._gizmoColor);
            gridLayout._showGizmos = EditorGUILayout.Toggle("Show Gizmos", gridLayout._showGizmos);
            gridLayout.destroyOnPlay = EditorGUILayout.Toggle("Destroy On Play", gridLayout.destroyOnPlay);

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                gridLayout.UpdateGrid();
            }

            int currentChildCount = gridLayout.transform.childCount;

            if (currentChildCount != _previousChildCount)
            {
                _previousChildCount = currentChildCount;
                gridLayout.UpdateGrid();
            }
        
            if (GUILayout.Button("Update Grid"))
            {
                gridLayout.UpdateGrid();
            }
        }
    }
}
