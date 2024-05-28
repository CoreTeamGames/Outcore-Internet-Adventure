using UnityEngine;
using TMPro;
using DiscordUnity;

public class DiscordReportSender : MonoBehaviour
{
    [SerializeField] DiscordReportSenderSettings _settings;
    [SerializeField] TMP_InputField _textInputField;
    [SerializeField] TMP_InputField _nameInputField;
    private DiscordClient client;
    [SerializeField] DiscordChannel _discordChannel;


    void CreateDiscord()
    {
        client = new DiscordClient();
        client.StartBot(_settings.BotToken);
        client.SetStatus(false, "game");
        client.OnClientOpened += ClientOpened;
        client.OnClientClosed += ClientClosed;
    }

    private void ClientOpened(object s, DiscordEventArgs e)
    {
        SendReport();
        
        Debug.Log("Client opened");
    }

    private void ClientClosed(object s, DiscordEventArgs e)
    {
        client.Stop();
        Debug.Log("Client closed");
    }

    void Update()
    {
        if (client.isOnline)
        {
            client.Update();
        }
    }

    public void SendReportToDiscord()
    {
        CreateDiscord();
    }

    void SendReport()
    {
        Debug.Log("Try send Report");
        string _report = ConstructReport();

        foreach (var server in _settings.DiscordChannels)
        {
            DiscordServer discordServer = null;
            foreach (var _discordServer in client.servers)
            {
                if (_discordServer.name.ToLower() == server.ServerName.ToLower())
                {
                    discordServer = _discordServer;
                    break;
                }
            }
            foreach (var channel in discordServer.channels)
            {
                if (channel.name.ToLower() == server.ChannelName.ToLower())
                {
                    if (discordServer != null)
                    channel.SendMessage(_report, false);
                }
            }
            Debug.Log("Sended: " + _report);
        }
    }

    string ConstructReport()
    {
        return $"A new report by {_nameInputField.text} sended now. \n Text of the report is {'"'}{_textInputField.text}{'"'}\n See it and fix bugs";
    }


}