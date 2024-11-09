using OutcoreInternetAdventure.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class Window : MonoBehaviour
{
    #region Variables
    [SerializeField] bool _isHidden = false;
    [SerializeField] bool _canCloseWindow = true;
    [SerializeField] UISettings _settings;
    CanvasGroup _group;
    #endregion

    #region Events
    public delegate void OnWindowShow();
    public delegate void OnWindowHide();

    public OnWindowShow OnWindowShowEvent;
    public OnWindowHide OnWindowHideEvent;
    #endregion

    #region Code
    public bool IsHidden => _isHidden;
    public UISettings Settings => _settings;
    public CanvasGroup Group => _group;
    #endregion

    #region Code
    public void Hide()
    {
        if (!_canCloseWindow)
            return;

        _isHidden = true;
        OnWindowHideEvent?.Invoke();

        _group.interactable = false;
        _group.blocksRaycasts = false;
        _group.DOFade(0, _settings.MenuShowFadingDuration);
    }

    public void Show()
    {
        _isHidden = false;
        OnWindowShowEvent?.Invoke();

        _group.interactable = true;
        _group.blocksRaycasts = true;
        _group.DOFade(1, _settings.MenuShowFadingDuration);
    }

    private void Awake()
    {
        _group = GetComponent<CanvasGroup>();
    }
    #endregion
}