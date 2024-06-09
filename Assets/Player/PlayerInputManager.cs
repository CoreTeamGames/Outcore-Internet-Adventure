using UnityEngine;
using UnityEngine.InputSystem;


namespace OutcoreInternetAdventure.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerInputManager : MonoBehaviour
    {
        public delegate void OnAnykey(InputBinding binding);
        public OnAnykey onAnykeyEvent;
        PlayerInput _input;
        PlayerInteractor _interactor;
        Dash.Dasher _dasher;
        [SerializeField] UI.PauseMenuUI _menu;
        bool _isPaused { get { return _menu.IsPaused; } }

        public void Awake()
        {
            _interactor = gameObject.transform.parent.GetComponentInChildren<PlayerInteractor>();
            _input = GetComponent<PlayerInput>();
            if (_input.player.GetComponentInChildren<Dash.Dasher>() != null)
                _dasher = _input.player.GetComponentInChildren<Dash.Dasher>();
        }
        public void OnMove(InputValue _value)
        {
            if (!_isPaused)
                _input.MovingVector = _value.Get<Vector2>();
        }
        public void OnJump()
        {
            if (_input.JumpAbility && !_input.IsJumped && _input.CanJump && !_isPaused)
            {
                _input.Jump();
            }
        }
        public void OnFly()
        {
            if (!_isPaused)
                _input.TrySwitchFlyState();
        }
        public void OnDash()
        {
            if (_input.MovingVector.x != 0f || _input.MovingVector.y != 0f && !_isPaused)
            {
                _dasher.Dash(_input.MovingVector);
            }
        }

        public void OnPause()
        {
            if (_menu != null)
            {
                _menu.PauseGame();
            }
        }

        public void OnAnyKey(InputBinding binding)
        {
            onAnykeyEvent?.Invoke(binding);
        }
        public void OnInteract()
        {
            _interactor.TryInteract();
        }
    }
}