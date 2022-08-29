 using UnityEngine;
 using System.Collections.Generic;
 using System.Linq;

 namespace llagache
 {
     public static class GeometryHelpers
     {

         public static int ClosestPointIndex(Vector3 point, Vector3[] points)
         {
             var bestM = float.PositiveInfinity;
             var res = -1;
             for (int i = 0; i < points.Length; i++)
             {
                 var m = Vector3.SqrMagnitude(point - points[i]);
                 if (m > bestM) continue;
                 bestM = m;
                 res = i;
             }

             return res;
         }


         public static Vector3 ClosestVertex(Mesh mesh, Vector3 point, out int index, Transform meshTransform = null)
         {
             var p = meshTransform ? meshTransform.InverseTransformPoint(point) : point;
             var vertices = mesh.vertices;
             index = ClosestPointIndex(p, vertices);
             var res = meshTransform ? meshTransform.TransformPoint(vertices[index]) : vertices[index];
             return res;
         }


         public static Vector3 ClosestPointOnMesh(Mesh mesh, Vector3 point, Transform meshTransform = null)
         {
             var res = ClosestVertex(mesh, point, out int vertexIndex, meshTransform);
             Debug.Log(VertexTriangles(vertexIndex, mesh.vertices, mesh.triangles).Length);
             return res;
         }

         public static int[] VertexTriangles(int vertexIndex, Vector3[] vertices, int[] triangles)
         {
             List<int> res = new List<int>();
             for (int i = 0; i < triangles.Length; i++)
             {
                 var v = vertices[triangles[i]];
                 if (vertices[vertexIndex] == v)
                 {
                     res.Add(Mathf.FloorToInt(i / 3f) * 3);
                 }
             }

             return res.ToArray();
         }

         
     }
 }