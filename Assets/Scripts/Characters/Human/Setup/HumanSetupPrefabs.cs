using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters
{
    class HumanSetupPrefabs
    {
        private static string AccessoriesPath = "Human/Parts/Accessories/Prefabs/";

        public static string GetBackPrefab(string back)
        {
            return AccessoriesPath + "Back/Back" + back;
        }

        public static string GetHatPrefab(string hat)
        {
            return AccessoriesPath + "Hat/Hat" + hat;
        }

        public static string GetHeadPrefab(string head)
        {
            return AccessoriesPath + "Head/Head" + head;
        }
    }
}
