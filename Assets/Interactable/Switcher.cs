using UnityEngine.Events;
using UnityEngine;

public class Switcher : MonoBehaviour, IInteractor
{
    [SerializeField] bool _isOn;
    [SerializeField] bool _canInteract = true;
    [SerializeField] UnityEvent _onEnable;
    [SerializeField] UnityEvent _onDisable;

    public bool CanInteract => _canInteract;

    public void StartEvent()
    {
        if (_canInteract)
        {
            _isOn = !_isOn;
            if (_isOn == true)
            {
                _onEnable?.Invoke();
            }
            else
            {
                _onDisable?.Invoke();
            }
        }
    }
}