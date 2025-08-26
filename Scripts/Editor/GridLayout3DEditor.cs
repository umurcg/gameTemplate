using CorePublic.Helpers;
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

            gridLayout.cellSize = EditorGUILayout.Vector3Field("Cell Size", gridLayout.cellSize);
            gridLayout.spacing = EditorGUILayout.Vector3Field("Spacing", gridLayout.spacing);

            // Constraints
            EditorGUILayout.LabelField("Constraints");
            EditorGUI.indentLevel++;

            EditorGUI.BeginDisabledGroup(gridLayout.fixedRow && gridLayout.fixedLayer);
            gridLayout.fixedColumn = EditorGUILayout.Toggle("Fixed Column", gridLayout.fixedColumn);
            if (gridLayout.fixedColumn)
            {
                gridLayout.columnCount = EditorGUILayout.IntField("Column Count", gridLayout.columnCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout.fixedColumn && gridLayout.fixedLayer);
            gridLayout.fixedRow = EditorGUILayout.Toggle("Fixed Row", gridLayout.fixedRow);
            if (gridLayout.fixedRow)
            {
                gridLayout.rowCount = EditorGUILayout.IntField("Row Count", gridLayout.rowCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout.fixedColumn && gridLayout.fixedRow);
            gridLayout.fixedLayer = EditorGUILayout.Toggle("Fixed Layer", gridLayout.fixedLayer);
            if (gridLayout.fixedLayer)
            {
                gridLayout.layerCount = EditorGUILayout.IntField("Layer Count", gridLayout.layerCount);
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;
            
            gridLayout.runtimeUpdateMode = (LayoutController.RuntimeUpdateMode)EditorGUILayout.EnumPopup("Runtime Update Mode", gridLayout.runtimeUpdateMode);

            gridLayout.gizmoColor = EditorGUILayout.ColorField("Gizmo Color", gridLayout.gizmoColor);
            gridLayout.showGizmos = EditorGUILayout.Toggle("Show Gizmos", gridLayout.showGizmos);
            gridLayout.updateOnAwake = EditorGUILayout.Toggle("Update On Awake", gridLayout.updateOnAwake);
            gridLayout.updateOnStart = EditorGUILayout.Toggle("Update On Start", gridLayout.updateOnStart);
            gridLayout.updateOnValidate = EditorGUILayout.Toggle("Update On Validate", gridLayout.updateOnValidate);
            gridLayout.destroyOnPlay = EditorGUILayout.Toggle("Destroy On Play", gridLayout.destroyOnPlay);
            
            

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                gridLayout.UpdateChildrenPosition();
            }

            int currentChildCount = gridLayout.transform.childCount;

            if (currentChildCount != _previousChildCount)
            {
                _previousChildCount = currentChildCount;
                gridLayout.UpdateChildrenPosition();
            }
        
            if (GUILayout.Button("Update Grid"))
            {
                gridLayout.UpdateChildrenPosition();
            }
        }
    }
}
