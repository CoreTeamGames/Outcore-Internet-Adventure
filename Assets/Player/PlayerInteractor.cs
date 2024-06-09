using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;
        InputActionMap _inputActions;
        IInteractor _interactor;
        [SerializeField] GameObject _interactorHint;
        [SerializeField] TMP_Text _interactorKeyText;
        [SerializeField] string _interactorKeyName = "Interact";
        [SerializeField] Vector2 _offset = Vector2.up;
        Transform _firstRoot;
        Vector3 _firstPosition;

        public void Awake()
        {
            _firstPosition = _interactorHint.transform.localPosition;
            _firstRoot = _interactorHint.transform.parent;
            _inputActions = gameObject.transform.parent.gameObject.GetComponentInChildren<UnityEngine.InputSystem.PlayerInput>().currentActionMap;
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractor>(out IInteractor interactor))
            {
                _interactor = interactor;
                InitializeHint();
                _interactorHint.transform.parent = collision.transform;
                _interactorHint.transform.localPosition = new Vector3(_offset.x,_offset.y,_firstPosition.z);
                _interactorHint.SetActive(true);
            }
        }

        void InitializeHint()
        {
            _interactorKeyText.text = _inputActions.FindAction(_interactorKeyName).bindings[0].path.Split('/')[1];
        }

        public void TryInteract()
        {
            if (_interactor != null)
                _interactor.StartEvent();
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            _interactor = null;
            _interactorHint.SetActive(false);
            _interactorHint.transform.parent = _firstRoot;
            _interactorHint.transform.localPosition = _firstPosition;
        }
    }
}