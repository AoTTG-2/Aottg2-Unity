﻿using System;
using System.Net;
using System.Text;
using Utility;

namespace ApplicationManagers
{
    class ApplicationVersion
    {
        public static bool UseServerHash = false;

        public static NetworkCredential GetVersion()
        {
            return new NetworkCredential("TestVersion", "TestVersion");
        }

        public static string GetHashCode(string key)
        {
            return string.Empty;
        }

        public static string GetHashKey(string key)
        {
            if (UseServerHash)
                return key;
            return string.Empty;
        }
    }
}
