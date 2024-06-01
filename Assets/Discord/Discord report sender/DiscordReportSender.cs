using OutcoreInternetAdventure.Network;
using UnityEngine.Events;
using Lumpn.Discord;
using UnityEngine;
using TMPro;

public class DiscordReportSender : MonoBehaviour
{
    [SerializeField] UnityEvent _onMessageSend;
    [SerializeField] UnityEvent _onMessageFailToSend;
    [SerializeField] TMP_InputField _textInputField;
    [SerializeField] TMP_InputField _nameInputField;
    [SerializeField] UnityEngine.UI.Button _reportMenuButton;
    [SerializeField] WebhookData webhookData;

    Webhook webhook;

    public void TurnButton()
    {
        _reportMenuButton.interactable = NetworkService.CheckInternetConnection();
    }

    public void SendReportToDiscord()
    {
        if (NetworkService.CheckInternetConnection())
        {
            Debug.Log("Network is avaliable. Try send Report.");
            webhook = webhookData.CreateWebhook();
            var embed = new Embed()
                .SetTitle($"A new report by {_nameInputField.text} sended now.")
                .SetColor(new Color32(255, 0, 0, 255))
                .SetDescription($"Text of the report is {'"'}{_textInputField.text}{'"'}.\nSee it and fix bugs.");
            StartCoroutine(webhook.Send(embed));
            _onMessageSend?.Invoke();
        }
        else
        {
            Debug.LogError("Network isn't avaliable");
            _onMessageFailToSend?.Invoke();
        }
    }
}