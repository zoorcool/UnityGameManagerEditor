using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSmirnov.Core.Review
{
    public abstract class BaseReview<T> : Singleton<T> where T : Component
    {
        [SerializeField] protected bool initOnStart = true;
        private void Start()
        {
            if (initOnStart)
            {
                Initialized();
            }
        }
        public void Initialized()
        {
            OnInitialized();
        }
        protected abstract void OnInitialized();
        public abstract void RequestFlow(Action onSuccess);
        public abstract void LaunchFlow();
    }
}