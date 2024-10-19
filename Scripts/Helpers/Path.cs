using System.Collections.Generic;
using UnityEngine;

namespace CorePublic.Helpers
{
    public class Path
    {
        public Vector3[] corners;
        public float TotalLength;

        public Path(Vector3[] corners)
        {
            this.corners = corners;
            TotalLength = 0;
            for (int i = 0; i < this.corners.Length - 1; i++)
            {
                TotalLength += Vector3.Distance(this.corners[i], this.corners[i + 1]);
            }
        }

        public Vector3 GetPoint(float distance)
        {
            if (corners.Length == 0) return Vector3.zero;

            var points = corners;

            // Determine which two points the distance lies between
            float currentDistance = 0f;
            int firstCorner = 0;
            for (int i = 0; i < points.Length - 1; i++)
            {
                float segmentLen = Vector3.Distance(points[i], points[i + 1]);
                if (currentDistance + segmentLen >= distance)
                {
                    firstCorner = i;
                    break;
                }

                currentDistance += segmentLen;
            }

            // Interpolate between the two points
            float remainingDistance = distance - currentDistance;
            if (firstCorner >= points.Length - 1)
                return points[points.Length - 1];

            var startPoint = points[firstCorner];
            var endPoint = points[firstCorner + 1];


            float segmentLength = Vector3.Distance(startPoint, endPoint);
            float t = remainingDistance / segmentLength;
            return Vector3.Lerp(startPoint, endPoint, t);
        }

        public Path GetSubPath(float startDistance, float endDistance)
        {
            float pathLength = 0;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                pathLength += Vector3.Distance(corners[i], corners[i + 1]);
            }

            List<Vector3> subPath = new List<Vector3>();


            float currentDistance = 0;
            bool foundStart = false;
            for (int i = 0; i < corners.Length - 1; i++)
            {
                float distance = Vector3.Distance(corners[i], corners[i + 1]);
                if (!foundStart && currentDistance + distance > startDistance)
                {
                    float distanceFromStart = startDistance - currentDistance;
                    float distanceFromEnd = endDistance - currentDistance;
                    float distanceFromStartPercentage = distanceFromStart / distance;
                    float distanceFromEndPercentage = distanceFromEnd / distance;
                    Vector3 startPoint = Vector3.Lerp(corners[i], corners[i + 1], distanceFromStartPercentage);
                    Vector3 endPoint = Vector3.Lerp(corners[i], corners[i + 1], distanceFromEndPercentage);
                    subPath.Add(startPoint);
                    subPath.Add(endPoint);
                    foundStart = true;
                }
                else if (foundStart && currentDistance + distance > endDistance)
                {
                    float distanceFromEnd = endDistance - currentDistance;
                    float distanceFromEndPercentage = distanceFromEnd / distance;
                    Vector3 endPoint = Vector3.Lerp(corners[i], corners[i + 1], distanceFromEndPercentage);
                    subPath.Add(endPoint);
                    break;
                }
                else if (foundStart)
                {
                    subPath.Add(corners[i + 1]);
                }

                currentDistance += distance;
            }

            return new Path(subPath.ToArray());
        }
    }
}