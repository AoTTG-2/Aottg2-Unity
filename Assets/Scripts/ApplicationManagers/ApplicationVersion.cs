using System;
using System.Net;
using System.Text;
using Utility;
using UnityEngine;

namespace ApplicationManagers
{
    partial class ApplicationVersion
    {
        public static bool UseServerHash = false;

        public static System.Func<NetworkCredential> VersionProvider;
        public static System.Func<string, string> HashProvider;

        public static NetworkCredential GetVersion()
        {
            if (VersionProvider != null)
                return VersionProvider();

            return new NetworkCredential("TestVersion", "TestVersion");
        }

        public static string GetHashCode(string key)
        {
            if (HashProvider != null)
                return HashProvider(key);
            return string.Empty;
        }

        public static string GetHashKey(string key)
        {
            if (UseServerHash)
                return key;
            return string.Empty;
        }

        public static string GetSessionID()
        {
            return "eWire7HpInEhOO7r" + SystemInfo.deviceUniqueIdentifier;
        }
    }
}
