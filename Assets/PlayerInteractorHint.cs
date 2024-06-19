using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(OutcoreInternetAdventure.Player.PlayerInteractor))]
public class PlayerInteractorHint : MonoBehaviour
{
    OutcoreInternetAdventure.Player.PlayerInteractor _playerInteractor;
    InputActionMap _inputActions;
    [SerializeField] GameObject _interactorHint;
    [SerializeField] TMP_Text _interactorKeyText;
    [SerializeField] InputActionReference _reference;
    [SerializeField] Vector2 _offset = Vector2.up;
    [SerializeField] Image _interactorKeyImage;
    Transform _firstRoot;
    Vector3 _firstPosition;
    TMP_Text[] _texts;
    Image[] _images;
    Vector3 _scale;


    float _duration = 0.4f;

    public void Awake()
    {
        _inputActions = gameObject.transform.parent.gameObject.GetComponentInChildren<PlayerInput>().currentActionMap;
        _playerInteractor = GetComponent<OutcoreInternetAdventure.Player.PlayerInteractor>();
        _texts = _interactorHint.GetComponentsInChildren<TMP_Text>();
        _images = _interactorHint.GetComponentsInChildren<Image>();
        _firstPosition = _interactorHint.transform.localPosition;
        _firstRoot = _interactorHint.transform.parent;
        _scale = _interactorKeyImage.transform.localScale;
        foreach (var text in _texts)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
        foreach (var image in _images)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        }
    }

    public void HintPress()
    {
        _interactorKeyImage.transform.DOScale(_scale * 1.3f, 0.25f).OnComplete(() =>
        {
            _interactorKeyImage.transform.DOScale(_scale, 0.25f);
        });
    }
    public void ShowHint()
    {
        _interactorKeyText.text = InputControlPath.ToHumanReadableString(_reference.action.bindings[0].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
        _interactorHint.transform.parent = _playerInteractor.Collision.transform;
        _interactorHint.transform.localPosition = new Vector3(_offset.x, _offset.y, _firstPosition.z);
        foreach (var text in _texts)
        {
            text.DOFade(1, _duration);
        }
        foreach (var image in _images)
        {
            image.DOFade(1, _duration);
        }
    }

    public void HideHind()
    {
        foreach (var text in _texts)
        {
            text.DOFade(0, _duration);
        }
        for (int i = 0; i < _images.Length; i++)
        {
            if (i == _images.Length)
            {
                _images[i].DOFade(0, _duration).OnComplete(() =>
                {
                    _interactorHint.transform.parent = _firstRoot;
                    _interactorHint.transform.localPosition = _firstPosition;
                });
            }
            else
                _images[i].DOFade(0, _duration);
        }
    }
}
