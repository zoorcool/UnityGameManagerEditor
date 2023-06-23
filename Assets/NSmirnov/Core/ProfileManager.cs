using System;
using UnityEngine;

namespace NSmirnov.Core
{
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class ProfileManager : Singleton<ProfileManager>
    {
        public static event Action<bool> OnSoundStatusChangedEvent;
        public static event Action<bool> OnMusicStatusChangedEvent;
        public static event Action<bool> OnNotificationStatusChangedEvent;

        [HideInInspector] public bool isSoundEnabled = true;
        [HideInInspector] public bool isMusicEnabled = true;
        [HideInInspector] public bool isNotificationEnabled = true;

        private void OnEnable()
        {
            initProfileStatus();
        }

        public void initProfileStatus()
        {
            isSoundEnabled = PlayerPrefs.GetInt("isSoundEnabled", 0) == 0 ? true : false;
            isMusicEnabled = PlayerPrefs.GetInt("isMusicEnabled", 0) == 0 ? true : false;
            isNotificationEnabled = PlayerPrefs.GetInt("isNotificationEnabled", 0) == 0 ? true : false;

            if (!isSoundEnabled && OnSoundStatusChangedEvent != null)
            {
                OnSoundStatusChangedEvent.Invoke(isSoundEnabled);
            }
            if (!isMusicEnabled && OnMusicStatusChangedEvent != null)
            {
                OnMusicStatusChangedEvent.Invoke(isMusicEnabled);
            }
            if (!isNotificationEnabled && OnNotificationStatusChangedEvent != null)
            {
                OnNotificationStatusChangedEvent.Invoke(isNotificationEnabled);
            }
        }
        public void ToggleSoundStatus()
        {
            isSoundEnabled = isSoundEnabled ? false : true;
            PlayerPrefs.SetInt("isSoundEnabled", isSoundEnabled ? 0 : 1);

            if (OnSoundStatusChangedEvent != null)
            {
                OnSoundStatusChangedEvent.Invoke(isSoundEnabled);
            }
        }
        public void ToggleMusicStatus()
        {
            isMusicEnabled = isMusicEnabled ? false : true;
            PlayerPrefs.SetInt("isMusicEnabled", isMusicEnabled ? 0 : 1);

            if (OnMusicStatusChangedEvent != null)
            {
                OnMusicStatusChangedEvent.Invoke(isMusicEnabled);
            }
        }
        public void ToggleNotificationStatus()
        {
            isNotificationEnabled = isNotificationEnabled ? false : true;
            PlayerPrefs.SetInt("isNotificationEnabled", isNotificationEnabled ? 0 : 1);

            if (OnNotificationStatusChangedEvent != null)
            {
                OnNotificationStatusChangedEvent.Invoke(isNotificationEnabled);
            }
        }
    }
}