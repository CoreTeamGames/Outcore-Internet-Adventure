using System;
using System.Collections.Generic;
using UnityEngine;
using FileWorker;

public class Localization : MonoBehaviour
{
    [Serializable]
    public class languages
    {
        public string langCode = "en";
        public bool isRTL = false;
        public bool leftToRight = true;

    }
    public List<languages> langs;
    public List<LocalizeText> localizeTextAssets;
    public string localizePath;
    public string levelName;
    public string characterLocalizeFileName;
    public int curLang = 0;

    public int dialogueLinesCount = 5;
    public int CharacterNameLinesCount = 5;
    public List<string> dialogueLines;
    public List<string> characterNames;
    //C:\Users\fnaf9\Desktop\M1L1_en.OIALocalize

    public void Awake()
    {
        ReadWriteSeekLine readAllLines = new ReadWriteSeekLine();
        readAllLines.ReadAllLines($@"{localizePath}\{characterLocalizeFileName}_{langs[curLang].langCode}.OIALocalize", characterNames, CharacterNameLinesCount);
        readAllLines.ReadAllLines($@"{localizePath}\{levelName}_{langs[curLang].langCode}.OIALocalize", dialogueLines, dialogueLinesCount);
    }

    public string LocalizeLine(languages _languages, int stringID)
    {

        ReadWriteSeekLine readWrite = new ReadWriteSeekLine();
        string line;
        line = readWrite.SeekAndReadLine($@"{localizePath}\{levelName}_{_languages.langCode}.OIALocalize", stringID);
        return line;
    }
}
