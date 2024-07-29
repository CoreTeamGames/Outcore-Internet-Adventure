using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]

public class LocalizeText : MonoBehaviour
{
    LocalizationService _textLocalizator;
    [SerializeField] string _lineId;
    public string lineId { get => _lineId; set => _lineId = value; }
    [SerializeField] string _fileName;
    public List<string> variables;
    TMP_Text _text;

    public void Awake()
    {
        _text = GetComponent<TMP_Text>();
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
        if (_textLocalizator.LineExist(_fileName, _lineId))
        {
            string _localizedText = _textLocalizator.GetLocalizedLine(_fileName, _lineId);
            if (variables.Count > 0)
            {
                for (int i = 0; i < variables.Count; i++)
                {
                    if (_localizedText.Contains("{" + i + "}"))
                    {
                        _localizedText = _localizedText.Replace("{" + i + "}", variables[i]);
                    }
                }
            }
            _text.text = _localizedText;
        }
        else
            _text.text = $"{_textLocalizator.CurrentLanguage.LangName},{_textLocalizator.CurrentLanguage.LangCode}, HAS NO LOCALIZATION KEY: {_lineId}! FILE NAME IS: {_fileName}";
    }

    public string ReturnLocalizedLine(string fileName, string lineKey)
    {
        return _textLocalizator.GetLocalizedLine(_textLocalizator.CurrentLanguage.Path, fileName, lineKey);
    }
}
