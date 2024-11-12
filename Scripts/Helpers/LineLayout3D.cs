using UnityEngine;

namespace CorePublic.Helpers
{
    public class LineLayout3D : LayoutController
    {
        [SerializeField] protected float spacing = 1;
        [SerializeField] protected bool alignToMiddle = true;
        [SerializeField] protected Vector3 direction = Vector3.forward;
        [SerializeField] protected int gizmoCount = 10;
        [SerializeField] protected Color gizmoColor = Color.white;
        
        public float GetLength()
        {
            return spacing * (transform.childCount - 1);
        }

        public override void UpdateChildrenPosition()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.localPosition = GetPosition(i);
            }
        }
    
        public Vector3 GetPosition(int index)
        {
            return GetPosition(index, transform.childCount);
        }

        public Vector3 GetPosition(int index, int childrenCount)
        {
            if (alignToMiddle)
            {
                return direction * index * spacing - direction * (childrenCount - 1) * spacing / 2;
            }
            else
            {
                return direction * index * spacing;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            for (int i = 0; i < gizmoCount; i++)
            {
                var worldPosition = transform.TransformPoint(GetPosition(i,gizmoCount));
                Gizmos.DrawSphere(worldPosition, 0.1f);
            }
        }
    }
}