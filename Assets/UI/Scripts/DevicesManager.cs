using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// A class for management Input control schemes
/// </summary>
[RequireComponent(typeof(DevicesManagerUI))]
public class DevicesManager : MonoBehaviour
{
    #region Variables
    #region Private variables
    [SerializeField] UnityEvent _onDeviceConnected;
    [SerializeField] UnityEvent _onDeviceDisconnected;
    [Dropdown("GetControlSchemes")]
    [SerializeField] string _defaultControlScheme;
    [SerializeField] PlayerInput _input;
    DevicesManagerUI _devicesManagerUI;
    #endregion
    #endregion

    #region Events
    public delegate void OnChangeControlScheme(SpriteAtlas atlas);
    public OnChangeControlScheme OnChangeControlSchemeEvent;
    #endregion

    #region Code

    #region Special Code
    DropdownList<string> GetControlSchemes()
    {
        DropdownList<string> _controlSchemes = new DropdownList<string>();
        if (_input != null)
            foreach (var controlScheme in _input.actions.controlSchemes)
            {
                _controlSchemes.Add(controlScheme.name, controlScheme.name);
            }
        else
            _controlSchemes.Add("None", "");
        return _controlSchemes;
    }
    #endregion

    public void Awake()
    {
        _devicesManagerUI = GetComponent<DevicesManagerUI>();

        InputSystem.onDeviceChange +=
      (device, change) =>
      {
          OnDeviceChange(device, change);
      };
        if (!_input.devices[0].device.name.Contains("Keyboard"))
            foreach (var deviceAndControlScheme in _devicesManagerUI.DevicesAndControlSchemes)
            {
                foreach (var deviceName in deviceAndControlScheme.DeviceNames)
                {
                    if (_input.devices[0].device.displayName.TrimStart(' ').TrimEnd(' ').ToLower().Contains(deviceName.ToLower()))
                    {
                        ChangeControlScheme(deviceAndControlScheme.ControlSchemeName, deviceAndControlScheme.ControlsAtlas);
                    }
                }
            }
    }
    public void OnDestroy()
    {
        InputSystem.onDeviceChange -=
      (device, change) =>
      {
          OnDeviceChange(device, change);
      };
    }
    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                Debug.Log("Device Disconnected: " + device.displayName);
                _onDeviceDisconnected?.Invoke();
                _devicesManagerUI.OnDeviceDisconnected(device);
                ChangeControlScheme(_defaultControlScheme, null);
                break;
            case InputDeviceChange.Reconnected:
                Debug.Log("Device Connected: " + device.displayName);
                _onDeviceConnected?.Invoke();
                _devicesManagerUI.OnDeviceConnected(device);
                break;
        }
    }
    public void ChangeControlScheme(string controlscheme, SpriteAtlas atlas)
    {
        _input.SwitchCurrentControlScheme(controlscheme);
        OnChangeControlSchemeEvent?.Invoke(atlas);
    }
    #endregion
}