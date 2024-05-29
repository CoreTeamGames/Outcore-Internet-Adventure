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
        public float MenuShowFadingDuration { get { return _menuShowFadingDuration; } }

        [SerializeField] Color _panelColor;
        [SerializeField] float _menuShowFadingDuration;
    }
}