using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace OutcoreInternetAdventure.UI
{
    /// <summary>
    /// The base class for UI windows in game
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIWindow : MonoBehaviour
    {
        #region Variables
        #region Private variables
        [SerializeField] UnityEvent _onMenuStartsShow;
        [SerializeField] UnityEvent _onMenuStartsHide;
        [SerializeField] UnityEvent _onMenuShows;
        [SerializeField] UnityEvent _onMenuHides;
        [SerializeField] UISettings _uiSettings;
        [SerializeField] Image _panelImage;
        EventSystem _eventSystem;
        CanvasGroup _canvasGroup;
        GameObject _rememberedGameObject;
        #endregion
        #endregion

        #region Code
        public void Start()
        {
            _eventSystem = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
            if (_eventSystem == null) Debug.LogError($"Can't find EventSystem! Check tag {'"'}EventSystem{'"'} on GameObject with EventSystem component!");
            _panelImage.color = _uiSettings.PanelColor;
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void RememberSelectedGameObject()
        {
            if (_eventSystem != null)
                _rememberedGameObject = !_eventSystem.currentSelectedGameObject.transform.parent.name.Contains("Device")?_eventSystem.currentSelectedGameObject: _rememberedGameObject;
            else
                return;
        }

        public void SelectRememberedGameObject()
        {
            if (_eventSystem != null)
                _eventSystem.SetSelectedGameObject(_rememberedGameObject);
            else
                return;
        }
        /// <summary>
        /// This method shows the UI Window
        /// </summary>
        public void ShowWindow()
        {
            _onMenuStartsShow?.Invoke();
            _canvasGroup.interactable = true;
            _canvasGroup.DOFade(1, _uiSettings.MenuShowFadingDuration).OnComplete(() =>
            {
                _onMenuShows?.Invoke();
            });
        }
        /// <summary>
        /// This method hides the UI Window
        /// </summary>
        public void HideWindow()
        {
            _onMenuStartsHide?.Invoke();
            _canvasGroup.interactable = false;
            _canvasGroup.DOFade(0, _uiSettings.MenuShowFadingDuration).OnComplete(() =>
            {
                _onMenuHides?.Invoke();
            });
        }
        #endregion
    }
}