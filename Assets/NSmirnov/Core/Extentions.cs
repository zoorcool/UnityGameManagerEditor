using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NSmirnov
{
    public static class Extentions
    {
        public static void ClearAllChild(this Transform target)
        {
            if (target.childCount > 0)
            {
                foreach (Transform child in target)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        public static void Activate(this GameObject target)
        {
            target.SetActive(true);
            target.transform.SetAsLastSibling();
        }
        public static void Deactivate(this GameObject target)
        {
            if (target.activeInHierarchy)
            {
                Animator animator = target.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.Play("Close");
                }
                else
                {
                    target.SetActive(false);
                }
            }
        }
        public static T LoadJSONFile<T>(this object current, string path) where T : class
        {
            if (File.Exists(path))
            {
                var file = new StreamReader(path);
                var fileContents = file.ReadToEnd();
                file.Close();
                return JsonConvert.DeserializeObject<T>(fileContents, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return null;
        }
        public static void SaveJSONFile<T>(this T data, string path) where T : class
        {
            var file = new StreamWriter(path);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            file.WriteLine(json);
            file.Close();
        }

        public static T LoadJSONString<T>(this object current, string path) where T : class
        {
            var textAsset = Resources.Load<TextAsset>(path);

            if (textAsset != null)
            {
                return JsonConvert.DeserializeObject<T>(textAsset.text, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            return null;
        }
        public static void SaveJSONString<T>(this T data, string path) where T : class
        {
            var file = new StreamWriter(path);
            var json = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            file.WriteLine(json);
            file.Close();
        }
        public static string GetDescription(this Enum enumType)
        {
            FieldInfo fi = enumType.GetType().GetField(enumType.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return enumType.ToString();
        }
        public static string GetEnumDescription(this Enum e)
        {
            var attribute = e.GetType().GetMember(e.ToString())[0]
                .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]
                as DescriptionAttribute;

            return attribute.Description;
        }
        public static string GetEnumDescriptionOrName(this Enum e)
        {
            var name = e.ToString();
            var memberInfo = e.GetType().GetMember(name)[0];
            var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);

            if (!attributes.Any())
                return name;

            return (attributes[0] as DescriptionAttribute).Description;
        }
        public static string GetMemberDescription<T>(this T t, string memberName) where T : class
        {
            var memberInfo = t.GetType().GetMember(memberName)[0];
            var attribute = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0] as DescriptionAttribute;
            return attribute.Description;
        }
        public static string GetMemberDisplayName<T>(this T t, string memberName) where T : class
        {
            var memberInfo = t.GetType().GetMember(memberName)[0];
            var attribute = memberInfo.GetCustomAttributes(typeof(DisplayNameAttribute), inherit: false)[0] as DisplayNameAttribute;
            return attribute.DisplayName;
        }
        public static string GetClassDescription<T>(this T t) where T : class
        {
            var attribute = t.GetType().GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0]
                as DescriptionAttribute;

            return attribute.Description;
        }
    }
}