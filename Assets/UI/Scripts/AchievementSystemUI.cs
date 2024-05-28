using UnityEngine.UI;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using TMPro;
using System.Collections;

namespace OutcoreInternetAdventure.UI
{
    [RequireComponent(typeof(Achievements.AchievementSystem))]
    public class AchievementSystemUI : MonoBehaviour
    {
        public enum Direction
        {
            LeftUp,
            LeftDown,
            RightUp,
            RightDown
        }
        public Direction _direction;

        [SerializeField] string _achievementsFileName = "Achievements.OIALocalize";
        [SerializeField] float _showAndHideDuration;
        [SerializeField] float _showWaitDuration;
        [SerializeField] LocalizationService _localizationService;
        [SerializeField] RectTransform _panel;
        [SerializeField] Image _icon;
        [SerializeField] TMP_Text _titleText;
        [SerializeField] TMP_Text _descriptionText;
        Achievements.AchievementSystem _system;
        CanvasScaler _canvas;

        public delegate void OnAchievementShow();
        public delegate void OnAchievementHide();

        public OnAchievementShow OnAchievementShowEvent;
        public OnAchievementHide OnAchievementHideEvent;

        [Button]
        void PlaceAchievementByDirection()
        {
            switch (_direction)
            {
                case Direction.LeftUp:
                    _panel.pivot = Vector2.zero;
                    _panel.anchoredPosition = Vector2.zero;
                    break;
                case Direction.LeftDown:
                    _panel.pivot = new Vector2(0, 1);
                    _panel.anchoredPosition = new Vector2(0, -_canvas.referenceResolution.y);
                    break;
                case Direction.RightUp:
                    _panel.pivot = new Vector2(1, 0);
                    _panel.anchoredPosition = new Vector2(_canvas.referenceResolution.x, 0);
                    break;
                case Direction.RightDown:
                    _panel.pivot = new Vector2(1, 1);
                    _panel.anchoredPosition = new Vector2(_canvas.referenceResolution.x, -_canvas.referenceResolution.y);
                    break;
            }
        }

        IEnumerator ShowAndHideAchievement()
        {
            ShowAchievement();
            yield return new WaitForSeconds(_showWaitDuration + _showAndHideDuration);
            HideAchievement();
            yield return new WaitForSeconds(_showAndHideDuration);
            _titleText.text = "Title";
            _descriptionText.text = "Description";
            _icon.sprite = null;

        }

        void ShowAchievement()
        {
            if (_direction == Direction.LeftUp || _direction == Direction.RightUp)
            {
                _panel.DOAnchorPosY(-_panel.rect.height, _showAndHideDuration);
            }
            else if (_direction == Direction.LeftDown || _direction == Direction.RightDown)
            {
                _panel.DOAnchorPosY(-_canvas.referenceResolution.y + _panel.rect.height, _showAndHideDuration);
            }
        }

        void HideAchievement()
        {
            if (_direction == Direction.LeftUp || _direction == Direction.RightUp)
            {
                _panel.DOAnchorPosY(0, _showAndHideDuration);
            }
            else if (_direction == Direction.LeftDown || _direction == Direction.RightDown)
            {
                _panel.DOAnchorPosY(-_canvas.referenceResolution.y, _showAndHideDuration);
            }
        }

        [Button]
        void OnEnable()
        {
            _canvas = GetComponentInParent<CanvasScaler>();
            _system = GetComponent<Achievements.AchievementSystem>();
            PlaceAchievementByDirection();
            SubscribeToEvents();
        }
        void SubscribeToEvents()
        {
            _system.OnAchievemenntUnlockedEvent += UnlockAchievement;
        }
        void UnlockAchievement(Achievements.Achievement achievement)
        {
            _titleText.text = _localizationService.GetLocalizedLine(_localizationService.CurrentLanguage.path, _achievementsFileName, achievement.TitleKey);
            _descriptionText.text = _localizationService.GetLocalizedLine(_localizationService.CurrentLanguage.path, _achievementsFileName, achievement.DescriptionKey);
            _icon.sprite = achievement.AchievementIcon;
            if (achievement.IsSecret)
            {

            }
            else
            {

            }
            StartCoroutine("ShowAndHideAchievement");
        }
    }
}