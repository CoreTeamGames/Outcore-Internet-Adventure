using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerEvents : MonoBehaviour
    {
        public bool OnGround { get { return input.OnGround; } }

        #region Variables

        public PlayerAudio audio;
        public PlayerCamera camera;
        public PlayerCollisionChecker checker;
        public Dash.Dasher dasher;
        public PlayerHealth health;
        public PlayerInput input;
        public PlayerInteractor interactor;
        public DialogueSystem.DialogueGameWindow dialogueWindow;

        bool _canFly;
        #endregion

        #region Code
        public void Awake()
        {
            checker.events = this;
            audio.events = this;
            camera.events = this;
            dasher.events = this;
            health.events = this;
            input.events = this;
            interactor.events = this;
            dialogueWindow.events = this;

            SubscribeToEvents();
        }

        public void OnEnable()
        {
            SubscribeToEvents();
        }

        public void OnApplicationPause(bool pause)
        {
            if (pause == false)
                SubscribeToEvents();
        }

        public void SubscribeToEvents()
        {
            checker.onLandBigImpactEvent += OnPlayerLandBigParticles;
            checker.onLandSmallImpactEvent += OnPlayerLandSmallParticles;
            checker.onLandNoImpactEvent += OnPlayerLandNoParticles;
            checker.onLandEvent += OnPlayerLand;
            checker.onLoseGroundEvent += OnPlayerLoseGround;

            dasher.onPlayerDashedEvent += OnPlayerDashed;
            dasher.onPlayerDashingEvent += onPlayerDashing;
            dasher.onPlayerStartDashingEvent += onPlayerStartDashing;
            dasher.onPlayerDashHitEvent += onPlayerDashHit;
            dasher.onPlayerDashMissEvent += onPlayerDashMiss;

            input.onPlayerJumpEvent += onPlayerJump;
            input.onPlayerLandedEvent += onPlayerGrounded;
            input.onPlayerStartFlyEvent += onPlayerStartFly;
            input.onPlayerStartWalkEvent += onPlayerStartWalk;

            health.PlayerDeathEvent += PlayerDeath;
            health.PlayerSubHealthDamageEvent += PlayerSubHealth;
            health.PlayerHealthDamageEvent += PlayerHealthDamage;
            health.PlayerBlockDamageEvent += PlayerBlockDamage;

            dialogueWindow.onDialogueStartsEvent += onDialogueStarts;
            dialogueWindow.onNewLineStartsWritingEvent += onNewLineStartsWriting;
            dialogueWindow.onPlayerSkipWriteEvent += onPlayerSkipWrite;
            dialogueWindow.onDialogueEndEvent += onDialogueEnd;

        }
        #region Events methods
        private void onPlayerStartDashing() { audio.PlayCilp(audio.slash); }
        private void onPlayerDashing() { input.canJump = false; }
        private void OnPlayerDashed()
        {
            if (input.OnGround)
            {
                input.OnPlayerLand();
                dasher.OnLand();
            }
            else
            {
                input.OnLoseGround();
            }
        }
        private void OnPlayerLandNoParticles(Collision2D collision) => input.OnLanded(false, collision);
        private void OnPlayerLandSmallParticles(Collision2D collision) => input.OnLanded(true, collision);
        private void OnPlayerLandBigParticles(Collision2D collision) => input.OnLanded(true, collision, true);
        private void OnPlayerLand(Collision2D collision) => dasher.OnLand();
        private void OnPlayerLoseGround() => input.OnLoseGround();
        private void onPlayerDashHit() => audio.PlayCilp(audio.hit);
        private void onPlayerDashMiss() => audio.PlayCilp(audio.dashMiss);
        private void onPlayerJump() => audio.PlayCilp(audio.jump);
        private void onPlayerGrounded() { audio.PlayCilp(audio.land); dasher.OnLand(); }
        private void onPlayerStartFly() { }
        private void onPlayerStartWalk() { }
        private void PlayerDeath() => audio.PlayCilp(audio.death);
        private void PlayerSubHealth() { }
        private void PlayerHealthDamage() { }
        private void PlayerBlockDamage() { }
        private void onDialogueStarts(bool canMove = false) { if (!canMove) input.BlockMovement();  }
        private void onNewLineStartsWriting() { }
        private void onPlayerSkipWrite() { }
        private void onDialogueEnd() { input.UnblockMovement(); }

        #endregion
        #endregion
    }
}