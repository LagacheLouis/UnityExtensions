using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Reflection;
using UnityEditorInternal;
using UnityEditor;
using Object = UnityEngine.Object;

namespace llagache.Editor
{
    public static class EditorHelpers
    {
        public static void OpenLockInspector(Object target)
        {
            var inspectorType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.InspectorWindow");
            var inspectorInstance = ScriptableObject.CreateInstance(inspectorType) as EditorWindow;
            inspectorInstance.Show();
            var prevSelection = Selection.activeGameObject;
            Selection.activeObject = target;
            var isLocked = inspectorType.GetProperty("isLocked", BindingFlags.Instance | BindingFlags.Public);
            isLocked.GetSetMethod().Invoke(inspectorInstance, new object[] { true });
            Selection.activeGameObject = prevSelection;
        }

        public static void FocusObject(Object obj)
        {
            EditorApplication.ExecuteMenuItem("Window/General/Inspector");
            AssetDatabase.OpenAsset(obj);
        }
        
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
        
        public static void Row(ref Rect rect, ref float y, float x, float width, int columns, int columnIndex)
        {
            float spacing = 10;
            var w = width / columns + spacing * 0.5f / columns;
            var rx = x + w * columnIndex;
            rect.Set(rx, y, w - spacing * 0.5f, EditorGUIUtility.singleLineHeight);
            if(columnIndex >= columns - 1) y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        
        public static void ToggleGizmos(bool value, params int[] classIDs)
        {
            var annotation = Type.GetType("UnityEditor.Annotation, UnityEditor");
            var classId = annotation.GetField("classID");
            var scriptClass = annotation.GetField("scriptClass");
         
            var annotationUtility = Type.GetType("UnityEditor.AnnotationUtility, UnityEditor");
            var getAnnotations = annotationUtility.GetMethod("GetAnnotations", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var setGizmoEnabled = annotationUtility.GetMethod("SetGizmoEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var setIconEnabled = annotationUtility.GetMethod("SetIconEnabled", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
         
            var annotations = (Array)getAnnotations.Invoke(null, null);
            foreach (var a in annotations)
            {
                var classIdValue = (int)classId.GetValue(a);
                var scriptClassValue = (string)scriptClass.GetValue(a);
                
                if(classIDs.Length > 0 && !classIDs.Contains(classIdValue)) continue;
         
                setGizmoEnabled.Invoke(null, new object[] { classIdValue, scriptClassValue, value ? 1 : 0, false });
                setIconEnabled.Invoke(null, new object[] { classIdValue, scriptClassValue, value ? 1 : 0 });
            }
        }
        
    }
}

