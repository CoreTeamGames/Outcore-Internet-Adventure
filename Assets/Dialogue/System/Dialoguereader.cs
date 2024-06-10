using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dialoguereader : MonoBehaviour
{
    string[] _dialogue;
    string[] _answersKeys;
    [SerializeField] string _lineKey;
    [SerializeField] string _leftNameKey;
    [SerializeField] string _rightNameKey;
    [SerializeField] AnimationClip _rightAnimation;
    [SerializeField] AnimationClip _leftAnimation;
    [SerializeField] AnimationClip[] _animations;
    [SerializeField] bool _isLeft;
    string _fileExtension = "OIADialogue";
    int _stringId = 0;

    public void OpenDialogue(string name)
    {
        _stringId = 0;
        _dialogue = File.ReadAllLines(Application.streamingAssetsPath + "/Dialogues/" + name + '.' + _fileExtension);
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
                                        _leftAnimation = FindAnimation(_dialogue[i].Remove(0, 3));
                                        _isLeft = true;
                                        continue;

                                    //Set left name key
                                    default:
                                        _leftNameKey = _dialogue[i].Remove(0, 2).TrimStart(' ').TrimEnd(' ').ToLower();
                                        _isLeft = true;
                                        continue;
                                }
                                break;
                            case '/':
                                switch (_dialogue[i][2])
                                {
                                    //Set animation to right character
                                    case '#':
                                        _rightAnimation = FindAnimation(_dialogue[i].Remove(0, 3));
                                        _isLeft = false;
                                        continue;

                                    //Set right name key
                                    default:
                                        _rightNameKey = _dialogue[i].Remove(0, 2).TrimStart(' ').TrimEnd(' ').ToLower();
                                        _isLeft = false;
                                        continue;
                                }
                                break;
                            case '|':
                                continue;
                                break;
                            case '*':
                                _stringId = Convert.ToInt32(_dialogue[i].Remove(0, 2)) - 1;
                                Message _message = ReadLine();
                                return _message;

                            default:
                                break;
                        }
                        break;
                    case '/':
                        switch (_dialogue[i][0])
                        {
                            case '/':
                                continue;
                                break;

                            default:
                                _isLeft = false;
                                if (_dialogue[i].Length > 1)
                                {
                                    _lineKey = _dialogue[i].Remove(0);
                                    _stringId = i + 1;
                                    return new Message(_lineKey, _leftNameKey, _rightNameKey, _rightAnimation, _leftAnimation, _isLeft);
                                }
                                else
                                    continue;
                                break;
                        }
                        break;
                    case '\\':
                        switch (_dialogue[i][0])
                        {

                            default:
                                _isLeft = true;
                                _lineKey = _dialogue[i].Remove(0);
                                _stringId = i + 1;
                                return new Message(_lineKey, _leftNameKey, _rightNameKey, _rightAnimation, _leftAnimation, _isLeft);
                        }

                    default:
                        _lineKey = _dialogue[i];
                        _stringId = i + 1;
                        return new Message(_lineKey, _leftNameKey, _rightNameKey, _rightAnimation, _leftAnimation, _isLeft);
                }
            }
        }
        GetComponent<OutcoreInternetAdventure.DialogueSystem.DialogueGameWindow>().EndDialogue();
        return null;
    }

    AnimationClip FindAnimation(string animationKey)
    {
        foreach (var animation in _animations)
        {
            if (animationKey.TrimStart(' ').TrimEnd(' ').ToLower() == animation.name.TrimStart(' ').TrimEnd(' ').ToLower())
            {
                return animation;
            }
        }
        return null;
    }
}
