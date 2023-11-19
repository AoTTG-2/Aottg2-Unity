using ExitGames.Client.Photon;
using System;
using UnityEngine;

namespace Photon
{
    class CustomSerialization
    {
        public static void Init()
        {
            Register();
        }

        internal static void Register()
        {
            PhotonPeer.RegisterType(typeof(Color), (byte)'C', SerializeColor, DeserializeColor);
        }

        public static object DeserializeColor(byte[] data)
        {
            var result = new Color();
            result.r = ((float)(int)data[0]) / 255f;
            result.g = ((float)(int)data[1]) / 255f;
            result.b = ((float)(int)data[2]) / 255f;
            result.a = ((float)(int)data[3]) / 255f;
            return result;
        }

        public static byte[] SerializeColor(object obj)
        {
            var c = (Color)obj;
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((int)(c.r * 255f));
            bytes[1] = (byte)((int)(c.g * 255f));
            bytes[2] = (byte)((int)(c.b * 255f));
            bytes[3] = (byte)((int)(c.a * 255f));
            return bytes;
        }
    }
}
