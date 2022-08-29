using System.Collections.Generic;
using UnityEngine;

namespace llagache
{
    public static class GameObjectExtensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            T component = gameObject.GetComponent<T>();

            if (component == null)
                component = gameObject.AddComponent<T>();

            return component;
        }

        public static bool HasComponent<T>(this GameObject gameObject) where T : Component
        {
            return gameObject.GetComponent<T>() != null;
        }
        
        public static bool Contains(this LayerMask mask, int layer)
        {
            return mask == (mask | (1 << layer));
        }

        public static GameObject[] FindGameObjectsWithLayer(int layer)
        {
            var mask = LayerMask.GetMask(LayerMask.LayerToName(layer));
            return FindGameObjectsInLayerMask(mask);
        }
        
        public static GameObject[] FindGameObjectsInLayerMask(LayerMask mask)
        {
            var gameObjects = (GameObject[]) Object.FindObjectsOfType(typeof(GameObject));
            var res = new List<GameObject>();
            foreach (var go in gameObjects)
            {
                if (mask.Contains(go.layer))
                {
                    res.Add(go);
                }
            }
            return res.ToArray();
        }

    }
}