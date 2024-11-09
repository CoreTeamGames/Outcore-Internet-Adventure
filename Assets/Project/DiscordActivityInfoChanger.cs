using UnityEngine;
using UnityEngine.Events;

public class DiscordActivityInfoChanger : MonoBehaviour
{
    #region Enumerators
    public enum typeOfPlay
    {
        Storymode = 0,
        Bossfight = 1,
        Desktop_Story = 3,
        Desktop_Free = 4,
        Surfing_In_Web = 5,
    }
    #endregion

    #region Classes
    [System.Serializable]
    public class DiscordActivity
    {
        [SerializeField] typeOfPlay _typeOfPlay = typeOfPlay.Storymode;
        [SerializeField] string _descriptionOfPhase;
        [Tooltip("Phase of game, like solving puzzle and etc.")]
        [SerializeField] string _currentPhase;
        [SerializeField] string _sceneNameInActivity;
        public typeOfPlay TypeOfPlay { get { return _typeOfPlay; } }
        public string DescriptionOfPhase { get { return _descriptionOfPhase; } }
        public string CurrentPhase { get { return _currentPhase; } }
        public string SceneNameInActivity { get { return _sceneNameInActivity; } }
    }
    #endregion

    #region Variables
    [SerializeField] DiscordActivity _activity;
    #endregion

    #region Events
    [SerializeField] UnityEvent _onUpdateActivity;
    public delegate void ChangeDiscordActivity(DiscordActivity activity);
    public ChangeDiscordActivity OnChangeDiscordActivity;
    #endregion

    #region Getters
    public DiscordActivity Activity { get { return _activity; } }
    #endregion

    #region Code
    public void UpdateActivityInDiscord()
    { UpdateActivityInDiscord(_activity); }    
    public void UpdateActivityInDiscord(DiscordActivity activity)
    {
        OnChangeDiscordActivity?.Invoke(activity);
        if (OnChangeDiscordActivity.Target != null)
        { 
            Debug.Log("Changing activity in Discord");
            _onUpdateActivity?.Invoke();
        }
        else
            Debug.Log("Failed change activity in Discord");
    }
    #endregion
}