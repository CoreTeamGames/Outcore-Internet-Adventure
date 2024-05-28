using TMPro;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LicenseChecker : MonoBehaviour
{
    [Serializable]
    public class AccessCodeAndEvent
    {
        public string Code { get { return _code; } }
        public UnityEvent Events { get { return _events; } }

        [SerializeField] string _code;
        [SerializeField] UnityEvent _events;
    }

    [SerializeField] AccessCodeAndEvent[] _codeAndEvents;
    [SerializeField] string[] _accessedUserNames;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] bool _enable1stStepOfChecking = true;
    public AccessCodeAndEvent[] CodeAndEvents { get { return _codeAndEvents; } }
    public string[] AccessedUserNames { get { return _accessedUserNames; } }
    public string UserName { get { return Environment.UserName; } }
    public void Awake()
    {
        Debug.Log(UserName);
        if (_enable1stStepOfChecking)
        {
            bool _allowedToBetaTest = false;
            //check for access 
            foreach (var userName in _accessedUserNames)
            {
                if (userName == UserName)
                {
                    _allowedToBetaTest = true;
                    Debug.Log("You're complete 1-st step of checking");
                    break;
                }
            }
            if (!_allowedToBetaTest)
            {
                Debug.Log("You're not allowed to alpha/beta test. Please contact with CoreTeam member");
                Application.Quit();
            }
        }
    }

    public void StartCheckingAccess()
    { CheckAccessCode(_codeAndEvents, _inputField.text); }

    public void CheckAccessCode(AccessCodeAndEvent[] accessCodesAndEvents, string currentCode)
    {
        bool _allowedToBetaTest = false;
        foreach (var accessCodeAndEvents in accessCodesAndEvents)
        {
            if (currentCode == accessCodeAndEvents.Code)
            {
                _allowedToBetaTest = true;
                Debug.Log("You're complete 2-st step of checking");
                accessCodeAndEvents.Events?.Invoke();
                break;
            }
        }
        if (!_allowedToBetaTest)
            Debug.Log("You wrote wrong access code or you're not allowed to alpha/beta test. Please contact with CoreTeam member to get a code for alpha/beta test. Writen code is: " + '"' + currentCode + '"');
    }

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);
    public void OpenURL(string URL) => Application.OpenURL(URL);
}
