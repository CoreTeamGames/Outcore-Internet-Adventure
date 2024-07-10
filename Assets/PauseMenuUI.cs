using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace OutcoreInternetAdventure.UI
{
    public class PauseMenuUI : UIWindow
    {
        public bool IsPaused { get; private set; }
        private float _currentGameTime;

        public void OnDestroy()
        {
            TimeService.SetTimeAsStandard();
        }

        public void OnDisable()
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
                TimeService.ChangeTime(0, UISettings.MenuShowFadingDuration);
                ShowWindow();
            }
            else
            {
                TimeService.ChangeTime(_currentGameTime, UISettings.MenuShowFadingDuration);
                HideWindow();
            }
        }
         
    }
}