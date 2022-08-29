using UnityEngine;

namespace llagache
{
    public static class TransformExtensions
    {
        public static void DestroyChildren(this Transform t, float delay = 0)
        {
            int length = t.childCount;
            for (int i = length - 1; i >= 0; i--)
            {
                Object.Destroy(t.GetChild(i).gameObject, delay);
            }
        }

        public static void DestroyChildrenImmediate(this Transform t)
        {
            int length = t.childCount;
            for (int i = length - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(t.GetChild(i).gameObject);
            }
        }
        
        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x,
                globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
    }
}