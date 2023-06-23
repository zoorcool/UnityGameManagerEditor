using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace NSmirnov.Core
{
    [RequireComponent(typeof(DontDestroyOnLoad))]
    public class API : Singleton<API>
    {
        [Serializable]
        public class InstallationRequest
        {
            public string device { get; set; }
            //public string game { get; set; }
        }
        [Serializable]
        public class InstallationResponse
        {
            public string access_token { get; set; }
            public double exp { get; set; }
            public int eshopId { get; set; }
            public bool eshopProd { get; set; }
        }
        [System.Serializable]
        public class ServerItem
        {
            [System.Serializable]
            public enum ServerType
            {
                local,
                dev,
                prod,
            }
#if UNITY_EDITOR
            [HideInInspector] public string name;
#endif

            public ServerType type;
            public bool isHttps = false;
            public bool customePort = false;
            public string ip = "127.0.0.1";
            public string port = "8080";
            public string path => $"{(isHttps ? "https" : "http")}://{ip}{(customePort ? ":" + port : "")}/";
        }
        [SerializeField] private bool initOnStart = true;
        [SerializeField] private List<ServerItem> servers = new List<ServerItem>();
        [SerializeField] private ServerItem.ServerType serverType;
        [SerializeField] private string language = "ru";
        [SerializeField] private string uid = "";
        public string lang => language;
        private ServerItem server;
        public bool is_dev => serverType == ServerItem.ServerType.dev;
        public bool is_prod => serverType == ServerItem.ServerType.prod;
        public string path => server.path;
        public InstallationResponse installationResponse { get; private set; }
        public bool IsInstallation { get; private set; }
        public event Action OnConnectionError;
        private void Start()
        {
            server = servers.FirstOrDefault(_ => _.type == serverType);

            if (PlayerPrefs.HasKey("language"))
            {
                language = PlayerPrefs.GetString("language");
            }
            else
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Russian:
                    case SystemLanguage.Belarusian:
                        language = "ru";
                        break;
                    default:
                        language = "en";
                        break;
                }

                PlayerPrefs.SetString("language", language);
            }

            if (initOnStart)
            {
                StartCoroutine(Installation(new InstallationRequest
                {
                    device = SystemInfo.deviceUniqueIdentifier,
                    //game = Application.identifier,
                }, response =>
                {
                    installationResponse = response;
                    IsInstallation = true;
                }));
            }
        }
        public IEnumerator Installation(InstallationRequest request, Action<InstallationResponse> success = null)
        {

#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(uid))
            {
                request.device = uid;
            }
#endif

            var unityWebRequest = new UnityWebRequest(path + "/api/jwt/installation", "POST");
            string request_json = JsonConvert.SerializeObject(request);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(request_json);
            unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            unityWebRequest.uploadHandler.contentType = "application/json";
            unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept-Language", language);
            unityWebRequest.timeout = 30; unityWebRequest.redirectLimit = 1;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Log(unityWebRequest.downloadHandler.text);

                installationResponse = JsonConvert.DeserializeObject<InstallationResponse>(unityWebRequest.downloadHandler.text, new JsonBooleanConverter());

                IsInstallation = true;

                success?.Invoke(installationResponse);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);
            }
        }

        public IEnumerator GET<T>(string subpath, Action<T> success, Action error = null)
        {
            var unityWebRequest = UnityWebRequest.Get(path + subpath);
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {installationResponse.access_token}");
            unityWebRequest.SetRequestHeader("Accept-Language", language);

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Log(unityWebRequest.downloadHandler.text);

                T response = JsonConvert.DeserializeObject<T>(unityWebRequest.downloadHandler.text, new JsonBooleanConverter());

                success?.Invoke(response);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);
                error?.Invoke();
            }
        }

        public IEnumerator POST(string subpath, Action success, Action error = null)
        {
            var unityWebRequest = new UnityWebRequest(path + subpath, "POST");
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {installationResponse.access_token}");
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept-Language", language);
            unityWebRequest.timeout = 30; unityWebRequest.redirectLimit = 1;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                if (unityWebRequest.downloadHandler != null)
                {
                    Log(unityWebRequest.downloadHandler.text);
                }

                success?.Invoke();
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);

                error?.Invoke();
            }
        }

        public IEnumerator POST<T>(string subpath, T request, Action success, Action error = null)
        {
            var unityWebRequest = new UnityWebRequest(path + subpath, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(request));
            unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            unityWebRequest.uploadHandler.contentType = "application/json";
            unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {installationResponse.access_token}");
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept-Language", language);
            unityWebRequest.timeout = 30; unityWebRequest.redirectLimit = 1;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                if (unityWebRequest.downloadHandler != null)
                {
                    Log(unityWebRequest.downloadHandler.text);
                }

                success?.Invoke();
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);

                error?.Invoke();
            }
        }

        public IEnumerator POST<T>(string subpath, Action<T> success, Action error = null)
        {
            var unityWebRequest = new UnityWebRequest(path + subpath, "POST");
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {installationResponse.access_token}");
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept-Language", language);
            unityWebRequest.timeout = 30; unityWebRequest.redirectLimit = 1;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Log(unityWebRequest.downloadHandler.text);

                T response = JsonConvert.DeserializeObject<T>(unityWebRequest.downloadHandler.text, new JsonBooleanConverter());

                success?.Invoke(response);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);

                error?.Invoke();
            }
        }

        public IEnumerator POST<T, T1>(string subpath, T request, Action<T1> success, Action error = null)
        {
            var unityWebRequest = new UnityWebRequest(path + subpath, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(JsonConvert.SerializeObject(request));
            unityWebRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            unityWebRequest.uploadHandler.contentType = "application/json";
            unityWebRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            unityWebRequest.SetRequestHeader("Authorization", $"Bearer {installationResponse.access_token}");
            unityWebRequest.SetRequestHeader("Content-Type", "application/json");
            unityWebRequest.SetRequestHeader("Accept-Language", language);
            unityWebRequest.timeout = 30; unityWebRequest.redirectLimit = 1;

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                Log(unityWebRequest.downloadHandler.text);

                T1 response = JsonConvert.DeserializeObject<T1>(unityWebRequest.downloadHandler.text, new JsonBooleanConverter());

                success?.Invoke(response);
            }
            else if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                ConnectionError();
            }
            else
            {
                Log(unityWebRequest.downloadHandler.text, true);

                error?.Invoke();
            }
        }
        private void ConnectionError()
        {
            OnConnectionError?.Invoke();
        }
        private void Log(string response, bool error = false)
        {
#if UNITY_EDITOR

            if (error)
            {
                Debug.LogError(response);
            }
            else
            {
                Debug.Log(response);
            }
#endif
        }
        private void OnValidate()
        {
#if UNITY_EDITOR
            if (servers != null && servers.Count > 0)
            {
                foreach (var server in servers)
                {
                    server.name = server.type.ToString();
                }
            }
#endif
        }
    }
}