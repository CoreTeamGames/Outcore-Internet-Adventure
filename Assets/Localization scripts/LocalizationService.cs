using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using CsvHelper;
using System.Linq;
using System.Globalization;

public class LocalizationService : MonoBehaviour
{
    [Serializable]
    public class Language
    {
        public Language(string langCode, string langName, bool isRTL)
        {
            this.langCode = langCode;
            this.langName = langName;
            this.isRTL = isRTL;

        }

        public string langCode;

        public string langName;

        public bool isRTL;
        public string path;

    }

    [Serializable]
    public class CharacterName
    {
        public string langCode { get; set; }
        public string name { get; set; }
    }

    [Serializable]
    public class DialogueText
    {
        public DialogueText(string lineId, string nameKey, string text, bool isLeft)
        {
            this.lineId = lineId;
            this.nameKey = nameKey;
            this.text = text;
            this.isLeft = isLeft;
        }


        public string lineId { get; set; } = "";
        public string nameKey { get; set; } = "";
        public string text { get; set; } = "";
        public bool isLeft { get; set; } = false;

    }

    public List<Language> langs;
    public Language CurrentLanguage { get { return _currentLanguage; } }
    public Language _currentLanguage;

    string _pathToLocalizationFolder;
    string[] _Directories;
    [SerializeField] string _fileExtension = "OIALocalize";
    public string FileExtension { get { return _fileExtension; } }

    public delegate void onlanguageSelected();
    public onlanguageSelected onlanguageSelectedEvent;

    public void Awake() => AddLangs();

    public void SelectLanguage(Language language)
    {
        _currentLanguage = language;
        onlanguageSelectedEvent?.Invoke();
    }

    [Button("Search languages in folder")]
    public void AddLangs()
    {
        _currentLanguage = null;
        langs = new List<Language>();
        _pathToLocalizationFolder = $"{Application.dataPath}\\Localization";
        _Directories = AddAllDirectories(_pathToLocalizationFolder);
        langs = SearchAllLanguages(_Directories);
        
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
        return GetLocalizedLine(CurrentLanguage.path, fileName, lineID);
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
                        if (lineID == record.lineId.ToLower())
                        {
                            _localizedLine = record.text.ToString().TrimStart(' ').TrimEnd(' ');
                            break;
                        }
                    }
                    return _localizedLine;
                }
            }
        }
        else
        {
            Debug.LogError("Can't find file!");
            return null;
        }
    }

    public DialogueText GetLocalizedDialogueLineById(string path, string fileName, string lineID)
    {
        if (File.Exists($"{path}/{fileName}.{FileExtension.Trim('.')}"))
        {
            path = path.ToLower();
            DialogueText _dialogueText = null;
            using (var _fileStream = File.OpenText($"{path}/{fileName}.{FileExtension.Trim('.')}"))
            {
                var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.CurrentCulture)
                {
                    Delimiter = "/|\\"
                };
                using (CsvReader _csvReader = new CsvReader(_fileStream, csvConfig))
                {
                    _csvReader.Read();
                    _csvReader.ReadHeader();
                    List<DialogueText> _lines = _csvReader.GetRecords<DialogueText>().ToList();
                    for (int i = 0; i < _lines.Count; i++)
                    {
                        if (_lines[i].lineId.ToLower() == path)
                            _dialogueText = _lines[i]; break;
                    }
                    return _dialogueText;


                }
            }
        }
        else
        {
            Debug.LogError("Can't find file!");
            return null;
        }
    }

    public Language SearchlanguagebyID(string _languageId)
    {
        Language _lang = null;
        _languageId = _languageId.ToLower();
        for (int i = 0; i < langs.Count; i++)
        {
            if (langs[i].langCode.ToLower() == _languageId)
            {
                _lang = langs[i];
                break;

            }
        }
        if (_lang == null)
            Debug.LogError("Can't find language");

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
                        Language _lang = new Language(TrimBySpace(_temp.langCode), TrimBySpace(_temp.langName), _temp.isRTL);
                        _lang.path = _path[i];
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

