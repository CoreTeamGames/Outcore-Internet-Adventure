using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]

public class LocalizeText : MonoBehaviour
{
    LocalizationService _textLocalizator;
    [SerializeField] string _lineId;
    public string LineId { get { return _lineId; } set { _lineId = value != null ? value : _lineId; } }
    [SerializeField] string _fileName;

    public void Awake()
    {
        _textLocalizator = GameObject.FindGameObjectWithTag("LocalizationService").GetComponent<LocalizationService>();
        if (_textLocalizator == null)
            Debug.LogError("Can't find Localization service!");
        else
        {
        Localize();
        _textLocalizator.onlanguageSelectedEvent += Localize;
        }
    }

    public void Localize()
    {
        TMP_Text _text = GetComponent<TMP_Text>();
        if (_textLocalizator.LineExist(_fileName, _lineId))
            _text.text = _textLocalizator.GetLocalizedLine(_fileName, _lineId);
        else
            _text.text = $"{_textLocalizator.CurrentLanguage.langName},{_textLocalizator.CurrentLanguage.langCode}, HAS NO LOCALIZATION KEY: {_lineId}! FILE NAME IS: {_fileName}";
    }

    public string ReturnLocalizedLine(string fileName, string lineKey)
    {
        return _textLocalizator.GetLocalizedLine(_textLocalizator.CurrentLanguage.path, fileName, lineKey);
    }
}
