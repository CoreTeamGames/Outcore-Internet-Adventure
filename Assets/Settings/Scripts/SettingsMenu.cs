using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NaughtyAttributes;

namespace OutcoreInternetAdventure.Settings
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] LocalizationService.Language _selectedLanguage;
        [SerializeField] LocalizationService _localizationService;
        [SerializeField] List<LocalizationService.Language> _languageList;

        [SerializeField] Settings _settings;
        [SerializeField] InputActionAsset _inputAsset;
        [SerializeField] InputActionMap _actionMap;
        [SerializeField] InputBinding[] _bindsForIngore;
        [SerializeField] InputBinding _bind;
        [SerializeField] Player.PlayerInputManager _attachedInputManager;
        [SerializeField] InputAction _action;

        [SerializeField] Toggle _3DSoundToggle;
        [SerializeField] Slider _sfxSlider;
        [SerializeField] Slider _musicSlider;
        [SerializeField] Slider _CVVSlider;

        public void Start()
        {
            Initialize();
        }

        [Button]
        public void Initialize()
        {
            Debug.Log("Initialize Settings Menu");
            if (SettingsService.HasSettingsFile())
            {
                LoadSettings();
            }
            _actionMap = _inputAsset.actionMaps[0];
        }

        public void SetupSettings()
        {
            Debug.Log("Setup Settings Menu");
            _3DSoundToggle.isOn = _settings.Enable3DSound;
            _sfxSlider.value = _settings.SfxVolume;
            _musicSlider.value = _settings.MusicVolume;
            _CVVSlider.value = _settings.CharacterVolume;
            foreach (var language in _localizationService.langs)
            {
                if (language.langCode.ToLower() == _settings.LangLocale.ToLower())
                {
                    _localizationService._currentLanguage = language;
                }
            }
        }

        public void LoadSettings()
        {
            Debug.Log("Load Settings Menu");
            _settings = SettingsService.LoadSetiings();
            SetupSettings();
        }

        public void SaveSettings()
        {
            Debug.Log("Save Settings Menu");
            _settings = new Settings(_sfxSlider.value, _CVVSlider.value, _musicSlider.value, _3DSoundToggle.isOn, _localizationService.CurrentLanguage.langCode);
            SettingsService.SaveSettings(_settings);
        }

        public void StartRemapBinding(GameObject _f)
        {

            _bind = new InputBinding();
            _attachedInputManager.onAnykeyEvent += CheckBind;

        }

        void CheckBind(InputBinding binding)
        {
            if (SettingsService.CheckBind(_bindsForIngore, binding) != false)
            {
                _bind = binding;
                EndRemapBinding();
            }
        }

        void EndRemapBinding()
        {
            _attachedInputManager.onAnykeyEvent -= CheckBind;
            ChangeBind(_action);
        }

        public void ChangeBind(InputAction action)
        {
            InputAction _actionInMap = SettingsService.GetAction(action.name, _actionMap);
            _actionInMap.ChangeBinding(_bind);
        }
    }
}
