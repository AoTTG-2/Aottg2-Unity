using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utility;

namespace ApplicationManagers
{
    /// <summary>
    /// Handles automatic snapshots that are taken on kills.
    /// </summary>
    class SnapshotManager : MonoBehaviour
    {
        private static SnapshotManager _instance;
        public static readonly string SnapshotPath = FolderPaths.Snapshots;
        private static readonly string SnapshotTempPath = FolderPaths.Snapshots + "/Temp";
        private static readonly string SnapshotFilePrefix = "Snapshot";
        private static readonly int MaxSnapshots = 500;
        private static int _currentSnapshotSaveId = 0;
        private static int _maxSnapshotSaveId = 0;
        private static int[] _damages = new int[MaxSnapshots];

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
            ClearTemp();
        }

        private void OnApplicationQuit()
        {
            ClearTemp();
        }

        private static void ClearTemp()
        {
            if (Directory.Exists(SnapshotTempPath))
            {
                try
                {
                    Directory.Delete(SnapshotTempPath, true);
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Error deleting snapshot temp folder: {0}", e.Message));
                }
            }
        }

        private static string GetFileName(int snapshotId)
        {
            string name = SnapshotFilePrefix + snapshotId.ToString();
            return name;
        }

        public static void AddSnapshot(Texture2D texture, int damage)
        {
            // throwing an exception here might cause critical mem leak, so we do a broad try-except
            try
            {
                if (!Directory.Exists(SnapshotTempPath))
                    Directory.CreateDirectory(SnapshotTempPath);
                File.WriteAllBytes(SnapshotTempPath + "/" + GetFileName(_currentSnapshotSaveId), SerializeSnapshot(texture));
                _damages[_currentSnapshotSaveId] = damage;
                _currentSnapshotSaveId += 1;
                _maxSnapshotSaveId += 1;
                _maxSnapshotSaveId = Math.Min(_maxSnapshotSaveId, MaxSnapshots);
                if (_currentSnapshotSaveId >= MaxSnapshots)
                    _currentSnapshotSaveId = 0;
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Exception while adding snapshot: {0}", e.Message));
            }
        }

        // this is faster than encoding to png, so we use this to save temp snapshot during gameplay
        private static byte[] SerializeSnapshot(Texture2D texture)
        {
            Color32[] pixels = texture.GetPixels32();
            byte[] bytes = new byte[pixels.Length * 3 + 8];
            int index = 0;
            foreach (byte b in BitConverter.GetBytes(texture.width))
            {
                bytes[index] = b;
                index++;
            }
            foreach (byte b in BitConverter.GetBytes(texture.height))
            {
                bytes[index] = b;
                index++;
            }
            foreach (Color32 color in pixels)
            {
                bytes[index] = color.r;
                bytes[index + 1] = color.g;
                bytes[index + 2] = color.b;
                index += 3;
            }
            return bytes;
        }

        private static Texture2D DeserializeSnapshot(byte[] bytes)
        {
            int width = BitConverter.ToInt32(bytes, 0);
            int height = BitConverter.ToInt32(bytes, 4);
            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
            int index = 8;
            Color32[] pixels = new Color32[(bytes.Length - 8) / 3];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color32(bytes[index], bytes[index + 1], bytes[index + 2], 255);
                index += 3;
            }
            texture.SetPixels32(pixels);
            texture.Apply();
            return texture;
        }

        public static void SaveSnapshotFinish(Texture2D texture, string fileName)
        {
            if (!Directory.Exists(SnapshotPath))
                Directory.CreateDirectory(SnapshotPath);
            File.WriteAllBytes(SnapshotPath + "/" + fileName, texture.EncodeToPNG());
        }

        public static int GetDamage(int index)
        {
            if (index >= _maxSnapshotSaveId)
                return 0;
            return _damages[index];
        }

        public static Texture2D GetSnapshot(int index)
        {
            if (index >= _maxSnapshotSaveId)
                return null;
            string filePath = SnapshotTempPath + "/" + GetFileName(index);
            if (File.Exists(filePath))
            {
                Texture2D image = DeserializeSnapshot(File.ReadAllBytes(filePath));
                return image;
            }
            return null;
        }

        public static int GetLength()
        {
            return _maxSnapshotSaveId;
        }
    }
}
