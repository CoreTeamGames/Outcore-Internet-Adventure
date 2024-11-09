using UnityEngine;

namespace OutcoreInternetAdventure.Achievements
{
    /// <summary>
    /// The class of achievement
    /// </summary>
    [CreateAssetMenu(menuName = "Outcore SDK/Achievement")]
    public class Achievement: ScriptableObject
    {
        #region Variables
        [SerializeField] bool _isSecret = false;
        [SerializeField] int _id = 0;
        [SerializeField] string _titleKey = "";
        [SerializeField] string _descriptionKey = "";
        [SerializeField] Sprite _achievementIcon;
        #endregion

        #region Properties
        /// <summary>
        /// If this bool is true, achievement be secret
        /// </summary>
        public bool IsSecret { get { return _isSecret; } }
        /// <summary>
        /// The identificator of achievement
        /// </summary>
        public int ID { get { return _id; } }
        /// <summary>
        /// The localization key for the title of achievement
        /// </summary>
        public string TitleKey { get { return _titleKey; } }
        /// <summary>
        /// The localization key for the description of achievement
        /// </summary>
        public string DescriptionKey { get { return _descriptionKey; } }
        /// <summary>
        /// The icon of achievement
        /// </summary>
        public Sprite AchievementIcon { get { return _achievementIcon; } }
        #endregion
    }
}