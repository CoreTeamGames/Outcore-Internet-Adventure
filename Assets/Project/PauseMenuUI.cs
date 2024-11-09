using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace OutcoreInternetAdventure.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        public bool IsPaused { get; private set; }

        [SerializeField] UISettings uiSettings;
        [SerializeField] UnityEvent _onMenuShow;
        [SerializeField] UnityEvent _onMenuHide;
        [SerializeField] CanvasGroup _panel;
        [SerializeField] GameObject _pauseMenuObject;
        private float _currentGameTime;

        public void OnDestroy()
        {
            TimeService.SetTimeAsStandard();
        }

        public void PauseGame()
        {
            PauseGame(!IsPaused);
        }
        public void PauseGame(bool setPause = true)
        {
            IsPaused = setPause;
            if (setPause)
            {
            _currentGameTime = Time.timeScale;
                TimeService.PauseGame(true);
                ShowMenu();
            }
            else
            {
                TimeService.PauseGame(false);
                HideMenu();
            }
        }
        public void ShowMenu()
        {
            _pauseMenuObject.SetActive(true);
            DOTween.Kill(_panel);
            _panel.DOFade(1, uiSettings.MenuShowFadingDuration).SetUpdate(true).OnComplete(OnMenuShow);
        }
       
        public void HideMenu()
        {
            DOTween.Complete(_panel);
            _panel.DOFade(0, uiSettings.MenuShowFadingDuration).SetUpdate(true).OnComplete(OnMenuHide);
        }

        void OnMenuHide()
        {
            _onMenuHide?.Invoke();
            _pauseMenuObject.SetActive(false);
        }
        void OnMenuShow()
        {
            _onMenuShow?.Invoke();
            
        }

       
    }
}