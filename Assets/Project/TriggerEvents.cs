using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public delegate void OnTriggerEnter2DDelegate(Collider2D collision);
    public OnTriggerEnter2DDelegate OnTriggerEnter2DEvent;

    public delegate void OnTriggerStay2DDelegate(Collider2D collision);
    public OnTriggerStay2DDelegate OnTriggerStay2DEvent;

    public delegate void OnTriggerExit2DDelegate(Collider2D collision);
    public OnTriggerExit2DDelegate OnTriggerExit2DEvent;

    public UnityEvent _onTriggerEnter2DEvent;
    public UnityEvent _onTriggerStay2DEvent;
    public UnityEvent _onTriggerExit2DEvent;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnter2DEvent?.Invoke(collision);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        OnTriggerExit2DEvent?.Invoke(collision);
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerStay2DEvent?.Invoke(collision);
    }
}
