using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NSmirnov.Core
{
    [RequireComponent(typeof(EventSystem))]
    public class InputManager : Singleton<InputManager>
    {
        static bool isTouchAvailable = true;
        private EventSystem eventSystem;

        private void Start()
        {
            eventSystem = GetComponent<EventSystem>();
        }

        public bool canInput(float delay = 0.25F, bool disableOnAvailable = true)
        {
            bool status = isTouchAvailable;
            if (status && disableOnAvailable)
            {
                DisableTouch();

                StopCoroutine("EnableTouchAfterDelay");
                StartCoroutine("EnableTouchAfterDelay", delay);
            }
            return status;
        }
        public void DisableTouchForDelay(float delay = 0.25F)
        {
            DisableTouch();

            StopCoroutine("EnableTouchAfterDelay");
            StartCoroutine("EnableTouchAfterDelay", delay);
        }
        public void DisableTouch()
        {
            isTouchAvailable = false;
            eventSystem.enabled = false;
        }
        public void EnableTouch()
        {
            isTouchAvailable = true;
            eventSystem.enabled = true;
        }
        public IEnumerator EnableTouchAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            EnableTouch();
        }
    }
}