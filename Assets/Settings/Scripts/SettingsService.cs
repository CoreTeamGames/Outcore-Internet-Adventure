using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.IO;
using System;

namespace OutcoreInternetAdventure.Settings
{
    public static class SettingsService
    {
        [SerializeField] static string _saveFileName = "SaveMe";
        [SerializeField] static string _controlsFileName = "ControlMe";
        [SerializeField] static string _extension = "OIASettings";

        public static bool HasSettingsFile()
        {
            if (File.Exists($"{Application.persistentDataPath}/{_saveFileName}.{_extension}"))
                return true;
            else
                return false;
        }
        public static void SaveSettings(Settings settings)
        {
            Debug.Log("Save Settings");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Character Volume", settings.CharacterVolume.ToString(), "Audio");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "SFX Volume", settings.SfxVolume.ToString(), "Audio");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Music Volume", settings.MusicVolume.ToString(), "Audio");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "3D sound", settings.Enable3DSound.ToString(), "Audio");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Current Language", settings.LangLocale.ToString(), "Localization");
            Parsers.Ini.IniWriter.Write($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Brightness", settings.Brightness.ToString(), "Video");
            using (var _fileStream = File.Open($"{Application.persistentDataPath}/{_controlsFileName}.{_extension}", FileMode.Create,FileAccess.Write))
            {
                using (StreamWriter _streamWriter = new StreamWriter(_fileStream))
                {
                    foreach (var bind in settings.Binds)
                    {
                        _streamWriter.WriteLine(bind);
                    }
                }
            }
            Debug.Log($"{Application.persistentDataPath}/{_saveFileName}.{_extension}");
        }
        public static Settings LoadSetiings()
        {
            Debug.Log("Load Settings");
            string _sfxVolume = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "SFX Volume", "Audio");
            string _musicVolume = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Music Volume", "Audio");
            string _cVVolume = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Character Volume", "Audio");
            string _3dSound = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "3D sound", "Audio");
            string _langLocale = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Current Language", "Localization");
            string _brightness = Parsers.Ini.IniReader.Read($"{Application.persistentDataPath}/{_saveFileName}.{_extension}", "Brightness", "Video");
            string[] _binds;
            string _bindsNonArray = "";
            using (var _fileStream = File.Open($"{Application.persistentDataPath}/{_controlsFileName}.{_extension}", FileMode.OpenOrCreate))
            {
                using (StreamReader _streamReader = new StreamReader(_fileStream))
                {
                    _bindsNonArray = _streamReader.ReadToEnd();
                }
            }
            _binds = _bindsNonArray.Split('\n');
            Settings _settings = new Settings(Convert.ToSingle(_sfxVolume), Convert.ToSingle(_cVVolume), Convert.ToSingle(_musicVolume), Convert.ToBoolean(_3dSound), _langLocale, Convert.ToSingle(_brightness), _binds);
            return _settings;
        }

        public static InputAction GetAction(int index, InputActionMap map)
        {
            return map.actions[index];
        }
        public static InputAction GetAction(string name, InputActionMap map)
        {
            InputAction _action = null;
            name = name.ToLower();
            foreach (var action in map.actions)
            {
                if (name == action.name.ToLower())
                {
                    _action = action;
                    break;
                }
            }
            return _action != null ? _action : null;
        }

        public static InputBinding GetBinding(string name, InputAction action)
        {
            name = name.ToLower();
            InputBinding _binding = new InputBinding();
            foreach (var binding in action.bindings)
            {
                if (binding.name == name)
                {
                    _binding = binding;
                    break;
                }
            }
            return _binding != new InputBinding() ? _binding : throw new System.OperationCanceledException("Couldn't found binding!");
        }

        public static void ChangeBind(InputBinding binding, InputActionMap map)
        {

            InputBinding _bind = new InputBinding();
            foreach (var action in map.actions)
            {
                foreach (var _binding in action.bindings)
                {
                    if (_binding.name == binding.name)
                    {
                        _bind = _binding;
                        break;
                    }
                }
            }

            _bind.path = _bind != new InputBinding() ? binding.path : throw new System.OperationCanceledException("Couldn't found binding!");
        }

        public static InputBinding ReadBind(InputBinding[] keysForIgnore, InputBinding key, InputBinding binding)
        {
            if (CheckBind(keysForIgnore, key) != false)
            {
                binding.path = key.ToString();
                return binding;
            }
            else
            {
                throw new System.OperationCanceledException();
            }
        }

        public static bool CheckBind(InputBinding[] keysForIgnore, InputBinding key)
        {
            foreach (var _key in keysForIgnore)
            {
                if (key == _key)
                {
                    return false;
                }
            }
            return true;
        }
    }
}