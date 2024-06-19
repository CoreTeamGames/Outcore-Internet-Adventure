using OutcoreInternetAdventure.Settings;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine;

public class RebindindingSettings : MonoBehaviour
{
    #region Variables
    public UnityEvent onRebindStart;
    public UnityEvent onRebindCancel;
    public UnityEvent onRebindComplete;
    [SerializeField] SettingsMenu _menu;
    [SerializeField] PlayerInput _input;
    [SerializeField] string[] _binds;
    [SerializeField] EventSystem _system;
    InputActionRebindingExtensions.RebindingOperation _operation;
    InputAction _action;
    GameObject _beforeSelectedObject;
    #endregion

    public string[] Binds { get { return _binds; } }
    public PlayerInput Input { get { return _input; } }

    #region RebindingMethods
    [NaughtyAttributes.Button]
    public void ClearOverrides()
    {
        foreach (var map in _input.actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                action.RemoveAllBindingOverrides();
            }
        }
    }
    public void ChangeBind(InputActionReference action, int bindingId = 0)
    {
        _action = action;
        _action.Disable();
        onRebindStart?.Invoke();
        _operation = new InputActionRebindingExtensions.RebindingOperation();
        _operation.WithTimeout(10);
        _operation.OnMatchWaitForAnother(.1f);
        _operation.WithAction(_action).WithCancelingThrough("<Keyboard>/escape");
        _operation.WithControlsExcluding("<Pointer>/position").WithControlsExcluding("<Pointer>/delta");
        _operation.WithTargetBinding(bindingId);
        _operation.OnComplete(_operation => OnRebindComplete(bindingId));
        _operation.OnCancel(_operation => OnRebindCancel());
        _operation.Start();
    }
    void OnRebindComplete(int bindingId)
    {
        onRebindComplete?.Invoke();
        _operation.Dispose();
        _action.Enable();
    }
    public void ResetBindings()
    {
    }
    void OnRebindCancel()
    {
        _action.Enable();
        onRebindCancel?.Invoke();
    }
    public void SelectGameObject(GameObject gameObject)
    {
        _beforeSelectedObject = _system.currentSelectedGameObject;
        _system.SetSelectedGameObject(gameObject);
    }
    public void SelectObjectAgain()
    {
        _system.SetSelectedGameObject(_beforeSelectedObject);
    }
    #endregion

    #region LoadControls
    public void LoadSettings()
    {
        ClearOverrides();
        _binds = _menu.Settings.Binds;

        string _MapName;
        string _actionName;
        string _allBinds;
        foreach (var bind in _binds)
        {
            if (bind != "")
            {
                _MapName = bind.Split(':')[0].Split('/')[0];
                _actionName = bind.Split(':')[0].Split('/')[1];
                _allBinds = bind.Split(':')[1];
                foreach (var map in _input.actions.actionMaps)
                {
                    if (map.name == _MapName)
                    {
                        if (map.FindAction(_actionName) != null)
                        {
                            InputAction action = map.FindAction(_actionName);
                            if (_allBinds.Contains(","))
                            {
                                string[] _bindings = _allBinds.Split(',');
                                for (int i = 0; i < action.bindings.Count; i++)
                                {
                                    if (_bindings[i].ToLower().Contains("vector"))
                                        continue;
                                    else
                                    action.ApplyBindingOverride(i, _bindings[i]);
                                }
                            }
                            else
                            {
                                action.ApplyBindingOverride(_allBinds);
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region SaveControls
    public void SaveControls()
    {
        List<string> _allBinds = new List<string>();
        foreach (var map in _input.actions.actionMaps)
        {
            foreach (var action in map.actions)
            {
                string _bindings = "";
                if (action.bindings.Count > 0)
                {
                    foreach (var binding in action.bindings)
                    {
                        if (binding.effectivePath != binding.path)
                        {
                            _bindings += binding.effectivePath + ',';
                        }
                        //else if (binding.overridePath == binding.path)
                        //{
                        //    _bindings += binding.overridePath + ',';
                        //}
                        else
                        {
                            _bindings += binding.path + ',';
                        }
                    }
                    _allBinds.Add($"{action.actionMap.name}/{action.name}:{_bindings.TrimEnd(',')}");
                }
                else
                {
                    _allBinds.Add($"{action.actionMap.name}/{action.name}:{action.bindings[0]}");
                }
            }
        }
        _binds = _allBinds.ToArray();
    }
    #endregion
}