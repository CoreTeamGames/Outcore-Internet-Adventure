using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using NaughtyAttributes;

public class RebindingButton : MonoBehaviour
{
    [SerializeField] InputActionReference _reference;
    [SerializeField] TMP_Text _bindText;
    [Dropdown("Bindings")]
    [SerializeField] int _bindingIndex = 0;
    [SerializeField] string _keysLocalizationFileName = "Keys";
    RebindindingSettings _settings;
    LocalizationService _service;
    string _localizedKey;

    private DropdownList<int> Bindings()
    {
        DropdownList<int> _bindings = new DropdownList<int>();
        if (_reference != null)
            for (int i = 0; i < _reference.action.bindings.Count; i++)
            {

                _bindings.Add((_reference.action.bindings[i].name != "" ? _reference.action.bindings[i].name : "Binding") + " : " + InputControlPath.ToHumanReadableString(_reference.action.bindings[i].path, InputControlPath.HumanReadableStringOptions.None), i);
            }
        else
            _bindings.Add("None", 0);
        return _bindings;
    }

    public void Awake()
    {
        _service = GameObject.FindGameObjectWithTag("LocalizationService").GetComponent<LocalizationService>();
        _settings = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<RebindindingSettings>();
        _settings.onRebindComplete.AddListener(GetKey);
    }

    public void OnEnable()
    {
        GetKey();
    }

    public void Rebind()
    {
        _settings.ChangeBind(_reference, _bindingIndex);
    }

    void GetKey()
    {
        _localizedKey = InputControlPath.ToHumanReadableString(_settings.Input.actions.FindAction(_reference.action.name).bindings[_bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        if (_service.LineExist(_keysLocalizationFileName, _localizedKey))
            _localizedKey = _service.GetLocalizedLine(_keysLocalizationFileName, _localizedKey);
        UpdateBind();
    }

    public void UpdateBind()
    {
        _bindText.text = _localizedKey;
    }
}