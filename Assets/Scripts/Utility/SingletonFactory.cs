using System;
using UnityEngine;

namespace Utility
{
    class SingletonFactory : MonoBehaviour
    {
        public static T CreateSingleton<T>(T instance) where T : Component
        {
            if (instance != null)
            {
                Type t = typeof(T);
                throw new Exception(string.Format("Attempting to create duplicate singleton of {0}", t.Name));
            }
            return Util.CreateDontDestroyObj<T>();
        }
    }
}