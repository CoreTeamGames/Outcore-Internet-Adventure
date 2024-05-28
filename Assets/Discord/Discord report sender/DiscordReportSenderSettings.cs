using UnityEngine;

[CreateAssetMenu(menuName = "Outcore SDK/Discord report sender settings")]
public class DiscordReportSenderSettings : ScriptableObject
{
    [System.Serializable]
    public class DiscordChannel
    {
        [SerializeField] string _serverName;
        [SerializeField] string _channelName;
        public string ServerName { get { return _serverName; } }
        public string ChannelName { get { return _channelName; } }
    }
    [SerializeField] DiscordChannel[] _discordChannels;
    [SerializeField] string _botToken = "MTE3MjEwOTEzMDg3NDUwNzM0NQ.GIJlLH.NGtPo7LTr1Ol9GhYJbKFU5bAky7xkcy0CETPE8";
    public DiscordChannel[] DiscordChannels { get { return _discordChannels; } }
    public string BotToken { get { return _botToken; } } 
}
