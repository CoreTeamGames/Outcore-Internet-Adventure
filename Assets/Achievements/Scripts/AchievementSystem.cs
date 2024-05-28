using UnityEngine;
using System.Collections.Generic;

namespace OutcoreInternetAdventure.Achievements
{
    public class AchievementSystem: MonoBehaviour
    {
        [SerializeField] List<Achievement> _totalAchievements;
        [SerializeField] List<Achievement> _unlockedAchievements;
        [SerializeField] List<Achievement> _lockedAchievements;
        public delegate void OnAchievemenntUnlocked(Achievement achievement);
        public OnAchievemenntUnlocked OnAchievemenntUnlockedEvent;

        public List<Achievement> TotalAchievements { get { return _totalAchievements; } }
        public List<Achievement> UnlockedAchievements { get { return _unlockedAchievements; } }
        public List<Achievement> LockedAchievements { get { return _lockedAchievements; } }
        public List<Achievement> SecretAchievements
        {
            get
            {
                List<Achievement> _secretAchievements = new List<Achievement>();
                foreach (Achievement achievement in _totalAchievements)
                {
                    if (achievement.IsSecret)
                    {
                        _secretAchievements.Add(achievement);
                    }
                }
                return _secretAchievements;
            }
        }

        public void OnEnable()
        {
            _lockedAchievements = _totalAchievements;
        }

        [NaughtyAttributes.Button]
        public void UnlockAchievement(int ID = 0)
        {
            Achievement _achievement = null;
            foreach (Achievement totalAchievement in _totalAchievements)
            {
                if (totalAchievement.ID == ID)
                {
                    _achievement = totalAchievement;
                    break;
                }
            }
            if (_achievement != null)
            {
                foreach (Achievement unlockedAchievement in _unlockedAchievements)
                {
                    if (ID == unlockedAchievement.ID)
                    {
                        Debug.LogError($"Achievement with ID: {ID} already unlocked");
                        return;
                    }
                }
                _lockedAchievements.Remove(_achievement);
                _unlockedAchievements.Add(_achievement);
                OnAchievemenntUnlockedEvent?.Invoke(_achievement);
            }
            else
            {
                Debug.LogError($"Achievement with ID: {ID} not found");
                return;
            }
        }
    }
}