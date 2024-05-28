using UnityEngine;
using UnityEngine.InputSystem;

namespace OutcoreInternetAdventure.Settings
{
    /// <summary>
    /// This class contain data of settings
    /// </summary>
    [System.Serializable]
    public class Settings
    {
        #region Variables
        [SerializeField] string _langLocale;
        [SerializeField] float _sfxVolume = 1f;
        [SerializeField] float _characterVolume = 1f;
        [SerializeField] float _musicVolume = 1f;
        [SerializeField] bool _enable3DSound = true;

        [SerializeField] bool _enableBloom;
        [SerializeField] bool _enableParticles;
        [SerializeField] InputActionMap _actionMap;

        public Settings(float sfxVolume, float characterVolume, float musicVolume, bool enable3DSound,string langLocale)
        {
            _sfxVolume = sfxVolume;
            _characterVolume = characterVolume;
            _musicVolume = musicVolume;
            _enable3DSound = enable3DSound;
            _langLocale = langLocale;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The locale of selected language in game
        /// </summary>
        public string LangLocale { get { return _langLocale; } }
        /// <summary>
        /// The volume of sound effects
        /// </summary>
        public float SfxVolume { get { return _sfxVolume; } }
        /// <summary>
        /// The volume of music
        /// </summary>
        public float MusicVolume { get { return _musicVolume; } }
        /// <summary>
        /// The volume of Characters sound
        /// </summary>
        public float CharacterVolume { get { return _characterVolume; } }
        /// <summary>
        /// The Bloom
        /// </summary>
        public bool EnableBloom { get { return _enableBloom; } }
        /// <summary>
        /// 3D sound in game
        /// </summary>
        public bool Enable3DSound { get { return _enable3DSound; } }
        /// <summary>
        /// Particles in game
        /// </summary>
        public bool EnableParticles { get { return _enableParticles; } }
        /// <summary>
        /// The Input action map asset
        /// </summary>
        public InputActionMap ActionMap { get { return _actionMap; } }
        #endregion
    }
}