using UnityEngine;

/// <summary>
/// With this class you can easly control time in game
/// </summary>
public static class TimeService
{
    #region Variables
    [SerializeField] static float _standardTime = 1f;
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
    /// This function sets time in game to your value
    /// </summary>
    /// <param name="value"> Value, which set in Time.timeScale. Default is 0</param>
    public static void ChangeTime(float value = 0f) => Time.timeScale = value;

    /// <summary>
    /// This function ease sets time in game to your value
    /// </summary>
    /// <param name="value"> Value, which set in Time.timeScale. Default is 0</param>
    /// <param name="duration"> Duration of increasing or decreasing time to value. Default is 1</param>
    public static void ChangeTime(float value = 0f, float duration = 1f)
    {
        float _startTime = Time.timeScale;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            Time.timeScale = Mathf.Lerp(_startTime, value, timeElapsed / duration);
            timeElapsed += Time.unscaledDeltaTime;
        }
        Time.timeScale = value;
    }
    #endregion
}