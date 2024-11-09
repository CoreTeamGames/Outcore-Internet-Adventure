using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    public class PlayerHealth : MonoBehaviour, IDamagable
    {
        #region Events
        public delegate void OnPlayerDeath();
        public OnPlayerDeath PlayerDeathEvent;

        public delegate void OnPlayerSubHealthDamage();
        public OnPlayerDeath PlayerSubHealthDamageEvent;

        public delegate void OnPlayerHealthDamage();
        public OnPlayerDeath PlayerHealthDamageEvent;

        public delegate void OnPlayerBlockDamage();
        public OnPlayerDeath PlayerBlockDamageEvent;
        #endregion
        #region Variables
        [HideInInspector] public PlayerEvents events;

        [SerializeField] int health = 5;
        [SerializeField] int subHealth = 3;
        [Space(10)]
        [SerializeField] int maxHealth = 5;
        [SerializeField] int maxSubHealth = 3;
        #endregion
        #region Base code for health
        public void ApplyDamage(int damage)
        {
            if (damage >= 0)
            {
                if (subHealth != 0)
                {
                    subHealth -= damage;
                    PlayerSubHealthDamageEvent?.Invoke();
                }
                else if (subHealth <= 0 && health > 0)
                {
                    health--;
                    PlayerHealthDamageEvent?.Invoke();
                    subHealth = maxSubHealth;
                }
                else if (subHealth <= 0 && health <= 0) PlayerDeath();
            }
            else
            {
                damage = -damage;
                ApplyDamage(damage);
            }
        }

        public void BlockDamage() => PlayerBlockDamageEvent?.Invoke();

        public void PlayerDeath() => PlayerDeathEvent?.Invoke();
        #endregion
    }
}