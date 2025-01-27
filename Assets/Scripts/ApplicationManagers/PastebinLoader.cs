﻿using UnityEngine;
using System.Collections;
using Utility;
using System.Collections.Generic;
using Events;
using SimpleJSONFixed;
using System;

namespace ApplicationManagers
{
    /// <summary>
    /// Loads managed text data from pastebin.
    /// </summary>
    public class PastebinLoader : MonoBehaviour
    {
        public static JSONNode Leaderboard;
        public static string Changelog;
        public static JSONNode Version;
        public static PastebinStatus Status = PastebinStatus.Loading;
        static PastebinLoader _instance;

        // consts
        static readonly string VersionURL = "https://pastebin.com/raw/txV4YVcr";
        static readonly string LeaderboardURL = "https://pastebin.com/raw/zptDi9T6";
        static readonly string ChangelogURL = "https://pastebin.com/raw/Lw47FLT5";
        static readonly string PlatformURL = "https://aottgrc.com/Aottg2/Version.json";

        public static void Init()
        {
            _instance = SingletonFactory.CreateSingleton(_instance);
        }

        public static void LoadPastebin()
        {
            _instance.StartCoroutine(_instance.LoadPastebinCoroutine());
        }

        IEnumerator LoadPastebinCoroutine()
        {
            Status = PastebinStatus.Loading;
            string[] urls = new string[] { VersionURL, LeaderboardURL, PlatformURL, ChangelogURL };
            string changelog = string.Empty;
            JSONNode[] nodes = new JSONNode[urls.Length];
            for (int i = 0; i < urls.Length; i++)
                nodes[i] = null;
            for (int i = 0; i < urls.Length; i++)
            {
                using (WWW www = new WWW(urls[i]))
                {
                    yield return www;
                    if (www.error == null)
                    {
                        try
                        {
                            if (i == urls.Length - 1) // changelog is text only
                                changelog = www.text;
                            else
                                nodes[i] = JSON.Parse(www.text);
                        }
                        catch (Exception e)
                        {
                            DebugConsole.Log("Error parsing pastebin JSON: " + e.Message);
                        }
                    }
                    else
                    {
                        Debug.Log("Failed to load pastebin link: " + www.error);
                        break;
                    }
                }
            }
            Version = nodes[0];
            Leaderboard = nodes[1];
            Changelog = changelog;
            if (Leaderboard != null && Changelog != null && Version != null)
                Status = PastebinStatus.Loaded;
            else
                Status = PastebinStatus.Failed;
        }
    }

    public enum PastebinStatus
    {
        Loading,
        Loaded,
        Failed
    }
}