﻿using GameManagers;
using Map;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

static class PhotonExtensions
{
    public static void SetCustomProperty(this Player player, string key, object value)
    {
        if (player.GetCustomProperty(key) != value)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add(key, value);
            player.SetCustomProperties(properties);
        }
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

    public static bool GetBoolProperty(this RoomInfo room, string key, bool defaultValue = false)
    {
        if (room.CustomProperties.ContainsKey(key))
        {
            object obj = room.CustomProperties[key];
            if (obj != null && obj is bool)
                return (bool)obj;
        }
        return defaultValue;
    }

    public static bool HasSpawnPoint(this Player player)
    {
        var property = player.GetStringProperty(PlayerProperty.SpawnPoint, "null");
        return property != "null";
    }

    public static Vector3 GetSpawnPoint(this Player player)
    {
        var position = Vector3.zero;
        var property = player.GetStringProperty(PlayerProperty.SpawnPoint, "0,0,0");

        if (property.Contains(","))
        {
            var strArr = property.Split(',');
            position = new Vector3(float.Parse(strArr[0]), float.Parse(strArr[1]), float.Parse(strArr[2]));
        }
        else
        {
            if (MapLoader.IdToMapObject.TryGetValue(int.Parse(property), out MapObject mapObject))
                position = mapObject.GameObject.transform.position;
        }
        return position;
    }
}
