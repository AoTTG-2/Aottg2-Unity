using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace Replay
{
    class ReplayManager: MonoBehaviour
    {
        private static ReplayManager _instance;

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }
    }
}
