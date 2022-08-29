#if UNITY_EDITOR
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
        
#if UNITY_EDITOR
        public static T CreateOrLoadScriptableObject<T>(string filename, string destination, bool forceNew = true) where T : ScriptableObject
        {

            string dirpath = "Assets/" + destination + "/";
            string filepath = dirpath + filename + ".asset";

            T asset = (T)AssetDatabase.LoadAssetAtPath(filepath, typeof(T));
            if (asset != null && !forceNew)
            {
                Debug.Log("Asset loaded");
                return asset;
            }
            asset = ScriptableObject.CreateInstance<T>();

            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }
            Debug.Log(filepath);
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(filepath);
            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            Debug.Log("Asset created");
            return asset;
        }
    }
#endif
   
}