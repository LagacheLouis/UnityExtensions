using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace llagache.Editor
{
    public static class QualityOfLife
    {
        private static string GetProjectPath()
        {
            string path = Application.dataPath;
            path = path.Replace("/Assets", "");
            return path;
        }

#if UNITY_EDITOR_OSX
    [MenuItem("Shaker/Reveal in Finder")]
#else
        [MenuItem("Tools/QoL/Show in Explorer")]
#endif
        static void OpenInFileExplorer()
        {
            EditorUtility.RevealInFinder(GetProjectPath());
        }
        
        
        [MenuItem("Tools/QoL/Open Terminal")]
        static void OpenTerminal()
        {

            OpenTerminalFunc(GetProjectPath());
        }
        
        [MenuItem("Assets/Open in Terminal")]
        static void OpenAssetInTerminal()
        {
            var obj = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(obj);
            if(string.IsNullOrEmpty(path)) return;
            var dir = Directory.Exists(path) ? path : Path.GetDirectoryName(path);
            OpenTerminalFunc(dir);
        }
        
        [MenuItem("Assets/Open in Terminal", true, -1)]
        public static bool OpenAssetInTerminalValidate()
        {
            var obj = Selection.activeObject;
            var path = AssetDatabase.GetAssetPath(obj);
            return !string.IsNullOrEmpty(path);
        }

        private static void OpenTerminalFunc(string path)
        {
            var args = $"cd {path}";
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                var process = Process.Start("cmd.exe",$"/k {args}");
                Debug.Log(args);
            }
            else if (Application.platform == RuntimePlatform.OSXEditor)
            {
                Process.Start(@"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal", args);
            }
            else
            {
                Debug.LogWarning("Terminal shortcut is not supported on your plateform");
            }
        }
        
        public static T GetSerializedValue<T>(this PropertyDrawer propertyDrawer, SerializedProperty property)
        {
            object @object = propertyDrawer.fieldInfo.GetValue(property.serializedObject.targetObject);
            // UnityEditor.PropertyDrawer.fieldInfo returns FieldInfo:
            // - about the array, if the serialized object of property is inside the array or list;
            // - about the object itself, if the object is not inside the array or list;
 
            // We need to handle both situations.
            if (@object.GetType().GetInterfaces().Contains(typeof(IList<T>)))
            {
                int propertyIndex = int.Parse(property.propertyPath[property.propertyPath.Length - 2].ToString());
 
                return ((IList<T>) @object)[propertyIndex];
            }
            else
            {
                return (T) @object;
            }
        }
        
        [MenuItem("Tools/QOL/Open Persistent Data Path")]
        static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("GameObject/Unwrap",false, -1)]
        public static void Ungroup(MenuCommand menuCommand)
        {
            if (!ShouldExecute(menuCommand)) return;
            
            Undo.SetCurrentGroupName("Unwrap");
            int undo = Undo.GetCurrentGroup();
            
            var group = Selection.gameObjects[0].transform;

            List<Transform> children = new();
            int length = group.childCount;
            for (int i = length - 1; i >= 0; i--)
            {
                var child = group.GetChild(i);
                children.Add(child);
                Undo.SetTransformParent(child, group.parent, "Set GameObjects Parent");
            }
            Undo.DestroyObjectImmediate(group.gameObject);
            Undo.CollapseUndoOperations(undo);
            
            Selection.objects = children.ToArray();
        }

        [MenuItem("GameObject/Unwrap", true, -1)]
        public static bool UngroupValidate()
        {
            return Selection.gameObjects.Length > 0 && Selection.gameObjects[0].transform.childCount > 0;
        }

        private static bool ShouldExecute(MenuCommand menuCommand)
        {
            if (menuCommand.context == null) return true;

            if (menuCommand.context == Selection.activeObject) return true;

            return false;
        }
    }
}