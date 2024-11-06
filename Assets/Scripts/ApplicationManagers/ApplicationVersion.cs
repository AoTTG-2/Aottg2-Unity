using System;
using System.Net;
using System.Text;
using Utility;

namespace ApplicationManagers
{
#if (UNITY_EDITOR || DEVELOPMENT_BUILD) && HAS_CREDENTIALS
    class ApplicationVersion : DevApplicationVersion { }
#else
    class ApplicationVersion
    {
        public static NetworkCredential GetVersion()
        {
            return new NetworkCredential("TestVersion", "TestVersion");
        }

        public static string GetHashCode(string str)
        {
            return string.Empty;
        }
    }
#endif
}
