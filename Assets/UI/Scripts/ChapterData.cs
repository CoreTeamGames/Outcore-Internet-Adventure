using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Outcore SDK/Chapter/Data")]
public class ChapterData : ScriptableObject
{
    [Min(0)]
    [SerializeField] int _chapterNumber;
    [SerializeField] string _chapterNameKey;

    public int ChapterNumber { get { return _chapterNumber; } }
    public string ChapterNameKey { get { return _chapterNameKey; } }
}