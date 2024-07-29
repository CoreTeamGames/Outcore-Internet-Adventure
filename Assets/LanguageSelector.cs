using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LanguageSelector : MonoBehaviour
{
    LocalizationService _localizationService;
    [SerializeField] GameObject _buttonTemplate;
    [Min(0)]
    [SerializeField] float _spacing = 10f;

    public void Awake()
    {
        if (FindObjectOfType<LocalizationService>() != null)
        {
            _localizationService = FindObjectOfType<LocalizationService>();
        }
        else
        {
            Debug.LogError("Can't find LocalizationService!");
            gameObject.SetActive(false);
        }
    }

    public void SetUpscrollWiev()
    {
        Navigation _navigation = new Navigation();
        _navigation.mode = Navigation.Mode.Explicit;
        _navigation.selectOnLeft = _buttonTemplate.GetComponent<Button>().navigation.selectOnLeft;
        List<Button> _buttons = new List<Button>();
        _buttonTemplate.SetActive(true);
        for (int i = 0; i < _buttonTemplate.transform.parent.childCount; i++)
        {
            if (_buttonTemplate.transform.parent.GetChild(i).gameObject != _buttonTemplate)
                Destroy(_buttonTemplate.transform.parent.GetChild(i).gameObject);
        }
        GameObject _currentButton = _buttonTemplate;
        foreach (var language in _localizationService.Langs)
        {
            GameObject _button = Instantiate(_buttonTemplate, _buttonTemplate.transform.position, _buttonTemplate.transform.rotation, _buttonTemplate.gameObject.transform.parent);
            _buttons.Add(_button.GetComponent<Button>());
            _button.GetComponent<RectTransform>().localPosition = _currentButton.transform.localPosition - Vector3.up * (_currentButton.GetComponent<RectTransform>().sizeDelta.y + _spacing);
            _button.GetComponentInChildren<TMP_Text>().text = language.LangName;
            _button.GetComponent<Button>().onClick.AddListener(() => SelectLang(language.LangCode));
            _currentButton = _button;
        }
        _buttonTemplate.SetActive(false);
        for (int i = 0; i < _buttons.Count; i++)
        {
            _navigation.selectOnUp = i == 0 ? _buttons[_buttons.Count - 1] : _buttons[i - 1];
            _navigation.selectOnDown = i == _buttons.Count - 1 ? _buttons[0] : _buttons[i + 1];
            _buttons[i].navigation = _navigation;
        }
    }

    public void SelectLang(string languageCode)
    {
        foreach (var language in _localizationService.Langs)
        {
            if (languageCode.Trim().ToLower() == language.LangCode.Trim().ToLower())
            {
                _localizationService.ChangeLanguage(language);
            }
        }
    }

    int GetLangIndex(string _languageId)
    {
        int _lang = 0;
        _languageId = _languageId.ToLower();
        for (int i = 0; i < _localizationService.Langs.Count; i++)
        {
            if (_localizationService.Langs[i].LangCode.ToLower() == _languageId)
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
