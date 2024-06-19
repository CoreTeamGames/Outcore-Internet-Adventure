using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

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
        [SerializeField] LocalizationService _localizationService;
        [SerializeField] RebindindingSettings _rebindindingSettings;
        [SerializeField] Settings _settings;
        [SerializeField] UnityEvent _onSettingsLoaded;

        [SerializeField] Toggle _3DSoundToggle;
        [SerializeField] Slider _sfxSlider;
        [SerializeField] Slider _musicSlider;
        [SerializeField] Slider _CVVSlider;

        [SerializeField] Slider _brightnessSlider;
        [SerializeField] Toggle _particlesToggle;
        public Settings Settings { get { return _settings; } }
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
            _onSettingsLoaded?.Invoke();
            SetupSettings();
        }

        public void SaveSettings()
        {
            Debug.Log("Save Settings Menu");
            _settings = new Settings(_sfxSlider.value, _CVVSlider.value, _musicSlider.value, _3DSoundToggle.isOn, _localizationService.CurrentLanguage.langCode, _brightnessSlider.value, _rebindindingSettings.Binds);
            SettingsService.SaveSettings(_settings);
        }



        public void ChangeBrightness()
        {
            Screen.brightness = _brightnessSlider.value;
        }
    }
}