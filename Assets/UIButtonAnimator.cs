using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class UIButtonAnimator : MonoBehaviour
{
    Image _image;
    TMP_Text _text;
    [SerializeField] OutcoreInternetAdventure.UI.UISettings _uISettings;
    AudioSource _source;
    float _xsize = 0.95f; 

    public void Awake()
    {
        _source = GameObject.FindGameObjectWithTag("UIAudioSource").GetComponent<AudioSource>();
        _image = gameObject.GetComponentInChildren<Image>();
        _text = gameObject.GetComponentInChildren<TMP_Text>();
        Initialize();
    }

    public void OnSelect()
    {
        _image.rectTransform.DOScaleX(1, _uISettings.ButtonDuration);
        _source.PlayOneShot(_uISettings.ButtonSelectSound);
        _image.DOColor(_uISettings.ButtonSelectedColor, _uISettings.ButtonDuration);
        _text.DOColor(_uISettings.ButtonSelectedColor, _uISettings.ButtonDuration);
    }
    public void OnDeselect()
    {
        _image.rectTransform.DOScaleX(_xsize, _uISettings.ButtonDuration);
        _image.DOColor(_uISettings.ButtonNormalColor, _uISettings.ButtonDuration);
        _text.DOColor(_uISettings.ButtonNormalColor, _uISettings.ButtonDuration);
    }
    public void OnSubmit()
    {
        _source.PlayOneShot(_uISettings.ButtonSubmitSound);
        _image.DOColor(_uISettings.ButtonSubmitedColor, _uISettings.ButtonDuration);
        _text.DOColor(_uISettings.ButtonSubmitedColor, _uISettings.ButtonDuration);
    }

    void Initialize()
    {
        _image.rectTransform.DOScaleX(_xsize, 0);
        _image.color = GetComponent<Button>().interactable? _uISettings.ButtonNormalColor: _uISettings.ButtonDisabledColor;
        _text.color = GetComponent<Button>().interactable ? _uISettings.ButtonNormalColor : _uISettings.ButtonDisabledColor;
    }
}