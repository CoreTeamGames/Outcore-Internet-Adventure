using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dialoguereader : MonoBehaviour
{
    string[] _dialogue;
    string[] _answersKeys;
    [SerializeField] float _standardDelay = 0.01f;
    [SerializeField] bool _standardCanSkipOutput = true;
    [SerializeField] Message _message;
    [SerializeField] Sprite[] _sprites;
    string _fileExtension = "OIADialogue";
    int _stringId = 0;

    public void OpenDialogue(string name)
    {
        _message = new Message("","","", null, null, false,_standardCanSkipOutput,_standardDelay);
        _stringId = 0;
        _dialogue = ClearNullArrayElements(File.ReadAllLines(Application.streamingAssetsPath + "/Dialogues/" + name + '.' + _fileExtension));
        
    }

    public Message ReadLine()
    {
        if (_stringId < _dialogue.Length)
        {
            for (int i = _stringId; i < _dialogue.Length; i++)
            {
                switch (_dialogue[i][0])
                {
                    case '[':
                        switch (_dialogue[i][1])
                        {
                            case '\\':
                                switch (_dialogue[i][2])
                                {
                                    //Set animation to left character
                                    case '#':
                                        _message.leftAnimation = GetSprite(_dialogue[i].Split('#')[1].ToLower());
                                        _message.isLeft = true;
                                        continue;

                                    //Set left name key
                                    default:
                                        _message.leftNameKey = _dialogue[i].Split('\\')[1].TrimStart(' ').TrimEnd(' ').ToLower();
                                        _message.isLeft = true;
                                        continue;
                                }
                            case '/':
                                switch (_dialogue[i][2])
                                {
                                    //Set animation to right character
                                    case '#':
                                        _message.rightAnimation = GetSprite(_dialogue[i].Split('#')[1].ToLower());
                                        _message.isLeft = false;
                                        continue;

                                    //Set right name key
                                    default:
                                        _message.rightNameKey = _dialogue[i].Split('/')[1].TrimStart(' ').TrimEnd(' ').ToLower();
                                        _message.isLeft = false;
                                        continue;
                                }
                            case '|':
                                continue;
                            case '*':
                                _stringId = Convert.ToInt32(_dialogue[i].Split('*')[1]) - 1;
                                _message = ReadLine();
                                return _message;

                            default:
                                string[] _tagAndValue = _dialogue[i].Trim('[', ']').Split('=');
                                if (_tagAndValue.Length > 0)
                                {
                                    switch (_tagAndValue[0])
                                    {
                                        case "delay":
                                            _message.delay = Convert.ToSingle(_tagAndValue[1]);
                                            continue;

                                        case "canSkip":
                                            _message.canskip = Convert.ToBoolean(_tagAndValue[1]);
                                            continue;
                                        default:
                                            Debug.LogError("Syntax doesn't have tag: " + _dialogue[i]);
                                            continue;
                                    }
                                }
                                else
                                {
                                    Debug.LogError("Tag doesn't have value! tag is: " + _dialogue[i]);
                                    continue;
                                }
                        }

                    case '/':
                        switch (_dialogue[i][1])
                        {
                            case '/':
                                continue;

                            default:
                                _message.isLeft = false;
                                _stringId++;
                                if (_dialogue[i].Length > 0)
                                {
                                    _message.lineKey = _dialogue[i].Remove(0, 1);
                                    return _message;
                                }
                                else
                                    continue;
                        }

                    case '\\':
                        _message.isLeft = true;
                        _stringId++;
                        if (_dialogue[i].Length > 0)
                        {
                            _message.lineKey = _dialogue[i].Remove(0, 1);
                            return _message;
                        }
                        else
                            continue;

                    default:
                        _stringId = i + 1;
                        _message.lineKey = _dialogue[i];
                        return _message;
                }
            }
        }
        GetComponent<OutcoreInternetAdventure.DialogueSystem.DialogueGameWindow>().EndDialogue();
        return null;
    }

    Sprite GetSprite(string name)
    {
        name = name.ToLower();
        foreach (var sprite in _sprites)
        {
            if (sprite.name.ToLower() == name)
            {
                return sprite;
            }
        }
        return null;
    }

    string[] ClearNullArrayElements(string[] array)
    {
        List<string> _array = new List<string>();
        foreach (var item in array)
        {
            if (item != "")
            {
                _array.Add(item);
            }
        }
        return _array.ToArray();
    }
}