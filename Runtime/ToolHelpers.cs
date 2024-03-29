﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine;

namespace llagache
{
    public static class ToolHelpers
    {
        public static T InstantiatePrefab<T>(T prefab) where T : Object
        {
#if UNITY_EDITOR
            return PrefabUtility.InstantiatePrefab(prefab) as T;
#else
            return Object.Instantiate(prefab);
#endif
        }

        public static Camera GetRenderCamera()
        {
            if (Application.isPlaying) return Camera.main;
#if UNITY_EDITOR
            var v = SceneView.lastActiveSceneView;
            if (v!=null) return v.camera;
#endif
            return Camera.main;
        }
        

      
    }

}