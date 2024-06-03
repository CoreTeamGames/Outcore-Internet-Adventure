using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class DiscordIntegrate : MonoBehaviour
{
    public long applicationID = 1171783797776338954;
    [Space]
    public string largeImage;

    private long time;
    [SerializeField] private List<DiscordActivityInfoChanger> _discordActivityChangers;
    public Discord.Discord discord;
    public Discord.ActivityManager activityManager;

    public void LateUpdate()
    {
        if (discord != null)
            discord.RunCallbacks();
    }

    public void OnLevelWasLoaded(int level)
    {
        Awake();
    }

    public void Start()
    {
        UnityEditor.EditorApplication.playModeStateChanged += EditorAppQuit;
    }

    public void Awake()
    {
        if (OutcoreInternetAdventure.Network.NetworkService.CheckInternetConnection())
        {
            discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);
            time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            _discordActivityChangers = new List<DiscordActivityInfoChanger>();
            var objects = GameObject.FindGameObjectsWithTag("DiscordActivityChanger");
            foreach (var item in objects)
            {
                _discordActivityChangers.Add(item.GetComponent<DiscordActivityInfoChanger>());
            }
            foreach (var changer in _discordActivityChangers)
            {
                changer.OnChangeDiscordActivity += UpdateStatus;
            }
        }
    }

    public void UpdateStatus(DiscordActivityInfoChanger.DiscordActivity activity)
    {
        // Update Status every frame
        try
        {
            activityManager = discord.GetActivityManager();
            var _activity = new Discord.Activity
            {
                Details = $"{activity.CurrentPhase}, level is: {activity.SceneNameInActivity}",


                //Playing "storymode", current part is "DesktopX"
                State = $"Current phase is: {activity.DescriptionOfPhase}",
                Assets =
                {
                    LargeImage = largeImage,

                },
                Timestamps =
                {
                    Start = time
                }
            };

            activityManager.UpdateActivity(_activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
                else
                    Debug.Log("Activity changed");
            });
        }
        catch
        {
            // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }

    void EditorAppQuit(UnityEditor.PlayModeStateChange state)
    {
        if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
        {
            OnApplicationQuit();
        }
    }

    public void OnApplicationQuit()
    {
        activityManager.ClearActivity((res) =>
        {
            if (res != Discord.Result.Ok) Debug.LogWarning("Failed clear activity!");
            else
                Debug.Log("Clear activity");
        });
    }
}
