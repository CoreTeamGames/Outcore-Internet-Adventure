using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
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
        [SerializeField] GameObject[] _settingsMenus;
        [SerializeField] GameObject _settingsMenuButtonsRoot;
        [SerializeField] GameObject _menuWindow;
        [SerializeField] Image panelImage;
        [SerializeField] UnityEngine.EventSystems.EventSystem _system;
        [SerializeField] List<Button> _pauseMenuButtonsList;
        [SerializeField] List<Button> _settingsMenuButtonsList;
        private GameObject _selected;
        private Tween _tween;

        private Color NullAlpha { get { return new Color(uiSettings.PanelColor.r, uiSettings.PanelColor.g, uiSettings.PanelColor.b, 0); } }
        private float _currentGameTime;

        public void OnDestroy()
        {
            TimeService.SetTimeAsStandard();
        }

        void UnselectButton()
        {
            _system.SetSelectedGameObject(_selected);
        }
        public void SelectObject(GameObject gameObject)
        {
            _system.SetSelectedGameObject(gameObject);
        }
        void SelectButton()
        {
            _selected = _system.currentSelectedGameObject;
            _system.SetSelectedGameObject(_pauseMenuButtonsList[0].gameObject);
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
                TimeService.ChangeTime(0, uiSettings.MenuShowFadingDuration);
                ShowMenu();
            }
            else
            {
                TimeService.ChangeTime(_currentGameTime, uiSettings.MenuShowFadingDuration);
                HideMenu();
            }
        }
        public void ShowMenu()
        {
            _onMenuShow?.Invoke();
            panelImage.color = NullAlpha;
            _menuWindow.SetActive(true);
            SelectButton();
            DOTween.Kill(panelImage);
            panelImage.DOColor(uiSettings.PanelColor, uiSettings.MenuShowFadingDuration).SetUpdate(true).OnComplete(OnMenuShow);
        }
        void OnMenuShow()
        {
        }
        public void HideMenu()
        {
            _onMenuHide?.Invoke();
            DOTween.Complete(panelImage);
            panelImage.DOColor(NullAlpha, uiSettings.MenuShowFadingDuration).SetUpdate(true).OnComplete(OnMenuHide);
        }

        void OnMenuHide()
        {
            CloseSettings();
            UnselectButton();
            _menuWindow.SetActive(false);
        }

        public void OpenSettings()
        {
            SetInteractableMenuButtons(false);
            _settingsMenuButtonsRoot.SetActive(true);
            _system.SetSelectedGameObject(_settingsMenuButtonsList[0].gameObject);
        }

        public void CloseSettings()
        {
            SetInteractableMenuButtons(true);
            _settingsMenuButtonsRoot.SetActive(false);
            foreach (var button in _settingsMenuButtonsList)
            {
                button.gameObject.SetActive(true);
            }
            foreach (var menu in _settingsMenus)
            {
                menu.SetActive(false);
            }
            _system.SetSelectedGameObject(_pauseMenuButtonsList[0].gameObject);
        }

        void SetInteractableMenuButtons(bool interactable)
        {
            foreach (var button in _pauseMenuButtonsList)
            {
                button.interactable = interactable;
            }
        }
    }
}