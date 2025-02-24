using UnityEngine;

namespace CorePublic.ExtensionMethods
{
    public static class Vector3Extensions
    {
        public static Vector2 xy(this Vector3 v) {
            return new Vector2(v.x, v.y);
        }
        
        public static Vector3 xz(this Vector3 v) {
            return new Vector3(v.x, 0, v.z);
        }
        
        public static Vector2 yz(this Vector3 v) {
            return new Vector2(0, v.y);
        } 
        

        public static Vector3 WithX(this Vector3 v, float x) {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y) {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z) {
            return new Vector3(v.x, v.y, z);
        }

        public static Vector2 WithX(this Vector2 v, float x) {
            return new Vector2(x, v.y);
        }
	
        public static Vector2 WithY(this Vector2 v, float y) {
            return new Vector2(v.x, y);
        }
	
        public static Vector3 WithZ(this Vector2 v, float z) {
            return new Vector3(v.x, v.y, z);
        }
        
        public static float PlanarDistanceXY(this Vector3 a, Vector3 b) {
            return Vector2.Distance(a.xy(), b.xy());
        }
        
        public static float PlanarDistanceXZ(this Vector3 a, Vector3 b) {
            return Vector2.Distance(a.xz(), b.xz());
        }
        
        public static float PlanarDistanceYZ(this Vector3 a, Vector3 b) {
            return Vector2.Distance(a.yz(), b.yz());
        }
        
        public static float SqrDistance(this Vector3 a, Vector3 b) {
            return (a - b).sqrMagnitude;
        }
        
        public static float SqrDistanceXY(this Vector3 a, Vector3 b) {
            return (a.xy() - b.xy()).sqrMagnitude;
        }
        
        public static float SqrDistanceXZ(this Vector3 a, Vector3 b) {
            return (a.xz() - b.xz()).sqrMagnitude;
        }
        
        public static float SqrDistanceYZ(this Vector3 a, Vector3 b) {
            return (a.yz() - b.yz()).sqrMagnitude;
        }

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnLine(
            this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }
    }
}