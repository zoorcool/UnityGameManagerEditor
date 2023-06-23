using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSmirnov.Core
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        GameObject g = new GameObject("Controller");
                        instance = g.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        public static bool IsInstance => instance != null;


        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}