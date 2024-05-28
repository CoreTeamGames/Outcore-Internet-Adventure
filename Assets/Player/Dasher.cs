using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OutcoreInternetAdventure.Player.Dash
{
    public class Dasher : MonoBehaviour
    {
        [HideInInspector] public PlayerEvents events;
        [Serializable]
        public class SettingsData
        {
            public float dashDistance;

            public float dashScanDistance;

            public float postDashSpeed = 7.5f;

            public int maxDashes = 1;

            public int dashAmountOnReset = 1;

            public bool boostDiagonalDashes = true;
        }
        public class DashPoint
        {
            public Vector2 position;

            public Vector2 velocity;

            public float distanceLeft;

            public IDashCollisionHandler dashCollisionHandler;

            public Collider2D hitCollider;

            public List<IBeforeDashCallback> beforeCallbacks = new List<IBeforeDashCallback>();

            public List<IAfterDashCallback> afterCallbacks = new List<IAfterDashCallback>();
        }

        SettingsData settings;
        public Sprite spriteOnDash;

        [Header("Components")]
        public PlayerSettings playerSettings;
        [SerializeField] bool _enableDash = true;
        [SerializeField] private Collider2D[] _colliders;
        [SerializeField] private DashTrailRenderer _dashTrailRenderer;
        public Rigidbody2D rigidbodyToMove;

        public UnityEvent onDash;
        public UnityEvent onDashHit;
        public UnityEvent onDashMiss;
        public UnityEvent onCanDash;
        public UnityEvent onCantDash;
        public UnityEvent onCantComplete;

        public delegate void onPlayerStartDashing();
        public event onPlayerStartDashing onPlayerStartDashingEvent;
        public delegate void onPlayerDashing();
        public event onPlayerDashing onPlayerDashingEvent;
        public delegate void onPlayerDashed();
        public event onPlayerDashed onPlayerDashedEvent;
        public delegate void onPlayerDashHit();
        public event onPlayerDashHit onPlayerDashHitEvent;
        public delegate void onPlayerDashMiss();
        public event onPlayerDashMiss onPlayerDashMissEvent;
        private int _numberOfDashesLeft = 1;
        private float _lastCollisionDistance = -1f;

        public bool isDashing { get; private set; }
        public float dashDistance
        {
            get
            {
                return settings.dashDistance;
            }
            set
            {
                settings.dashDistance = value;
            }
        }
        public int numberOfDashesLeft
        {
            get
            {
                return _numberOfDashesLeft;
            }
            set
            {
                if (_numberOfDashesLeft == 0 && value == 1)
                {
                    onCanDash.Invoke();
                }
                _numberOfDashesLeft = value;
                if (_numberOfDashesLeft == 0)
                {
                    onCantDash.Invoke();
                }
            }
        }

        public void Awake()
        {
            settings = new SettingsData
            {
                boostDiagonalDashes = playerSettings.BoostDiagonalDashes,
                dashAmountOnReset = playerSettings.DashAmountOnReset,
                dashDistance = playerSettings.DashDistance,
                dashScanDistance = playerSettings.DashScanDistance,
                maxDashes = playerSettings.MaxDashes,
                postDashSpeed = playerSettings.PostDashSpeed
            };
        }
        public void OnLand()
        {
            numberOfDashesLeft = 1;
        }
        public RaycastHit2D[] pooledDashResults { get; } = new RaycastHit2D[128];
        public Collider2D activeCollider
        {
            get
            {
                Collider2D[] colliders = _colliders;
                foreach (Collider2D collider2D in colliders)
                {
                    if (collider2D.isActiveAndEnabled)
                    {
                        return collider2D;
                    }
                }
                return null;
            }
        }
        public void ResetDashes()
        {
            numberOfDashesLeft = settings.dashAmountOnReset;
        }
        public void GrantDash()
        {
            numberOfDashesLeft = Math.Min(numberOfDashesLeft + 1, settings.maxDashes);
        }
        public void Dash(Vector2 dashDirection)
        {
            StartCoroutine(_Dash(dashDirection, rigidbodyToMove.position));
        }
        public void Dash(Vector2 dashDirection, Vector2 startingPoint)
        {
            StartCoroutine(_Dash(dashDirection, startingPoint));
        }
        public DashPoint GetDashDestination(DashPoint dashPoint)
        {
            rigidbodyToMove.position = dashPoint.position;
            Physics2D.SyncTransforms();
            int num = activeCollider.Cast(dashPoint.velocity.normalized, pooledDashResults, dashPoint.distanceLeft);
            for (int i = 0; i < num; i++)
            {
                RaycastHit2D raycastHit2D = pooledDashResults[i];
                if (raycastHit2D.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    continue;
                }
                Vector2 normalized = dashPoint.velocity.normalized;
                if (Vector2.Angle(normalized, raycastHit2D.normal) < 90f || (Mathf.Abs(normalized.x) == Mathf.Abs(raycastHit2D.normal.y) && Mathf.Abs(normalized.y) == Mathf.Abs(raycastHit2D.normal.x)))
                {
                    continue;
                }
                IDashCollisionHandler component = raycastHit2D.collider.GetComponent<IDashCollisionHandler>();
                if (component == null)
                {
                    continue;
                }
                dashPoint.beforeCallbacks.AddRange(HandleBeforeDashCallbacks(component, raycastHit2D));
                dashPoint.afterCallbacks.AddRange(HandleAfterDashCallbacks(component, raycastHit2D));
                if (component.CanProcessCollision(this, activeCollider, raycastHit2D, dashPoint.velocity, dashPoint.distanceLeft))
                {
                    if (_lastCollisionDistance <= 0.01f && raycastHit2D.distance <= 0.01f)
                    {
                        raycastHit2D.distance = dashPoint.distanceLeft;
                    }
                    _lastCollisionDistance = raycastHit2D.distance;
                    component.HandleCollision(this, raycastHit2D, ref dashPoint);
                    dashPoint.dashCollisionHandler = component;
                    dashPoint.hitCollider = raycastHit2D.collider;
                    return dashPoint;
                }
            }
            dashPoint.position = rigidbodyToMove.position + dashPoint.velocity * dashPoint.distanceLeft;
            dashPoint.velocity *= settings.postDashSpeed;
            dashPoint.distanceLeft = 0f;
            return dashPoint;
        }
        private float CalculateHeightCorrectionMultiplier(Vector2 direction)
        {
            float num = Mathf.Clamp(Vector2.Angle(new Vector2(Mathf.Sign(direction.x), Mathf.Sign(direction.y)), direction), 0f, 45f);
            return 1f - num / 45f;
        }
        private IBeforeDashCallback[] HandleBeforeDashCallbacks(IDashCollisionHandler closestDashCollisionHandler, RaycastHit2D dashResult)
        {
            if (closestDashCollisionHandler.shouldProcessCallbacks)
            {
                return dashResult.collider.GetComponents<IBeforeDashCallback>();
            }
            return new IBeforeDashCallback[0];
        }
        private IAfterDashCallback[] HandleAfterDashCallbacks(IDashCollisionHandler closestDashCollisionHandler, RaycastHit2D dashResult)
        {
            if (closestDashCollisionHandler.shouldProcessCallbacks)
            {
                return dashResult.collider.GetComponents<IAfterDashCallback>();
            }
            return new IAfterDashCallback[0];
        }

        private IEnumerator _Dash(Vector2 dashDirection, Vector2 startingPoint)
        {
            if (numberOfDashesLeft > 0 && _enableDash)
            {
                onPlayerStartDashingEvent?.Invoke();
                isDashing = true;
                numberOfDashesLeft--;
                onDash?.Invoke();
                _dashTrailRenderer.CreateTrail();
                DashPoint dashPoint = new DashPoint
                {
                    position = startingPoint,
                    velocity = dashDirection,
                    distanceLeft = settings.dashDistance
                };
                WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
                float originalMagnitute = dashPoint.velocity.magnitude;
                do
                {
                    onPlayerDashingEvent?.Invoke();
                    Vector2 position = dashPoint.position;
                    if (settings.boostDiagonalDashes)
                    {
                        dashPoint.velocity = dashPoint.velocity.normalized * originalMagnitute * Mathf.Max(1f, 1.4144272f * CalculateHeightCorrectionMultiplier(dashPoint.velocity.normalized));
                    }
                    dashPoint = GetDashDestination(dashPoint);
                    if (dashPoint.dashCollisionHandler != null)
                    {
                        onDashHit?.Invoke();
                        onPlayerDashHitEvent?.Invoke();
                    }
                    else
                    {
                        onDashMiss?.Invoke();
                        onPlayerDashMissEvent?.Invoke();
                    }
                    onPlayerDashedEvent?.Invoke();
                    foreach (IBeforeDashCallback beforeCallback in dashPoint.beforeCallbacks)
                    {
                        beforeCallback.OnBeforeDash(this, dashPoint);
                    }
                    bool canBeHit = false;

                    rigidbodyToMove.velocity = (canBeHit ? Vector2.zero : dashPoint.velocity);
                    rigidbodyToMove.MovePosition(dashPoint.position);
                    yield return waitForFrame;
                    if (canBeHit)
                    {
                        rigidbodyToMove.velocity = dashPoint.velocity;
                        dashPoint.distanceLeft = 0f;
                    }
                    foreach (IAfterDashCallback afterCallback in dashPoint.afterCallbacks)
                    {
                        MonoBehaviour monoBehaviour2 = (MonoBehaviour)afterCallback;
                        if (!(monoBehaviour2 == null) && !(monoBehaviour2.gameObject == null))
                        {
                            afterCallback.OnAfterDash(this, dashPoint);
                        }
                    }
                    if (dashPoint.distanceLeft > 0f)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            yield return waitForFrame;
                            _dashTrailRenderer.CreateTrail();
                        }
                    }
                }
                while (dashPoint.distanceLeft > 0f);
                isDashing = false;
                onCantComplete?.Invoke();
            }
        }
    }
}