using UnityEngine.InputSystem.UI;
using UnityEngine;
using TMPro;
using System;

public class InGameConsole : MonoBehaviour, ILogger
{
    [SerializeField] TMP_Text _text;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] InputSystemUIInputModule _uiInputModule;
    [SerializeField] GameObject _consoleObject;

    private bool _logEnabled = true;
    private LogType _filterLogType;
    private CursorLockMode _cursorLockModeBeforeShowConsole;
    private bool _cursorVisibleBeforeShowConsole;

    public ILogHandler logHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool logEnabled { get => _logEnabled; set => _logEnabled = value; }
    public LogType filterLogType { get => _filterLogType; set => _filterLogType = value; }

    public void OpenConsole()
    {
        OpenConsole(!_consoleObject.activeInHierarchy);
    }

    public void OpenConsole(bool open)
    {
        if (open == _consoleObject.activeInHierarchy)
            return;

        if (open)
            ShowConsole();
        else
            HideConsole();
    }

    void ShowConsole()
    {
        _consoleObject.SetActive(true);
        _cursorLockModeBeforeShowConsole = Cursor.lockState;
        _cursorVisibleBeforeShowConsole = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideConsole()
    {
        _consoleObject.SetActive(false);
        Cursor.lockState = _cursorLockModeBeforeShowConsole;
        Cursor.visible = _cursorVisibleBeforeShowConsole;
    }

    public bool IsLogTypeAllowed(LogType logType)
    {
        throw new NotImplementedException();
    }

    public void Log(LogType logType, object message)
    {
        switch (logType)
        {
            case LogType.Log:
                Log(message);
                break;
            case LogType.Exception:
                LogException(new Exception(message.ToString()));
                break;
            case LogType.Warning:
                LogWarning(message);
                break;
            case LogType.Error:
                LogError(message);
                break;
            case LogType.Assert:
                // LogAssert(tag, message);
                break;
            default:
                break;
        }
    }

    public void Log(LogType logType, object message, UnityEngine.Object context)
    {
        switch (logType)
        {
            case LogType.Log:
                Log(message);
                break;
            case LogType.Exception:
                LogException(new Exception(message.ToString()));
                break;
            case LogType.Warning:
                LogWarning(message);
                break;
            case LogType.Error:
                LogError(message);
                break;
            case LogType.Assert:
                // LogAssert(message.ToString(), context);
                break;
            default:
                break;
        }
    }

    public void Log(LogType logType, string tag, object message)
    {
        switch (logType)
        {
            case LogType.Log:
                Log(tag, message);
                break;
            case LogType.Exception:
                LogException(new Exception(message.ToString()));
                break;
            case LogType.Warning:
                LogWarning(tag, message);
                break;
            case LogType.Error:
                LogError(tag, message);
                break;
            case LogType.Assert:
                // LogAssert(tag, message);
                break;
            default:
                break;
        }
    }

    public void Log(LogType logType, string tag, object message, UnityEngine.Object context)
    {
        switch (logType)
        {
            case LogType.Log:
                Log(tag, message, context);
                break;
            case LogType.Exception:
                LogException(new Exception(message.ToString()), context);
                break;
            case LogType.Warning:
                LogWarning(tag, message, context);
                break;
            case LogType.Error:
                LogError(tag, message, context);
                break;
            case LogType.Assert:
                // LogAssert(tag, message, context);
                break;
            default:
                break;
        }
    }

    public void Log(object message)
    {
        _text.text += $"{message}";
    }

    public void Log(string tag, object message)
    {
        _text.text += $"{tag}: {message}";
    }

    public void Log(string tag, object message, UnityEngine.Object context)
    {
        _text.text += $"{tag}: {message}: {context}";
    }

    public void LogError(string tag, object message)
    {
        _text.text += $"<color ={'"'}red{'"'}>{tag}: {message}</color>";
    }
    public void LogError(object message)
    {
        _text.text += $"<color ={'"'}red{'"'}>{message}</color>";
    }

    public void LogError(string tag, object message, UnityEngine.Object context)
    {
        _text.text += $"<color ={'"'}red{'"'}>{tag}: {message}: {context}</color>";
    }

    public void LogException(Exception exception)
    {
        _text.text += $"<color ={'"'}red{'"'}>{exception}</color>";
    }

    public void LogException(Exception exception, UnityEngine.Object context)
    {
        _text.text += $"<color ={'"'}red{'"'}>{exception}: {context}</color>";
    }

    public void LogFormat(LogType logType, string format, params object[] args)
    {
        throw new NotImplementedException();
    }

    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        throw new NotImplementedException();
    }

    public void LogWarning(string tag, object message)
    {
        _text.text += $"<color ={'"'}yellow{'"'}>{tag}: {message}</color>";
    }
    public void LogWarning(object message)
    {
        _text.text += $"<color ={'"'}yellow{'"'}>{message}</color>";
    }

    public void LogWarning(string tag, object message, UnityEngine.Object context)
    {
        _text.text += $"<color ={'"'}yellow{'"'}>{tag}: {message}</color>";
    }

    public void OnSendMessageToConsole()
    {
        if (_inputField.text == "")
        return;
        _text.text += $"\n_-->{_inputField.text}";
        GameConsole.onCommandSendToConsole?.Invoke(_inputField.text);
        _inputField.text = "";
    }
}
