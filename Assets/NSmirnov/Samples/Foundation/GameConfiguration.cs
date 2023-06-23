using Newtonsoft.Json;
using NSmirnov.Core.Foundation;
using NSmirnov.Samples.Foundation.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace NSmirnov.Samples.Foundation
{
    public class GameConfiguration : GameConfigurationBase<GameProperties>
    {
        public List<Weapon> Weapons = new List<Weapon>();
        public List<Armor> Armors = new List<Armor>();
        public List<Items.Resources> Resources = new List<Items.Resources>();
        public override void LoadGameConfiguration(GameConfigurationProfile.Item profile)
        {
            string LoadPath = profile.TypePath == ProfileType.Locale && profile.IsPersistentDataPath ? Application.persistentDataPath : profile.LoadPath;
            Debug.Log($"Profile: {profile.TypePath}\r\n\tLoadPath: {LoadPath}");

            switch (profile.TypePath)
            {
                case ProfileType.Resources:
                    properties = this.LoadJSONString<GameProperties>(System.IO.Path.Combine("Config", "properties"));
                    Weapons = this.LoadJSONString<List<Weapon>>(System.IO.Path.Combine("Config", "weapons"));
                    Armors = this.LoadJSONString<List<Armor>>(System.IO.Path.Combine("Config", "armors"));
                    Resources = this.LoadJSONString<List<Items.Resources>>(System.IO.Path.Combine("Config", "resources"));
                    break;

                case ProfileType.Locale:
                    properties = this.LoadJSONFile<GameProperties>(System.IO.Path.Combine(LoadPath, "properties.json"));
                    Weapons = this.LoadJSONFile<List<Weapon>>(System.IO.Path.Combine(LoadPath, "weapons.json"));
                    Armors = this.LoadJSONFile<List<Armor>>(System.IO.Path.Combine(LoadPath, "armors.json"));
                    Resources = this.LoadJSONFile<List<Items.Resources>>(System.IO.Path.Combine(LoadPath, "resources.json"));
                    break;
            }
        }

        public override void SaveGameConfiguration(GameConfigurationProfile.Item profile)
        {
            switch (profile.TypePath)
            {
                case ProfileType.Resources:
                    Properties.SaveJSONString(System.IO.Path.Combine(profile.BuildPath, "properties.json"));
                    Weapons.SaveJSONString(System.IO.Path.Combine(profile.BuildPath, "weapons.json"));
                    Armors.SaveJSONString(System.IO.Path.Combine(profile.BuildPath, "armors.json"));
                    Resources.SaveJSONString(System.IO.Path.Combine(profile.BuildPath, "resources.json"));
                    break;
                default:
                    string BuildPath = profile.IsPersistentDataPath ? Application.persistentDataPath : profile.BuildPath;

                    Properties.SaveJSONFile(System.IO.Path.Combine(BuildPath, "properties.json"));
                    Weapons.SaveJSONFile(System.IO.Path.Combine(BuildPath, "weapons.json"));
                    Armors.SaveJSONFile(System.IO.Path.Combine(BuildPath, "armors.json"));
                    Resources.SaveJSONFile(System.IO.Path.Combine(BuildPath, "resources.json"));
                    break;
            }
        }

        public IEnumerator LoadGameConfigurationAtServer(GameConfigurationProfile.Item profile)
        {
            Debug.Log(profile.LoadPath);

            yield return GET<GameProperties>(profile.LoadPath, "properties.json", response => properties = response);
            yield return GET<List<Weapon>>(profile.LoadPath, "weapons.json", response => Weapons = response);
            yield return GET<List<Armor>>(profile.LoadPath, "armors.json", response => Armors = response);
            yield return GET<List<Items.Resources>>(profile.LoadPath, "resources.json", response => Resources = response);
        }
        private IEnumerator GET<T>(string path, string subpath, Action<T> success)
        {
            var unityWebRequest = UnityWebRequest.Get(path + subpath);

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                T response = JsonConvert.DeserializeObject<T>(unityWebRequest.downloadHandler.text, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                success?.Invoke(response);
            }
        }
    }
}