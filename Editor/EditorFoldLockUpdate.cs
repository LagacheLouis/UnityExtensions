using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace llagache.Editor
{
    [UnityEditor.InitializeOnLoad]
    public static class EditorFoldLockUpdate{
 
        static EditorFoldLockUpdate(){
            UnityEditor.EditorApplication.update += Update;
        }
 
        static void Update(){
            if(Application.isPlaying) return;
            var gos = Object.FindObjectsOfType<MonoBehaviour>();
            foreach (var go in gos)
            {
                var @lock = go as IEditorFoldLock;
                if (@lock == null) continue;
                if(!@lock.IsEditorFoldLocked()) continue;
                if (!SceneHierarchyUtility.IsExpanded(go.gameObject)) continue;
                Selection.activeObject = go;
                SceneHierarchyUtility.SetExpanded(go.gameObject, false);
            }
        }
    }
}