using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using CoreTeamGamesSDK.Localization;

namespace OutcoreInternetAdventure.DialogueSystem
{
    public class DialogueGameWindow : MonoBehaviour
    {
        [SerializeField] UI.UISettings _uISettings;
        [SerializeField] LocalizationService _localizationService;
        [SerializeField] TMP_Text _leftNameText;
        [SerializeField] TMP_Text _rightNameText;
        [SerializeField] Image _leftCharacterAnimator;
        [SerializeField] Image _rightCharacterAnimator;
        [SerializeField] GameObject _leftMessagePrefab;
        [SerializeField] GameObject _rightMessagePrefab;
        [SerializeField] GameObject _messagesBox;
        [SerializeField] GameObject _currentMessage;
        [SerializeField] Sprite _previousMessageSprite;
        [SerializeField] List<GameObject> _previousMessages;
        [SerializeField] AudioSource _voiceSource;
        [SerializeField] int _lineLenght = 14;
        [SerializeField] float _messageOffset = 1;
        [SerializeField] Dialoguereader _reader;
        [SerializeField] bool _canSkipOutput = true;
        Dictionary<string, string> _localizedDialogue;
        bool _isOutput = false;
        bool _dialogueRunning = false;
        string _dialogueName = "Dialogue";
        string _localizedLine;

        public void TryDialogueStep()
        {
            if (_dialogueRunning)
            {
                if (!_isOutput)
                {
                    DialogueStep();
                }
                else if (_canSkipOutput)
                {
                    StopAllCoroutines();
                    _isOutput = false;
                    TMP_Text _text = _currentMessage.GetComponent<TMP_Text>();
                    _text.text = "";
                    int _totalLineLenght = 0;
                    for (int i = 0; i < _localizedLine.Length; i++)
                    {
                        if (_localizedLine[i] == '<')
                        {
                            for (int f = i; f < _localizedLine.Length; f++)
                            {
                                _text.text += _localizedLine[f];
                                if (_localizedLine[f] == '>')
                                {
                                    i = f;
                                    break;
                                }
                            }
                            continue;
                        }
                        else
                        {
                            _totalLineLenght++;
                            if (_totalLineLenght % _lineLenght != 0)
                                _text.text += _localizedLine[i];
                            else
                                _text.text += $"\n{_localizedLine[i]}";
                        }
                    }
                }
            }
        }

        public void StartDialogue(string dialogueName)
        {
            if (!_dialogueRunning)
            {
                _localizedDialogue = _localizationService.GetLocalizedFile(_dialogueName);

                _dialogueRunning = true;
                GetComponent<CanvasGroup>().DOFade(1, _uISettings.MenuShowFadingDuration).OnComplete(() =>
                {
                    _reader.OpenDialogue(dialogueName);
                    _dialogueName = dialogueName;
                    DialogueStep();
                });
            }
        }
        public void EndDialogue()
        {
            if (_dialogueRunning)
            {
                _dialogueRunning = false;
                GetComponent<CanvasGroup>().DOFade(0, _uISettings.MenuShowFadingDuration).OnComplete(() =>
                {
                    Destroy(_currentMessage);
                    _currentMessage = null;
                    foreach (var message in _previousMessages)
                    {
                        Destroy(message);
                    }
                    _previousMessages.Clear();
                    _leftCharacterAnimator.DOFade(0, _uISettings.MenuShowFadingDuration);
                    _rightCharacterAnimator.DOFade(0, _uISettings.MenuShowFadingDuration).OnComplete(() =>
                    {
                        _leftNameText.text = "";
                        _rightNameText.text = "";
                        _leftCharacterAnimator.sprite = null;
                        _rightCharacterAnimator.sprite = null;
                    });
                });
            }
        }
        void DialogueStep()
        {
            Message _message = _reader.ReadLine();
            _localizedLine = _localizedDialogue[_message.lineKey];
            _leftNameText.text = _localizationService.GetLocalizedLine("Names", _message.leftNameKey);
            _rightNameText.text = _localizationService.GetLocalizedLine("Names", _message.rightNameKey);
            _leftCharacterAnimator.sprite = _message.leftAnimation;
            _rightCharacterAnimator.sprite = _message.rightAnimation;
            _leftCharacterAnimator.SetNativeSize();
            _rightCharacterAnimator.SetNativeSize();
            _leftCharacterAnimator.DOFade(1, _uISettings.MenuShowFadingDuration);
            _rightCharacterAnimator.DOFade(1, _uISettings.MenuShowFadingDuration);

            _canSkipOutput = _message.canskip;
            if (_currentMessage != null)
            {
                _currentMessage.GetComponentInChildren<Image>().sprite = _previousMessageSprite;
                _currentMessage.GetComponent<CanvasGroup>().DOFade(0.25f, 0.5f);
                _previousMessages.Add(_currentMessage);
                foreach (var previousMessage in _previousMessages)
                {
                    previousMessage.transform.DOKill(true);
                    previousMessage.transform.DOLocalMoveY(previousMessage.transform.localPosition.y + _currentMessage.GetComponent<RectTransform>().rect.height + _messageOffset, 0.5f);
                }
            }
            _currentMessage = Instantiate(_message.isLeft ? _leftMessagePrefab : _rightMessagePrefab, _messagesBox.transform);
            TMP_Text _text = _currentMessage.GetComponent<TMP_Text>();
            for (int i = 0; i < _localizedLine.Length; i++)
            {
                if (i % _lineLenght != 0)
                    _text.text += _localizedLine[i];
                else
                    _text.text += $"\n{_localizedLine[i]}";
            }
            _currentMessage.transform.DOLocalMoveY(_currentMessage.GetComponent<RectTransform>().rect.height + _text.margin.y, 0.5f);
            _text.text = "";
            StartCoroutine(OutLine(_localizedLine, _message.delay, null, _currentMessage.GetComponent<TMP_Text>()));
        }

        IEnumerator OutLine(string line, float delay, AudioClip voice, TMP_Text text)
        {
            _isOutput = true;
            int _totalLineLenght = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '<')
                {
                    for (int f = i; f < line.Length; f++)
                    {
                        text.text += line[f];
                        if (line[f] == '>')
                        {
                            i = f;
                            break;
                        }
                    }
                    continue;
                }
                else
                {
                    _totalLineLenght++;
                    if (_totalLineLenght % _lineLenght != 0)
                        text.text += line[i];
                    else
                        text.text += $"\n{line[i]}";
                    if (voice != null)
                        _voiceSource.PlayOneShot(voice);
                    yield return new WaitForSeconds(delay);
                }
            }
            _isOutput = false;
        }
    }
}