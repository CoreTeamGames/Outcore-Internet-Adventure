using UnityEngine;

public class PlayerTrigger2D : TriggerEvents
{
    Collider2D _playerCollider;

    public void OnEnable()
    {
        OnTriggerEnter2DEvent += OnPlayerEnterTrigger;
        OnTriggerStay2DEvent += OnPlayerStayTrigger;
        OnTriggerExit2DEvent += OnPlayerExitTrigger;
    }
    void OnPlayerEnterTrigger(Collider2D collision)
    {
        _playerCollider = collision;
        _onTriggerEnter2DEvent?.Invoke();
    }
    void OnPlayerStayTrigger(Collider2D collision)
    {
        _onTriggerStay2DEvent?.Invoke();
    }
    void OnPlayerExitTrigger(Collider2D collision)
    {
        _onTriggerExit2DEvent?.Invoke();
    }
}