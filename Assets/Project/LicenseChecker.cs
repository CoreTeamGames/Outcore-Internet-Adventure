using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;

public class LicenseChecker : MonoBehaviour
{
    [SerializeField] private UnityEvent _onCheckSuccessfulEvents;
    [SerializeField] private CanvasGroup _notAllowedScreen;
    [SerializeField] private CanvasGroup _allowedScreen;
    [SerializeField] private float _fadeDuration = 0.5f;
    [SerializeField] private string _code;
    [SerializeField] string[] _accessedUserNames;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] bool _enable1stStepOfChecking = true;
    [SerializeField] bool _isBuild = true;
    public string[] AccessedUserNames { get { return _accessedUserNames; } }
    public string UserName { get { return Environment.UserName; } }
    public void Awake()
    {
        _code = Hash128.Compute(_code).ToString();
        Debug.Log(UserName);
        if (!_isBuild)
        {
            OnCheckSuccessful();
            return;
        }

        if (_enable1stStepOfChecking)
        {
            bool _allowedToBetaTest = false;
            //check for access 
            foreach (var userName in _accessedUserNames)
            {
                if (userName == UserName)
                {
                    _allowedToBetaTest = true;
                    _allowedScreen.interactable = true;
                    _allowedScreen.DOFade(1, _fadeDuration);
                    Debug.Log("You've complete 1-st step of checking");
                    break;
                }
            }
            if (!_allowedToBetaTest)
            {
                Debug.Log("!!You Aren't allowed to beta testing!! Please contact the CoreTeam Games if this is mistake, else please delete build");
                _notAllowedScreen.interactable = true;
                _notAllowedScreen.DOFade(1, _fadeDuration);
            }
        }
        else
        {
            _allowedScreen.interactable = true;
            _allowedScreen.DOFade(1, _fadeDuration);
            Debug.Log("You've complete 1-st step of checking");
        }
    }

    public void StartCheckingAccess()
    {
        bool isAllowed = CheckAccessCode(_code, _inputField.text);
        if (isAllowed)
        {
            OnCheckSuccessful();
        }
        else
        {
            OnCheckFailed();
        }
    }

    public bool CheckAccessCode(string code, string currentCode)
    {
        return code == Hash128.Compute(currentCode).ToString();
    }

    public void CloseGame()
    {
        Application.Quit();
    }
    public void LoadScene(string scene) => SceneManager.LoadScene(scene);
    public void OpenURL(string URL) => Application.OpenURL(URL);

    private void OnCheckSuccessful()
    {
        _onCheckSuccessfulEvents?.Invoke();
    }
    private void OnCheckFailed()
    {

    }
}