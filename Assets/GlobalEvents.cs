using UnityEngine.Events;
using UnityEngine;

public class GlobalEvents : MonoBehaviour
{
    #region Events
    public UnityEvent onAwake;
    public UnityEvent onStart;
    public UnityEvent onLevelLoad;
    public UnityEvent onGameQuit;
    public UnityEvent onGameFocus;
    public UnityEvent onGameUnfocus;
    #endregion
    #region Code
    public void Awake() => onAwake?.Invoke();
    public void Start() => onStart?.Invoke();
    public void OnLevelWasLoaded(int level) => onLevelLoad?.Invoke();
    public void OnApplicationQuit() => onGameQuit?.Invoke();
    public void OnApplicationFocus(bool focus)
    {
        if (focus)
            onGameFocus?.Invoke();
        else
            onGameUnfocus?.Invoke();
    }
    #endregion
}
