using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

namespace llagache.Editor
{
    [System.Serializable]
    public class ShortcutManager
    {
        public List<Shortcut> shortcuts = new List<Shortcut>();

        public void Add(string name, string context, KeyCode keycode)
        {
            if (Get(name, context) == null)
            {
                shortcuts.Add(new Shortcut(name,context,keycode));
            }
        }

        public Shortcut Get(string name, string context)
        {
            return shortcuts.FirstOrDefault(x => x.name == name && x.context == context);
        }

        public bool OnKeyDown(string name, string context)
        {
            return Get(name, context)?.OnKeyDown == true;
        }

        public bool OnKeyUp(string name, string context)
        {
            return Get(name, context)?.OnKeyUp == true;
        }
        public bool OnKey(string name, string context)
        {
            return Get(name, context)?.OnKey == true;
        }

        public void Tick(Event e)
        {
            foreach (var shortcut in shortcuts)
            {
                shortcut.Check(e);
            }
        }

        public void SceneGUI(params string[] contexts)
        {
            var ctxShortcuts = shortcuts.FindAll(x => contexts.Contains(x.context));
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(15 + 45, 15, 200, 200));
            var rect = EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Shortcuts :");
            foreach (var shortcut in ctxShortcuts)
            {
                GUILayout.Label(shortcut.GetDescription());
            }
            EditorGUILayout.EndVertical();
            GUILayout.EndArea();
            Handles.EndGUI();
        }

        [System.Serializable]
        public class Shortcut
        {
            public string name;
            public string context;
            public KeyCode keyCode;

            private bool _onKeyDown;
            private bool _onKeyUp;
            private bool _onKey;

            public bool OnKeyDown => _onKeyDown;
            public bool OnKeyUp => _onKeyUp;
            public bool OnKey => _onKey;

            public Shortcut(string name, string context, KeyCode keyCode)
            {
                this.name = name;
                this.context = context;
                this.keyCode = keyCode;
            }

            public string GetDescription()
            {
                return "[" + keyCode.ToString() + "] " + name;
            }

            public void Check(Event e)
            {
                _onKeyDown = false;
                _onKeyUp = false;
                if(e.alt || e.control) return;
                switch (e.type)
                {
                    case EventType.KeyDown when e.keyCode == keyCode:
                         e.Use();
                        _onKeyDown = true;
                        _onKey = true;
                        break;
                    case EventType.KeyUp when e.keyCode == keyCode:
                        e.Use();
                        _onKeyUp = true;
                        _onKey = false;
                        break;
                }
            }
        }
    }
}
