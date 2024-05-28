using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageSelector : MonoBehaviour
{
    [SerializeField] TMP_Dropdown _dropdown;
    [SerializeField] LocalizationService _localizationService;

    public void Awake()
    {
        SetUpDropdown();
    }

    void SetUpDropdown()
    {
        _dropdown.ClearOptions();
        List<string> _languageNames = new List<string>();
        _localizationService.AddLangs();
        foreach (var item in _localizationService.langs)
        {
            _languageNames.Add(item.langName);
        }
        _dropdown.AddOptions(_languageNames);
        _dropdown.value = GetLangIndex(_localizationService.CurrentLanguage.langCode);
    }

    public void SelectLang()
    {
        for (int i = 0; i < _localizationService.langs.Capacity; i++)
        {
            if (_localizationService.langs[i].langName.ToLower() == _dropdown.options[_dropdown.value].text.ToLower())
            {
                _localizationService._currentLanguage = _localizationService.langs[i];
                _localizationService.onlanguageSelectedEvent?.Invoke();
                break;
            }
        }
    }

    int GetLangIndex(string _languageId)
    {
        int _lang = 0;
        _languageId = _languageId.ToLower();
        for (int i = 0; i < _localizationService.langs.Count; i++)
        {
            if (_localizationService.langs[i].langCode.ToLower() == _languageId)
            {
                _lang = i;
                break;
            }
        }
        if (_lang == 0)
            Debug.LogError("Can't find language");

        return _lang;
    }
}
