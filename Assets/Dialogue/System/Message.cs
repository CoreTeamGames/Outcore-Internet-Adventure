using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField] string _lineKey;
    [SerializeField] string _leftNameKey;
    [SerializeField] string _rightNameKey;
    [SerializeField] AnimationClip _rightAnimation;
    [SerializeField] AnimationClip _leftAnimation;
    [SerializeField] bool _isLeft;

    public string LineKey { get { return _lineKey; } }
    public string LeftNameKey { get { return _leftNameKey; } }
    public string RightNameKey { get { return _rightNameKey; } }
    public AnimationClip RightAnimation { get { return _rightAnimation; } }
    public AnimationClip LeftAnimation { get { return _leftAnimation; } }
    public bool IsLeft { get { return _isLeft; } }

    public Message(string lineKey, string leftNameKey, string rightNameKey, AnimationClip rightAnimation, AnimationClip leftAnimation, bool isLeft)
    {
        _lineKey = lineKey;
        _leftNameKey = leftNameKey;
        _rightNameKey = rightNameKey;
        _rightAnimation = rightAnimation;
        _leftAnimation = leftAnimation;
        _isLeft = isLeft;
    }
}
