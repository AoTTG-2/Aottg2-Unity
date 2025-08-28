using Photon.Pun;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace CustomLogic
{
    /// <summary>
    /// Game functions such as spawning titans and managing game state.
    /// </summary>
    [CLType(Name = "Service", Abstract = true, Static = true)]
    partial class CustomLogicServiceBuiltin : BuiltinClassInstance
    {
        private static HashSet<string> _registeredServices;


        [CLConstructor]
        public CustomLogicServiceBuiltin()
        {
            foreach (var setting in SettingsManager.AdvancedSettings.Services.Value)
            {
                if (!_registeredServices.Contains(setting.Value))
                {
                    _registeredServices.Add(setting.Value);
                }
            }
        }

        private static bool IsAllowedToRun(string service)
        {
            if (PhotonNetwork.IsMasterClient == false) return false;
            if (SettingsManager.InGameCurrent.Misc.ServicesEnabled.Value == false) return false;
            if (_registeredServices.Contains(service) == false) return false;
            return true;
        }

        private static void CheckMe(string service)
        {
            if (IsAllowedToRun(service) == false) throw new Exception("Not allowed to run services, stop using this gamemode unless you know what you're doing.");
        }

        /// <summary>
        /// Check whether the user running CL can access this service
        /// Requirements:
        /// Host only
        /// Manually enabled in settings
        /// Service ip and port registered in settings to unique name (to avoid potential fault in momentary host switching)
        /// </summary>
        [CLMethod]
        public static bool CheckPermissions(string service)
        {
            return IsAllowedToRun(service);
        }

        //[CLMethod(description: "Sort the list using a custom method, expects a method with the signature int method(a,b)")]
        //public void SortCustom(UserMethod method)
        //{
        //    List.Sort((a, b) => (int)CustomLogicManager.Evaluator.EvaluateMethod(method, new object[] { a, b }));
        //}

        [CLMethod]
        public static void Get(string service, string route, UserMethod callback)
        {
            CheckMe(service);
            CustomLogicManager._instance.StartCoroutine(GetRequest($"{service}/{route}", callback));
        }

        [CLMethod]
        public static void Post(string service, string route, string data, UserMethod callback = null, string format = "application/json")
        {
            CheckMe(service);
            CustomLogicManager._instance.StartCoroutine(PostRequest($"{service}/{route}", data, callback, format));
        }

        [CLMethod]
        public static void Put(string service, string route, string data, UserMethod callback = null)
        {
            CheckMe(service);
            CustomLogicManager._instance.StartCoroutine(PutRequest($"{service}/{route}", data, callback));
        }

        [CLMethod]
        public static void Delete(string service, string route, UserMethod callback = null)
        {
            CheckMe(service);
            CustomLogicManager._instance.StartCoroutine(DeleteRequest($"{service}/{route}", callback));
        }


        static IEnumerator GetRequest(string uri, UserMethod callback)
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                string result = string.Empty;

                if (webRequest.result == UnityWebRequest.Result.Success)
                {
                    result = webRequest.downloadHandler.text;
                }
                else
                {
                    result = GetWebRequestFailureJSON(webRequest);
                }

                CustomLogicManager.Evaluator.EvaluateMethod(callback, new object[] { result, webRequest.result.ToString() });
            }
        }

        static IEnumerator PostRequest(string uri, string data, UserMethod callback = null, string format = "application/json")
        {
            using (UnityWebRequest www = UnityWebRequest.Post(uri, data, format))
            {
                yield return www.SendWebRequest();

                string result = string.Empty;

                if (www.result != UnityWebRequest.Result.Success)
                {
                    result = GetWebRequestFailureJSON(www);
                }
                else
                {
                    result = www.downloadHandler.text;
                }

                if (callback != null) CustomLogicManager.Evaluator.EvaluateMethod(callback, new object[] { result, www.result.ToString() });
            }
        }

        static IEnumerator PutRequest(string uri, string data, UserMethod callback = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Put(uri, data))
            {
                yield return www.SendWebRequest();

                string result = string.Empty;

                if (www.result != UnityWebRequest.Result.Success)
                {
                    result = GetWebRequestFailureJSON(www);
                }
                else
                {
                    result = www.downloadHandler.text;
                }

                if (callback != null) CustomLogicManager.Evaluator.EvaluateMethod(callback, new object[] { result, www.result.ToString() });
            }
        }

        static IEnumerator DeleteRequest(string uri, UserMethod callback = null)
        {
            using (UnityWebRequest www = UnityWebRequest.Delete(uri))
            {
                yield return www.SendWebRequest();

                string result = string.Empty;

                if (www.result != UnityWebRequest.Result.Success)
                {
                    result = GetWebRequestFailureJSON(www);
                }

                if (callback != null) CustomLogicManager.Evaluator.EvaluateMethod(callback, new object[] { result, www.result.ToString() });
            }
        }

        static string GetWebRequestFailureJSON(UnityWebRequest webRequest)
        {
            string status = $"\"status:\" {webRequest.result.ToString()}";
            string error = $"\"error:\" \"{webRequest.error.Replace("\"", "\\\"")}\"";

            return $"{{{status},{error}}}";
        }



    }
}
