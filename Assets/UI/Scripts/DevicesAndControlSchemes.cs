using UnityEngine.InputSystem;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(menuName = "Outcore SDK/Input/DeviceControlSchemePreset")]
public class DevicesAndControlSchemes : ScriptableObject
{
    #region Variables
    [SerializeField] InputActionAsset _defaultActions;
    [Dropdown("GetControlSchemes")]
    [Tooltip("Select, which control scheme will be used with using this device")]
    [SerializeField] string _controlSchemeName;
    [SerializeField] SpriteAtlas _controlsAtlas;
    [Tooltip("This is name of device.displayName")] [ReorderableList]
    [SerializeField] string[] _deviceNames;
    #endregion

    #region Properties
    /// <summary>
    /// This is name of device.displayName
    /// </summary>
    public string[] DeviceNames { get { return _deviceNames; } }
    /// <summary>
    /// The name of control scheme
    /// </summary>
    public string ControlSchemeName { get { return _controlSchemeName; } }
    /// <summary>
    /// The SpriteAtlas of device
    /// </summary>
    public SpriteAtlas ControlsAtlas { get { return _controlsAtlas; } }
    #endregion

    #region Special code
    DropdownList<string> GetControlSchemes()
    {
        DropdownList<string> _controlSchemes = new DropdownList<string>();
        if (_defaultActions != null)
        foreach (var controlScheme in _defaultActions.controlSchemes)
        {
            _controlSchemes.Add(controlScheme.name, controlScheme.name);
        }
        else
            _controlSchemes.Add("None", "");
        return _controlSchemes;
    }
    #endregion
}