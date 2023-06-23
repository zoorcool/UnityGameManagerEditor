using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NSmirnov.Core
{
    [RequireComponent(typeof(DontDestroyOnLoad), typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        [Serializable]
        public class AudioItem
        {
            public string name;
            public string description;
            public AudioClip audio;
        }

        private AudioSource source;

        [SerializeField] private List<AudioItem> items;

        private void OnEnable()
        {
            source = GetComponent<AudioSource>();
        }

        public void PlaySound(string name)
        {
            if (ProfileManager.Instance.isSoundEnabled && items.Any(_ => _.name == name))
            {
                AudioItem item = items.FirstOrDefault(_ => _.name == name);

                if (item != null)
                {
                    source.PlayOneShot(item.audio);
                }
            }
        }
    }
}