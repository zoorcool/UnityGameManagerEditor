using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace NSmirnov.Core.Billing
{
    public abstract class BaseInApp<T> : Singleton<T> where T : Component
    {
        [SerializeField] protected bool initOnStart = true;
        [SerializeField] protected string[] productIds;

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
        public abstract void CheckPurchasesAvailability();
        public abstract void GetProducts();
    }
}