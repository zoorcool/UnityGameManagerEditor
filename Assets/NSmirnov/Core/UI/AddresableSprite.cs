using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace NSmirnov.Core.UI
{
    public class AddresableSprite : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private string guid;
        [SerializeField] private float m_Alpha = 1;
        private bool isLoader;
        private Color m_Color;

        private AsyncOperationHandle<Texture2D> OnHandle;

        public event Action<AddresableSprite> OnBeginLoadEvent, OnEndLoadEvent;

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(guid) && !isLoader)
            {
                Load();
            }
        }
        private void Load()
        {
            StartCoroutine(TextureDownload(guid));
        }
        private IEnumerator TextureDownload(string guid)
        {
            OnHandle = Addressables.LoadAssetAsync<Texture2D>(guid);

            if (!OnHandle.IsDone)
                yield return OnHandle;

            if (OnHandle.Status == AsyncOperationStatus.Succeeded)
            {
                OnLoad(OnHandle.Result);
            }
            else
            {
                Addressables.Release(OnHandle);
            }
        }
        private void OnLoad(Texture2D texture)
        {
            OnBeginLoadEvent?.Invoke(this);

            if (image == null) image = GetComponent<Image>();

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            image.sprite = sprite;

            m_Color = image.color;

            m_Color.a = m_Alpha;
            image.color = m_Color;

            isLoader = true;

            OnEndLoadEvent?.Invoke(this);
        }
        public void SetGuid(string guid)
        {
            this.guid = guid;
            if (gameObject != null && gameObject.activeInHierarchy)
            {
                Load();
            }
        }

        public void SetGuid(string guid, float alpha)
        {
            m_Alpha = alpha;
            if (gameObject != null && gameObject.activeInHierarchy)
            {
                SetGuid(guid);
            }
        }

        public void SetSprite(Sprite sprite)
        {
            if (gameObject != null && gameObject.activeInHierarchy)
            {
                if (image == null) image = GetComponent<Image>();

                image.sprite = sprite;
                image.color = Color.white;
            }
        }
    }
}