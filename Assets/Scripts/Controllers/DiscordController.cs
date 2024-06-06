using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Characters;
using Settings;
using Photon.Pun;
using GameManagers;

public class DiscordController : MonoBehaviour
{

    public Discord.Discord discord;

    private static bool instanceExists;
    private string largeImage = "aottg2-logo1";
    private long time;
    private GameObject player;
    private void Awake()
    {
        if (!instanceExists)
        {
            instanceExists = true;
            DontDestroyOnLoad(gameObject);
        }
        else if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        discord = new Discord.Discord(1247921316913483888, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        time = DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = "Name: " + SettingsManager.ProfileSettings.Name.Value,
                State = "Playing Aottg2!",
                Assets =
                {
                    LargeImage = largeImage,
                },
                Timestamps =
                {
                    Start = time,
                }
            };
            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok)
                {
                    Debug.Log("Discord is not connected!");
                }
            });
        }
        catch
        {
            Debug.Log("Discord activity update Failed");
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        discord.Dispose();
    }
}
