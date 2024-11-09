using UnityEngine.Events;
using UnityEngine;

public class Switcher : MonoBehaviour,IInteractor
{
    [SerializeField] bool _isOn;
    [SerializeField] UnityEvent _onEnable;
    [SerializeField] UnityEvent _onDisable;
    public void StartEvent()
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