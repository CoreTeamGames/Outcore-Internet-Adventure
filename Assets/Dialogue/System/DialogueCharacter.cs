using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue character",menuName = "Outcore SDK/Dialogue system/Dialogue character")]
public class DialogueCharacter : ScriptableObject
{
    [SerializeField] string _nameId;
    [SerializeField] AudioClip _voice;
    [SerializeField] CharacterEmotion[] _characterEmotions;

    /// <summary>
    /// The ID of character name for get localized name
    /// </summary>
    public string NameID { get { return _nameId; } }
    /// <summary>
    /// The voice of character
    /// </summary>
    public AudioClip Voice { get { return _voice; } }
    /// <summary>
    /// Array of character emotions with ID
    /// </summary>
    public CharacterEmotion[] characterEmotions { get { return _characterEmotions; } }
}

[System.Serializable]
public class CharacterEmotion
{
    [SerializeField] AnimationClip _emotionClip;
    [SerializeField] string _emotionId;

    /// <summary>
    /// The animation of character emotion
    /// </summary>
    public AnimationClip EmotionSprite { get { return _emotionClip; } }
    public string EmotionID { get { return _emotionId; } }
}