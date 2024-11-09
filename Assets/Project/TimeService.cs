using UnityEngine;

/// <summary>
/// With this class you can easly control time in game
/// </summary>
public static class TimeService
{
    #region Variables
    [SerializeField] static float _standardTime = 1f;
    [SerializeField] static bool _isPaused = false;
    #endregion

    #region Variables
    public static bool IsPaused => _isPaused;
    #endregion

    #region Code
    /// <summary>
    /// Standard time in game
    /// </summary>
    public static float StandardTime { get { return _standardTime; } }

    /// <summary>
    /// This function set time in game as standard
    /// </summary>
    public static void SetTimeAsStandard() => Time.timeScale = _standardTime;

    /// <summary>
    /// This function pauses game
    /// </summary>
    public static void PauseGame(bool isPaused = true)
    {
        Time.timeScale = isPaused ? 0 : _standardTime;
        _isPaused = isPaused;
    }
    #endregion
}