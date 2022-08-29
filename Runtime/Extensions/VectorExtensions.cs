using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace llagache
{
    public static class VectorExtensions
    {
        public static Vector3 ToVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }
        
        public static Vector4 ToVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
        
        public static Vector3 Center(Vector3[] points)
        {
            Vector3 center = Vector3.zero;
            if (points.Length > 0)
            {
                foreach (var point in points)
                {
                    center += point;
                }

                center /= points.Length;
            }

            return center;
        }

        public static Vector3[] SubdivideLineUniform(Vector3[] line, float distance)
        {
            var res = new List<Vector3>();
            for (int i = 0; i < line.Length - 1; i++)
            {
                float d = Vector3.Distance(line[i], line[i + 1]) / distance;
                for (int j = 0; j < d; j++)
                {
                    res.Add(Vector3.Lerp(line[i], line[i + 1], (1f / d) * j));
                }
            }

            res.Add(line[line.Length - 1]);
            return res.ToArray();
        }


        public static Vector3[] LaplaceSmoothing(Vector3[] points, int laplaceFactor)
        {
            Vector3[] res = points.ToArray();
            for (int n = 0; n < laplaceFactor; n++)
            {
                for (int i = 1; i < points.Length - 1; i++)
                {
                    res[i] = (res[i - 1] + res[i] + res[i + 1]) / 3;
                }
            }

            return res;
        }
        
        public static Vector3 Clamp(this Vector3 vector3, float min, float max)
        {
            var clampedVector3 = new Vector3
            {
                x = Mathf.Clamp(vector3.x, min, max),
                y = Mathf.Clamp(vector3.y, min, max),
                z = Mathf.Clamp(vector3.z, min, max)
            };
            return clampedVector3;
        }

        public static Vector3Int Clamp(this Vector3Int vector3, int min, int max)
        {
            Vector3Int clampedVector3 = new Vector3Int();
            clampedVector3.x = Mathf.Clamp(vector3.x, min, max);
            clampedVector3.y = Mathf.Clamp(vector3.y, min, max);
            clampedVector3.z = Mathf.Clamp(vector3.z, min, max);
            return clampedVector3;
        }

        public static Vector3 Abs(this Vector3 vector3)
        {
            return new Vector3(Mathf.Abs(vector3.x), Mathf.Abs(vector3.y), Mathf.Abs(vector3.z));
        }
        
        public static float Get(this Vector3 vec, int index)
        {
            return vec[index];
        }
        public static int Get(this Vector3Int vec, int index)
        {
            return vec[index];
        }
		
        public static Vector3 Snap(this Vector3 v, float snapValue){
            float x = Mathf.Round(v.x / snapValue) * snapValue;
            float y = Mathf.Round(v.y / snapValue) * snapValue;
            float z = Mathf.Round(v.z / snapValue) * snapValue;
            return new Vector3(x,y,z);
        }

        public static Vector3 Snap(this Vector3 v, Vector3 snapValues){
            float x = Mathf.Round(v.x / snapValues.x) * snapValues.x;
            float y = Mathf.Round(v.y / snapValues.y) * snapValues.y;
            float z = Mathf.Round(v.z / snapValues.z) * snapValues.z;
            return new Vector3(x,y,z);
        }
        
        public static Vector3 ProjectPointOnSegment(Vector3 a, Vector3 b, Vector3 point)
        {
            return ClampPointToSegment(ProjectPointOnLine(a,b, point), a, b);
        }

        public static Vector3 ProjectPointOnLine(Vector3 a, Vector3 b, Vector3 point)
        {
            return Vector3.Project((point-a),(b-a))+a;
        }
        
        public static Vector3 ClampPointToSegment(Vector3 point, Vector3 start, Vector3 end)
        {
            var toStart = (point - start).sqrMagnitude;
            var toEnd = (point - end).sqrMagnitude;
            var segment = (start - end).sqrMagnitude;
            if (toStart > segment || toEnd > segment) return toStart > toEnd ? end : start;
            return point;
        }
        
        public static Vector3 ToCanvasSpacePosition(this Vector3 position, RectTransform canvasRect, out bool inCameraFrustum, out bool inFrontOfCamera)
        {
            var viewport = Camera.main.WorldToViewportPoint(position);

            inCameraFrustum = viewport.x > 0 && viewport.x < 1 && viewport.y > 0 && viewport.y < 1;
            inFrontOfCamera = viewport.z > 0;

            var sizeDelta = canvasRect.sizeDelta;
            var x = viewport.x * sizeDelta.x - sizeDelta.x * 0.5f;
            var y = viewport.y * sizeDelta.y - sizeDelta.y * 0.5f;
            return new Vector3(x, y);
        }

    }
}