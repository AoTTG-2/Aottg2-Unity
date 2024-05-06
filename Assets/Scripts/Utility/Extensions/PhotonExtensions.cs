using Characters;
using GameManagers;
using NUnit.Framework.Internal.Commands;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

static class PhotonExtensions
{
    public static void SetCustomProperty(this Player player, string key, object value)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        properties.Add(key, value);
        player.SetCustomProperties(properties);
    }

    public static void SetCustomProperties(this Player player, Dictionary<string, object> dictionary)
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
        foreach (string key in dictionary.Keys)
        {
            object value = dictionary[key];
            if (player.GetCustomProperty(key) != value)
                properties.Add(key, value);
        }
        if (properties.Count > 0)
            player.SetCustomProperties(properties);
    }

    public static object GetCustomProperty(this Player player, string key)
    {
        if (player.CustomProperties.ContainsKey(key))
            return player.CustomProperties[key];
        return null;
    }

    public static int GetIntProperty(this Player player, string key, int defaultValue = 0)
    {
        object obj = player.GetCustomProperty(key);
        if (obj != null && obj is int)
            return (int)obj;
        return defaultValue;
    }

    public static float GetFloatProperty(this Player player, string key, float defaultValue = 0)
    {
        object obj = player.GetCustomProperty(key);
        if (obj != null && obj is float)
            return (float)obj;
        return defaultValue;
    }

    public static bool GetBoolProperty(this Player player, string key, bool defaultValue = false)
    {
        object obj = player.GetCustomProperty(key);
        if (obj != null && obj is bool)
            return (bool)obj;
        return defaultValue;
    }

    public static string GetStringProperty(this Player player, string key, string defaultValue = "")
    {
        object obj = player.GetCustomProperty(key);
        if (obj != null && obj is string)
            return (string)obj;
        return defaultValue;
    }

    public static string GetStringProperty(this RoomInfo room, string key, string defaultValue = "")
    {
        if (room.CustomProperties.ContainsKey(key))
        {
            object obj = room.CustomProperties[key];
            if (obj != null && obj is string)
                return (string)obj;
        }
        return defaultValue;
    }

    public static bool HasSpawnPoint(this Player player)
    {
        var property = player.GetStringProperty(PlayerProperty.SpawnPoint, "null");
        return property != "null" && property.Contains(",");
    }

    public static Vector3 GetSpawnPoint(this Player player)
    {
        var property = player.GetStringProperty(PlayerProperty.SpawnPoint, "0,0,0");
        var strArr = property.Split(',');
        return new Vector3(float.Parse(strArr[0]), float.Parse(strArr[1]), float.Parse(strArr[2]));
    }

    public static GameObject GetMyPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv.IsMine)
                return player;
        }
        return null;
    }

    public static GameObject GetMyHuman()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            Human pv = player.GetComponent<Human>();
            if (pv.IsMine())
                return player;
        }
        return null;
    }

    public static GameObject GetPlayerFromID(int actorNumber)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in players)
        {
            PhotonView pv = player.GetComponent<PhotonView>();
            if (pv != null && pv.OwnerActorNr == actorNumber)
                return player;
        }
        return null;
    }

}
