using UnityEngine;
using Utility;
using Settings;
using ApplicationManagers;
using System.Collections;

namespace Events
{
    /// <summary>
    /// Defines a list of useful event callbacks for use by other managers.
    /// </summary>

    public delegate void OnPreLoadScene(SceneName sceneName);
    public delegate void OnLoadScene(SceneName sceneName);
    public delegate void OnFinishInit();
    public delegate void OnSecondTick();

    class EventManager: MonoBehaviour
    {
        private static EventManager _instance;
        public static event OnPreLoadScene OnPreLoadScene;
        public static event OnLoadScene OnLoadScene;
        public static event OnFinishInit OnFinishInit;
        public static event OnSecondTick OnSecondTick;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            _instance.StartCoroutine(_instance.SecondTick());
        }

        public static void InvokePreLoadScene(SceneName sceneName)
        {
            OnPreLoadScene?.Invoke(sceneName);
        }

        public static void InvokeLoadScene(SceneName sceneName)
        {
            OnLoadScene?.Invoke(sceneName);
        }

        public static void InvokeFinishInit()
        {
            OnFinishInit?.Invoke();
        }

        private IEnumerator SecondTick()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                OnSecondTick?.Invoke();
            }
        }
    }
}
