using System.Collections;
using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    [RequireComponent(typeof(PlayerInputManager))]
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerInput : MonoBehaviour
    {
        #region Events
        public delegate void onPlayerJump();
        public onPlayerJump onPlayerJumpEvent;

        public delegate void onPlayerGrounded();
        public onPlayerGrounded onPlayerLandedEvent;

        public delegate void onPlayerStartFly();
        public onPlayerStartFly onPlayerStartFlyEvent;

        public delegate void onPlayerStartWalk();
        public onPlayerStartWalk onPlayerStartWalkEvent;
        #endregion

        #region Variables
        [SerializeField] PlayerSettings _settings;
        [HideInInspector] public PlayerEvents events;
        [Header("Set player variables")]
        [Header("Ground movement setup")]

        [Header("Set player components")]
        public Rigidbody2D playerRigidBody2D;
        public GameObject player;
        [SerializeField] bool _setPlayerParentOnLand;
        [SerializeField] GameObject[] _dustPrefabs;
        [SerializeField] Animator playerAnimator;
        [SerializeField] Transform rotateTransform;
        [SerializeField] SpriteRenderer playerSpriteRenderer;


        public enum movingType
        {
            walk = 0,
            fly = 1
        }

        public enum states
        {
            idle,
            run,
            spin,
            jump,
            fly_idle,
            fly_run,
            flying
        }

        [Space(5)]
        [Header("Player states and abilities")]
        [Space(5)]
        [SerializeField] states _playerStates = states.idle;
        [SerializeField] movingType _playerMovingType;
        [SerializeField] bool _flyAbility = false;
        [SerializeField] bool _WalkAbility = true;
        [SerializeField] bool _jumpAbility = true;
        public bool _canJump = true;
        [SerializeField] bool _onGround = true;
        bool _flipX;

        public states PlayerStates { get { return _playerStates; } }
        public movingType PlayerMovingType { get { return _playerMovingType; } }
        public bool FlyAbility { get { return _flyAbility; } }
        public bool WalkAbility { get { return _WalkAbility; } }
        public bool JumpAbility { get { return _jumpAbility; } }
        public bool CanJump { get { return _canJump; } }
        public bool IsJumped { get { return _isJumped; } }
        public bool OnGround { get { return _onGround; } }

        private bool _timerStarted = false;
        bool _isJumped = false;
        private bool _isFlying = false;
        public Vector2 MovingVector { get; set; }
        [SerializeField] StretchAndSquasher _stretcher;
        Vector2 velocity;

        #endregion

        #region Code
        IEnumerator Timer()
        {
            yield return new WaitForSeconds(_settings.TimerForJump);
            if (!_onGround)
                _canJump = false;
        }

        public void OnPlayerLand()
        {
            _onGround = true;
            _timerStarted = false;
            _isJumped = false;
            _canJump = true;
            if (rotateTransform.rotation.z != 0)
                rotateTransform.rotation = new Quaternion(0, 0, 0, 0);
        }

        public void OnLanded(bool _spawnParticles, Collision2D _collider2D, bool _strongParticles = false)
        {
            OnPlayerLand();
            if (_setPlayerParentOnLand)
                player.transform.parent = _collider2D.collider.gameObject.transform;
            if (_spawnParticles)
            {
                playerSpriteRenderer.flipX = _flipX;
                onPlayerLandedEvent?.Invoke();
                Instantiate(_dustPrefabs[_strongParticles ? 1 : 0], playerRigidBody2D.position, new Quaternion(0, 0, 0, 0));
                _stretcher.SquashAnimation(_collider2D.relativeVelocity, 0.5f);
            }
        }
        public void OnLoseGround()
        {
            if (_setPlayerParentOnLand && !_onGround)
                player.transform.parent = null;
            if (!_timerStarted && OnGround)
            {
                _onGround = false;
                _timerStarted = true;
                StartCoroutine(Timer());
            }
        }
        void Update()
        {
            if (_WalkAbility && MovingVector.x != 0)
            {
                _flipX = MovingVector.x < 0;
                if (_playerStates != states.jump)
                {
                    playerSpriteRenderer.flipX = _flipX;
                }
            }

            SwitchState();

            switch (_playerMovingType)
            {
                case movingType.walk:
                    if (_onGround)
                    {
                        if (MovingVector.x != 0)
                            _playerStates = states.run;
                        else
                            _playerStates = states.idle;
                    }
                    else
                    {
                        if (playerRigidBody2D.velocity.y != 0 && (playerRigidBody2D.velocity.y <= -_settings.VelocityForApplyRotation || playerRigidBody2D.velocity.y >= _settings.VelocityForApplyRotation))
                            _playerStates = states.spin;
                        else if (_isJumped)
                            _playerStates = states.jump;

                    }
                    break;
                case movingType.fly:
                    break;
                default:
                    break;
            }
        }
        void FixedUpdate()
        {
            float num = Mathf.Abs(playerRigidBody2D.velocity.y * 0.75f);
            float max = ((playerRigidBody2D.velocity.y > 0f) ? (_settings.MaxUpwardsGravity * _settings.MaxGravityScale) : _settings.MaxDownwardsGravity);
            playerRigidBody2D.gravityScale = Mathf.Clamp(num * _settings.DownGravityScale, _settings.MinGravityScale, max);
            Move(MovingVector);
        }

        void WalkMove(Vector2 _movingVector)
        {
            if (!(events.dasher == null) && events.dasher.isDashing)
            {
                return;
            }
            velocity = playerRigidBody2D.velocity;
            float num2 = 0f;
            if (_movingVector.x != 0f)
            {
                if (_movingVector.x > 0f)
                {
                    if (velocity.x < _settings.PlayerSpeed)
                    {
                        num2 += _settings.MovementAcceleration;
                    }
                }
                else if (_movingVector.x < 0f && velocity.x > 0f - _settings.PlayerSpeed)
                {
                    num2 -= _settings.MovementAcceleration;
                }
                velocity.x += num2 * Time.fixedDeltaTime;
            }
            else if (velocity.x != 0f)
            {
                float num3 = Mathf.Sign(velocity.x);
                float num4 = num3 * (OnGround ? _settings.MovementDeacceleration : _settings.AirMovementDeacceleration);
                if (Mathf.Sign(velocity.x - num4) == num3)
                {
                    velocity.x -= num4;
                }
                else
                {
                    velocity.x = 0f;
                }
                if (Mathf.Abs(velocity.x) < 0.01f)
                {
                    velocity.x = 0f;
                }
            }
            playerRigidBody2D.velocity = velocity * _settings.SpeedMultiplier;
        }
        void FlyMove(Vector2 _movingVector)
        {
            if (_movingVector.x != 0 || _movingVector.y != 0)
                _playerStates = states.flying;
            else
                _playerStates = states.fly_idle;
            playerRigidBody2D.velocity = transform.TransformDirection(_movingVector * _settings.PlayerFlySpeed * 100 * Time.fixedDeltaTime);
        }
        void SwitchState()
        {
            switch (_playerStates)
            {
                case states.idle:
                    playerAnimator.Play("stand");
                    break;

                case states.run:
                    playerAnimator.Play("Run");
                    break;

                case states.spin:
                    playerAnimator.Play("Spin");
                    break;

                case states.jump:
                    playerAnimator.Play("Jump");
                    break;

                case states.fly_idle:
                    playerAnimator.Play("Fly idle");
                    break;

                case states.flying:
                    playerAnimator.Play("Fly");
                    break;


            }

        }
        void Jump(float _jumpForce)
        {
            _onGround = false;
            _canJump = false;
            _isJumped = true;
            velocity.y = _jumpForce;
            playerRigidBody2D.velocity = velocity;
            onPlayerJumpEvent?.Invoke();
            if (_setPlayerParentOnLand && !_onGround)
                player.transform.parent = null;
        }
        void RotatePlayerSprite()
        {
            float z = _settings.RotationSpeed * (0f - playerRigidBody2D.velocity.x) * Time.timeScale;
            rotateTransform.transform.Rotate(new Vector3(0f, 0f, z));
        }

        public void Jump()
        {
            Jump(_settings.JumpForce);
        }
        public void Move(Vector2 _movingVector)
        {
            switch (PlayerMovingType)
            {
                case movingType.walk:
                    WalkMove(_movingVector);
                    if (!OnGround)
                        RotatePlayerSprite();
                    break;
                case movingType.fly:
                    FlyMove(_movingVector);
                    break;
            }
        }

        public void TrySwitchFlyState()
        {
            if (FlyAbility)
            {
                if (PlayerMovingType == movingType.walk)
                    _playerMovingType = movingType.fly;
                else
                    _playerMovingType = movingType.walk;
            }
            else
            {
                _playerMovingType = movingType.walk;
            }
            _isFlying = PlayerMovingType != movingType.walk;
        }
        #endregion
    }
}