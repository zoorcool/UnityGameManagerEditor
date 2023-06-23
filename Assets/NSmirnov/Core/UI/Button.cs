using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSmirnov.Core.UI
{
    public class Button : UnityEngine.UI.Button
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 1) return;
            }

            if (InputManager.Instance.canInput())
            {
                base.OnPointerClick(eventData);
            }
        }
    }
}