using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine.Audio;

namespace OutcoreInternetAdventure.Settings
{
    public class SettingsMenu : MonoBehaviour
    {
        [System.Serializable]
        public class VolumeClass
        {
            public AudioMixerGroup audioMixerGroup;
            public Slider slider;
        }
        [SerializeField] List<VolumeClass> _volumes;

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

        [SerializeField] Slider _brightnessSlider;
        [SerializeField] Toggle _particlesToggle;

        public void Start()
        {
            Initialize();
        }

        public void SetVolumeToAllMixers()
        {
            foreach (var item in _volumes)
            {
                item.audioMixerGroup.audioMixer.SetFloat(item.audioMixerGroup.name + "Volume", item.slider.value);
            }
        }

        public void Initialize()
        {
            Debug.Log("Initialize Settings Menu");
            if (SettingsService.HasSettingsFile())
            {
                LoadSettings();
            }
            SetVolumeToAllMixers();
            _actionMap = _inputAsset.actionMaps[0];
        }

        public void SetupSettings()
        {
            Debug.Log("Setup Settings Menu");
            _3DSoundToggle.isOn = _settings.Enable3DSound;
            _sfxSlider.value = _settings.SfxVolume;
            _musicSlider.value = _settings.MusicVolume;
            _CVVSlider.value = _settings.CharacterVolume;
            _brightnessSlider.value = _settings.Brightness;
            Screen.brightness = _settings.Brightness;
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
            _settings = new Settings(_sfxSlider.value, _CVVSlider.value, _musicSlider.value, _3DSoundToggle.isOn, _localizationService.CurrentLanguage.langCode,_brightnessSlider.value);
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

        public void ChangeBrightness()
        {
            Screen.brightness = _brightnessSlider.value;
        }
    }
}