using UnityEngine;

public class Menu : Window
{
    [SerializeField] bool _pauseGameOnWindowShow = true;
    [SerializeField] bool _gamePausedBeforeWindowShow = false;

    private void Awake()
    {
        OnWindowShowEvent += OnShow;
        OnWindowHideEvent += OnHide;
    }

    private void OnShow()
    {
        _gamePausedBeforeWindowShow = TimeService.IsPaused;
        if (_pauseGameOnWindowShow && !_gamePausedBeforeWindowShow)
        {
            TimeService.PauseGame(true);
        }
    }

    private void OnHide()
    {
        if (_pauseGameOnWindowShow && !_gamePausedBeforeWindowShow)
        {
            TimeService.PauseGame(false);
        }
    }
}