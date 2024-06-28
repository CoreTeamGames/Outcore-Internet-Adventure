using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

/// <summary>
/// A class for UI window of Device manager
/// </summary>
[RequireComponent(typeof(DevicesManager))]
public class DevicesManagerUI : OutcoreInternetAdventure.UI.UIWindow
{
    [SerializeField] DevicesAndControlSchemes[] _devicesAndControlSchemes;
    [SerializeField] LocalizeText _descriptionLocalizator;
    [SerializeField] string _onDeviceDisconnectedLocalizeKey = "OnDeviceDisconnected";
    [SerializeField] string _onDeviceConnectedLocalizeKey = "OnDeviceConnected";
    DevicesManager _devicesManager;
    InputDevice _device;

    public DevicesAndControlSchemes[] DevicesAndControlSchemes { get => _devicesAndControlSchemes; }

    public void Awake()
    {
        _devicesManager = GetComponent<DevicesManager>();
    }

    public void OnDeviceConnected(InputDevice device)
    {
        _device = device;
        _descriptionLocalizator.variables = new List<string>();
        _descriptionLocalizator.variables.Add(device.displayName.TrimStart(' ').TrimEnd(' '));
        _descriptionLocalizator.lineId = _onDeviceConnectedLocalizeKey;
        _descriptionLocalizator.Localize();
        ShowWindow();
    }

    public void OnDeviceDisconnected(InputDevice device)
    {
        _device = device;
        _descriptionLocalizator.variables = new List<string>();
        _descriptionLocalizator.variables.Add(device.displayName.TrimStart(' ').TrimEnd(' '));
        _descriptionLocalizator.lineId = _onDeviceDisconnectedLocalizeKey;
        _descriptionLocalizator.Localize();
        ShowWindow();
    }
    
    public void ChangeControlScheme()
    {
        HideWindow();
        foreach (var deviceAndControlScheme in _devicesAndControlSchemes)
        {
            foreach (var deviceName in deviceAndControlScheme.DeviceNames)
            {
                if (_device.displayName.TrimStart(' ').TrimEnd(' ').ToLower().Contains(deviceName.ToLower()))
                {
                    _devicesManager.ChangeControlScheme(deviceAndControlScheme.ControlSchemeName,deviceAndControlScheme.ControlsAtlas);
                }
            }
        }
    }

}