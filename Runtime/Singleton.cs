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
                _instance = FindFirstObjectByType<T>();
                return _instance;
            }
        }
        
        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                this.gameObject.SetActive(false);
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this.GetComponent<T>();
            }
        }
    }
}
