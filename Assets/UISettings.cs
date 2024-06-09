using UnityEngine;

namespace OutcoreInternetAdventure.UI
{
    /// <summary>
    /// The class Settings of UI
    /// </summary>
    [CreateAssetMenu(menuName = "Outcore SDK/UI Settings")]
    public class UISettings : ScriptableObject
    {
        public Color PanelColor { get { return _panelColor; } }
        public Color ButtonNormalColor { get { return _buttonNormalColor; } }
        public Color ButtonDisabledColor { get { return _buttonDisabledColor; } }
        public Color ButtonSelectedColor { get { return _buttonSelectedColor; } }
        public Color ButtonSubmitedColor { get { return _buttonSubmitedColor; } }
        public float MenuShowFadingDuration { get { return _menuShowFadingDuration; } }
        public float ButtonDuration { get { return _buttonDuration; } }
        public AudioClip ButtonSubmitSound { get { return _buttonSubmitSound; } }
        public AudioClip ButtonSelectSound { get { return _buttonSelectSound; } }

        [SerializeField] Color _buttonNormalColor;
        [SerializeField] Color _buttonDisabledColor;
        [SerializeField] Color _buttonSelectedColor;
        [SerializeField] Color _buttonSubmitedColor;
        [SerializeField] float _buttonDuration;
        [SerializeField] AudioClip _buttonSelectSound;
        [SerializeField] AudioClip _buttonSubmitSound;

        [SerializeField] Color _panelColor;
        [SerializeField] float _menuShowFadingDuration;
    }
}