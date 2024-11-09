using UnityEngine;

[CreateAssetMenu(menuName = "Outcore SDK/Player settings")]
public class PlayerSettings : ScriptableObject
{
    #region Getters
    public float SpeedMultiplier { get { return _speedMultiplier; } }
    public float PlayerFlySpeed { get { return _playerFlySpeed; } }

    public float MovementDeacceleration { get { return _movementDeacceleration; } }
    public float AirMovementDeacceleration { get { return _airMovementDeacceleration; } }
    public float MovementAcceleration { get { return _movementAcceleration; } }
    public float PlayerSpeed { get { return _playerSpeed; } }
    public float DownGravityScale { get { return _downGravityScale; } }
    public float MaxUpwardsGravity { get { return _maxUpwardsGravity; } }
    public float MinGravityScale { get { return _minGravityScale; } }
    public float MaxGravityScale { get { return _maxGravityScale; } }
    public float MaxDownwardsGravity { get { return _maxDownwardsGravity; } }
    public float VelocityForApplyRotation { get { return _velocityForApplyRotation; } }
    public float TimerForJump { get { return _timerForJump; } }
    public float RotationSpeed { get { return _rotationSpeed; } }
    public float JumpForce { get { return _jumpForce; } }
    public float DashDistance { get { return _dashDistance; } }
    public float DashScanDistance { get { return _dashScanDistance; } }
    public float PostDashSpeed { get { return _postDashSpeed; } }
    public int MaxDashes { get { return _maxDashes; } }
    public int DashAmountOnReset { get { return _dashAmountOnReset; } }
    public bool BoostDiagonalDashes { get { return _boostDiagonalDashes; } }
    #endregion

    #region Variables
    [Header("Movement")]
    [SerializeField] float _speedMultiplier;
    [SerializeField] float _playerSpeed;
    [SerializeField] float _playerFlySpeed;
    [SerializeField] float _movementAcceleration;
    [SerializeField] float _movementDeacceleration;
    [SerializeField] float _airMovementDeacceleration;

    [Header("Gravity")]
    [SerializeField] float _minGravityScale;
    [SerializeField] float _maxGravityScale;
    [SerializeField] float _downGravityScale;
    [SerializeField] float _maxDownwardsGravity;
    [SerializeField] float _maxUpwardsGravity;

    [Header("Rotation in air")]
    [SerializeField] float _velocityForApplyRotation;
    [SerializeField] float _rotationSpeed;

    [Header("Jump")]
    [SerializeField] float _timerForJump;
    [SerializeField] float _jumpForce;

    [Header("Dash")]
    [SerializeField] float _dashDistance;
    [SerializeField] float _dashScanDistance;
    [SerializeField] float _postDashSpeed = 7.5f;
    [SerializeField] int _maxDashes = 1;
    [SerializeField] int _dashAmountOnReset = 1;
    [SerializeField] bool _boostDiagonalDashes = true;
    #endregion
}
