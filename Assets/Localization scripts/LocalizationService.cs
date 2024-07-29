using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;
using System.IO;
using CsvHelper;
using System;

public class LocalizationService : MonoBehaviour
{
    #region Classes
    [Serializable]
    public class Language
    {
        [SerializeField] string _langCode;
        [SerializeField] string _langName;
        [SerializeField] bool _isRTL;
        [SerializeField] string _path;

        public string LangCode { get => _langCode; }
        public string LangName { get => _langName; }
        public bool IsRTL { get => _isRTL; }
        public string Path { get => _path; }

        public Language(string langCode, string langName, bool isRTL, string path)
        {
            _langCode = langCode;
            _langName = langName;
            _isRTL = isRTL;
            _path = path;
        }
    }

    [Serializable]
    public class CharacterName
    {
        public string LangCode { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class DialogueText
    {
        [SerializeField] string _lineKey = "";
        [SerializeField] string _text = "";

        public string LineKey { get => _lineKey; }
        public string Text { get => _text; }

        public DialogueText(string lineKey, string text)
        {
            _lineKey = lineKey;
            _text = text;
        }
    }
    #endregion

    #region Variables
    [SerializeField] List<Language> _langs;
    [SerializeField] string _fileExtension = "OIALocalize";
    [SerializeField] Language _currentLanguage;
    string _pathToLocalizationFolder;
    string[] _directories;
    #endregion

    #region Properties
    public List<Language> Langs { get => _langs; }
    public string FileExtension { get => _fileExtension; }
    public Language CurrentLanguage { get => _currentLanguage;  }
    #endregion

    public delegate void onlanguageSelected();
    public onlanguageSelected onlanguageSelectedEvent;

    public void Awake() => AddLangs();

    public void ChangeLanguage(Language language)
    {
        _currentLanguage = language;
        onlanguageSelectedEvent?.Invoke();
    }

    [Button("Search languages in folder")]
    public void AddLangs()
    {
        _currentLanguage = null;
        _langs = new List<Language>();
        _pathToLocalizationFolder = $"{Application.streamingAssetsPath}\\Localization";
        _directories = AddAllDirectories(_pathToLocalizationFolder);
        _langs = SearchAllLanguages(_directories);

        if (_currentLanguage == null)
        {
            if (SearchlanguagebyID(CultureInfo.CurrentCulture.ToString().ToLower()) != null)
                _currentLanguage = SearchlanguagebyID(CultureInfo.CurrentCulture.ToString().ToLower());
            else
                _currentLanguage = SearchlanguagebyID("en-us");
        }
    }
    public void Start()
    {
        if (_currentLanguage != null)
            onlanguageSelectedEvent?.Invoke();
    }
    public string GetLocalizedLine(string fileName, string lineID)
    {
        return GetLocalizedLine(CurrentLanguage.Path, fileName, lineID);
    }

    public string GetLocalizedLine(string path, string fileName, string lineID)
    {
        if (File.Exists($"{path}/{fileName}.{FileExtension.Trim('.')}"))
        {
            using (var _fileStream = File.OpenText($"{path}/{fileName}.{FileExtension.Trim('.')}"))
            {
                var _config = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "/|\\",
                    BadDataFound = null
                };
                using (CsvReader _csvReader = new CsvReader(_fileStream, _config))
                {
                    lineID = lineID.ToLower();
                    _csvReader.Read();

                    _csvReader.ReadHeader();
                    string _localizedLine = null;
                    var records = _csvReader.GetRecords<DialogueText>();
                    foreach (var record in records)
                    {
                        if (lineID == record.LineKey.ToLower())
                        {
                            _localizedLine = record.Text.ToString().TrimStart(' ').TrimEnd(' ');
                            break;
                        }
                    }
                    return _localizedLine;
                }
            }
        }
        else
        {
            Debug.LogError("Can't find file! file name is: " + fileName);
            return null;
        }
    }

    public bool LineExist(string fileName, string lineID)
    {
        if (GetLocalizedLine(CurrentLanguage.Path, fileName, lineID) != "")
            return true;
        else
            return false;
    }

    public Language SearchlanguagebyID(string _languageId)
    {
        Language _lang = null;
        _languageId = _languageId.ToLower();
        for (int i = 0; i < _langs.Count; i++)
        {
            if (_langs[i].LangCode.ToLower() == _languageId)
            {
                _lang = _langs[i];
                break;

            }
        }
        if (_lang == null)
            Debug.LogError("Can't find language! Language id is: " + _languageId);

        return _lang;
    }

    public string[] AddAllDirectories(string _path)
    { return Directory.GetDirectories(_path); }

    public List<Language> SearchAllLanguages(string[] _path)
    {
        List<Language> _languages = new List<Language>();
        for (int i = 0; i < _path.Length; i++)
        {
            if (File.Exists(_path[i] + $"/Language.{_fileExtension}"))
            {
                using (var _fileStream = File.OpenText(_path[i] + $"/Language.{_fileExtension}"))
                {
                    var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = @"/|\"
                    };
                    using (CsvReader _csvReader = new CsvReader(_fileStream, csvConfig))
                    {
                        _csvReader.Read();
                        _csvReader.GetRecord<Language>();
                        _csvReader.ReadHeader();
                        var _temp = _csvReader.GetRecord<Language>();
                        Language _lang = new Language(TrimBySpace(_temp.LangCode), TrimBySpace(_temp.LangName), _temp.IsRTL, _path[i]);
                        _languages.Add(_lang);
                    }
                }
            }
            else
            {
                Debug.LogWarning($"Folder {_path[i]} doesn't contain any language or file {'"'}Language.{_fileExtension}{'"'} doesn't found or exist");
            }
        }
        return _languages;
    }

    string TrimBySpace(string _inputString) { return _inputString.TrimStart(' ').TrimEnd(' '); }

}