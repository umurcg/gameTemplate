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

            var pivotMode = (GridLayout3D.PivotMode)EditorGUILayout.EnumPopup("Pivot Mode", gridLayout.pivotMode);
            if (pivotMode != gridLayout.pivotMode)
            {
                gridLayout.pivotMode = pivotMode;
                SetDirty(gridLayout);
            }

            EditorGUILayout.LabelField("Axis Directions");
            EditorGUI.indentLevel++;
            var xDir = (GridLayout3D.AxisDirection)EditorGUILayout.EnumPopup("X Direction", gridLayout.xDirection);
            if (xDir != gridLayout.xDirection)
            {
                gridLayout.xDirection = xDir;
                SetDirty(gridLayout);
            }
            var yDir = (GridLayout3D.AxisDirection)EditorGUILayout.EnumPopup("Y Direction", gridLayout.yDirection);
            if (yDir != gridLayout.yDirection)
            {
                gridLayout.yDirection = yDir;
                SetDirty(gridLayout);
            }
            var zDir = (GridLayout3D.AxisDirection)EditorGUILayout.EnumPopup("Z Direction", gridLayout.zDirection);
            if (zDir != gridLayout.zDirection)
            {
                gridLayout.zDirection = zDir;
                SetDirty(gridLayout);
            }
            EditorGUI.indentLevel--;

            var cellSize = EditorGUILayout.Vector3Field("Cell Size", gridLayout.cellSize);
            if (cellSize != gridLayout.cellSize)
            {
                gridLayout.cellSize = cellSize;
                SetDirty(gridLayout);
            }

            var spacing = EditorGUILayout.Vector3Field("Spacing", gridLayout.spacing);
            if (spacing != gridLayout.spacing)
            {
                gridLayout.spacing = spacing;
                SetDirty(gridLayout);
            }

            // Constraints
            EditorGUILayout.LabelField("Constraints");
            EditorGUI.indentLevel++;

            EditorGUI.BeginDisabledGroup(gridLayout.fixedRow && gridLayout.fixedLayer);
            var fixedColumn = EditorGUILayout.Toggle("Fixed Column", gridLayout.fixedColumn);
            if (fixedColumn != gridLayout.fixedColumn)
            {
                gridLayout.fixedColumn = fixedColumn;
                SetDirty(gridLayout);
            }
            if (gridLayout.fixedColumn)
            {
                var columnCount = EditorGUILayout.IntField("Column Count", gridLayout.columnCount);
                if (columnCount != gridLayout.columnCount)
                {
                    gridLayout.columnCount = columnCount;
                    SetDirty(gridLayout);
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout.fixedColumn && gridLayout.fixedLayer);
            var fixedRow = EditorGUILayout.Toggle("Fixed Row", gridLayout.fixedRow);
            if (fixedRow != gridLayout.fixedRow)
            {
                gridLayout.fixedRow = fixedRow;
                SetDirty(gridLayout);
            }
            if (gridLayout.fixedRow)
            {
                var rowCount = EditorGUILayout.IntField("Row Count", gridLayout.rowCount);
                if (rowCount != gridLayout.rowCount)
                {
                    gridLayout.rowCount = rowCount;
                    SetDirty(gridLayout);
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(gridLayout.fixedColumn && gridLayout.fixedRow);
            var fixedLayer = EditorGUILayout.Toggle("Fixed Layer", gridLayout.fixedLayer);
            if (fixedLayer != gridLayout.fixedLayer)
            {
                gridLayout.fixedLayer = fixedLayer;
                SetDirty(gridLayout);
            }
            if (gridLayout.fixedLayer)
            {
                var layerCount = EditorGUILayout.IntField("Layer Count", gridLayout.layerCount);
                if (layerCount != gridLayout.layerCount)
                {
                    gridLayout.layerCount = layerCount;
                    SetDirty(gridLayout);
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel--;

            var runtimeUpdateMode = (LayoutController.RuntimeUpdateMode)EditorGUILayout.EnumPopup("Runtime Update Mode", gridLayout.runtimeUpdateMode);
            if (runtimeUpdateMode != gridLayout.runtimeUpdateMode)
            {
                gridLayout.runtimeUpdateMode = runtimeUpdateMode;
                SetDirty(gridLayout);
            }


            var showGizmos = EditorGUILayout.Toggle("Show Gizmos", gridLayout.showGizmos);
            if (showGizmos != gridLayout.showGizmos)
            {
                gridLayout.showGizmos = showGizmos;
                SetDirty(gridLayout);
            }

            if (showGizmos)
            {
                var gizmoColor = EditorGUILayout.ColorField("Gizmo Color", gridLayout.gizmoColor);
                if (gizmoColor != gridLayout.gizmoColor)
                {
                    gridLayout.gizmoColor = gizmoColor;
                    SetDirty(gridLayout);
                }

                var gizmoCount = EditorGUILayout.IntField("Gizmo Count", gridLayout.gizmoCount);
                if (gizmoCount != gridLayout.gizmoCount)
                {
                    gridLayout.gizmoCount = gizmoCount;
                    SetDirty(gridLayout);
                }
            }

            var updateOnAwake = EditorGUILayout.Toggle("Update On Awake", gridLayout.updateOnAwake);
            if (updateOnAwake != gridLayout.updateOnAwake)
            {
                gridLayout.updateOnAwake = updateOnAwake;
                SetDirty(gridLayout);
            }
            var updateOnStart = EditorGUILayout.Toggle("Update On Start", gridLayout.updateOnStart);
            if (updateOnStart != gridLayout.updateOnStart)
            {
                gridLayout.updateOnStart = updateOnStart;
                SetDirty(gridLayout);
            }
            var updateOnValidate = EditorGUILayout.Toggle("Update On Validate", gridLayout.updateOnValidate);
            if (updateOnValidate != gridLayout.updateOnValidate)
            {
                gridLayout.updateOnValidate = updateOnValidate;
                SetDirty(gridLayout);
            }
            var destroyOnPlay = EditorGUILayout.Toggle("Destroy On Play", gridLayout.destroyOnPlay);
            if (destroyOnPlay != gridLayout.destroyOnPlay)
            {
                gridLayout.destroyOnPlay = destroyOnPlay;
                SetDirty(gridLayout);
            }



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

        private void SetDirty(GridLayout3D gridLayout)
        {
            EditorUtility.SetDirty(gridLayout);
        }
    }
}
