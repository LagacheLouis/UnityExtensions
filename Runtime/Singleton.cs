using System;
using System.Collections.Generic;
using UnityEngine;

namespace llagache
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Inst
        {
            get
            {
                if(_instance) return _instance;
                _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        [SerializeField]
        private bool _dontDestroyOnLoad;

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this.GetComponent<T>();
                if(_dontDestroyOnLoad) DontDestroyOnLoad(this);
            }
        }
    }
}
