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
        if (gameObject.GetComponent<TMP_Text>())
        {
            _text = gameObject.GetComponent<TMP_Text>();
        }
        else
            _text = gameObject.GetComponentInChildren<TMP_Text>();

        Initialize();
    }

    public void OnSelect()
    {
        _image.rectTransform.DOScaleX(1, _uISettings.ButtonDuration).SetUpdate(true);
        _source.PlayOneShot(_uISettings.ButtonSelectSound);
        _image.DOColor(_uISettings.ButtonSelectedColor, _uISettings.ButtonDuration).SetUpdate(true);
        _text.DOColor(_uISettings.ButtonSelectedColor, _uISettings.ButtonDuration).SetUpdate(true);
    }
    public void OnDeselect()
    {
        _image.rectTransform.DOScaleX(_xsize, _uISettings.ButtonDuration).SetUpdate(true);
        _image.DOColor(_uISettings.ButtonNormalColor, _uISettings.ButtonDuration).SetUpdate(true);
        _text.DOColor(_uISettings.ButtonNormalColor, _uISettings.ButtonDuration).SetUpdate(true);
    }
    public void OnSubmit()
    {
        _source.PlayOneShot(_uISettings.ButtonSubmitSound);
        _image.DOColor(_uISettings.ButtonSubmitedColor, _uISettings.ButtonDuration).SetUpdate(true);
        _text.DOColor(_uISettings.ButtonSubmitedColor, _uISettings.ButtonDuration).SetUpdate(true);
    }

    void Initialize()
    {
        _image.rectTransform.DOScaleX(_xsize, 0).SetUpdate(true);
        _image.color = GetComponent<Button>().interactable ? _uISettings.ButtonNormalColor : _uISettings.ButtonDisabledColor;
        _text.color = GetComponent<Button>().interactable ? _uISettings.ButtonNormalColor : _uISettings.ButtonDisabledColor;
    }
}