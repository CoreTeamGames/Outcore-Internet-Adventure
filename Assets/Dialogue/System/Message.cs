using UnityEngine;

public class Message
{
    public string lineKey;
    public string leftNameKey;
    public string rightNameKey;
    public Sprite rightAnimation;
    public Sprite leftAnimation;
    public bool isLeft;
    public bool canskip;
    public float delay;

    public Message(string lineKey, string leftNameKey, string rightNameKey, Sprite rightAnimation, Sprite leftAnimation, bool isLeft, bool canskip, float delay)
    {
        this.lineKey = lineKey;
        this.leftNameKey = leftNameKey;
        this.rightNameKey = rightNameKey;
        this.rightAnimation = rightAnimation;
        this.leftAnimation = leftAnimation;
        this.isLeft = isLeft;
        this.canskip = canskip;
        this.delay = delay;
    }
    public Message()
    { }
}