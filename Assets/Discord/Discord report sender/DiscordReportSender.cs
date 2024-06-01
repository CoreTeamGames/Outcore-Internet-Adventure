using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Lumpn.Discord;

public class DiscordReportSender : MonoBehaviour
{

    [SerializeField] UnityEvent _onMessageSend;
    [SerializeField] TMP_InputField _textInputField;
    [SerializeField] TMP_InputField _nameInputField;

    public WebhookData webhookData;
    Webhook webhook;

    public void SendReportToDiscord()
    {
        Debug.Log("Try send Report");
        webhook = webhookData.CreateWebhook();


        var embed = new Embed()
            .SetTitle($"A new report by {_nameInputField.text} sended now.")
            .SetColor(new Color32(255,0,0,255))
            .SetDescription($"Text of the report is {'"'}{_textInputField.text}{'"'}.\nSee it and fix bugs.");

        StartCoroutine(webhook.Send(embed));
    }
}