using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using System.Reflection;

#region Dialogue lines
[Serializable]
public class DialogueLine
{
    [SerializeField] float _outputSpeed = 0.1f;
    [SerializeField] bool _canSkipOutput = true;
    [SerializeField] bool _enableVoice = true;
    [SerializeField] DialogueCharacter _dialogueCharacter;
    [SerializeField] string _lineKey;
    public float OutputSpeed { get { return _outputSpeed; } }
    public bool CanSkipOutput { get { return _canSkipOutput; } }
    public bool EnableVoice { get { return _enableVoice; } }
    public DialogueCharacter DialogueCharacter { get { return _dialogueCharacter; } }
    public string LineKey { get { return _lineKey; } }
}

[Serializable]
public class DialogueLineChoice : DialogueLine
{
    [Serializable]
    public class ChoiceLine
    {
        [SerializeField] string _lineKey;
        [SerializeField] Dialogue _onSelectDialogue;

        public string LineKey { get { return _lineKey; } }
        public Dialogue OnSelectDialogue { get { return _onSelectDialogue; } }
    }

    [SerializeField] ChoiceLine[] _selectableLines;
    public ChoiceLine[] SelectableLines { get { return _selectableLines; } }
}
#endregion

[CreateAssetMenu(fileName = "Dialogue", menuName = "Outcore SDK/Dialogue system/Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] List<DialogueLine> _lines;
    public List<DialogueLine> DialogueLines { get { return _lines; } }
    
    [Button]
    void AddLine()
    {
        _lines.Add(new DialogueLine());
    }
}