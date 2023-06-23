using NSmirnov.Core.Foundation;
using NSmirnov.Core;
using NSmirnov.Samples.Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace NSmirnov.Samples
{
    public class LoaderManager : MonoBehaviour, ILoaderManager<GameManager, GameConfiguration, GameProperties>
    {
        public GameManager gameManager => GameManager.Instance;

        [SerializeField] private GameConfigurationProfile profiles;
        [SerializeField] private ProfileType profileType = ProfileType.Locale;
        [SerializeField] private string lang = "ru";
        [SerializeField, Space(10)] private string preloadLabel;
        [SerializeField] private UnityEvent<float> progressEvent;

        private AsyncOperationHandle downloadHandle;
        private AsyncOperation asyncOperation;

        private IEnumerator Start()
        {
            if (!PlayerPrefs.HasKey("lang"))
            {
                PlayerPrefs.SetString("lang", lang);
            }

            downloadHandle = Addressables.DownloadDependenciesAsync(preloadLabel, false);
            float progress = 0;

            Debug.Log(downloadHandle.GetDownloadStatus().Percent);

            while (downloadHandle.Status == AsyncOperationStatus.None)
            {
                float percentageComplete = downloadHandle.GetDownloadStatus().Percent;

                if (percentageComplete > progress * 1.1)
                {
                    progress = percentageComplete;
                    progressEvent.Invoke(progress);
                }

                yield return null;
            }

            Addressables.Release(downloadHandle);

            if (profileType == ProfileType.Remote)
            {
                yield return gameManager.Config.LoadGameConfigurationAtServer(profiles.items.FirstOrDefault(_ => _.TypePath == profileType));
            }
            else
            {
                gameManager.Config.LoadGameConfiguration(profiles.items.FirstOrDefault(_ => _.TypePath == profileType));
            }

            yield return loadScene("Main");
        }
        private IEnumerator loadScene(string sceneName)
        {
            asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;

            while (!asyncOperation.isDone)
            {
                if (asyncOperation.progress >= 0.9f)
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return new WaitForSeconds(.15f);
            }
        }
    }
}
