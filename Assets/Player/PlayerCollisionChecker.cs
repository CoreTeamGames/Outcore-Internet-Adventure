using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OutcoreInternetAdventure.Player
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerCollisionChecker : MonoBehaviour
    {
        public PlayerEvents events;
        public delegate void onLandBigImpact(Collision2D collision);
        public onLandBigImpact onLandBigImpactEvent;
        public delegate void onLandSmallImpact(Collision2D collision);
        public onLandSmallImpact onLandSmallImpactEvent;
        public delegate void onLandNoImpact(Collision2D collision);
        public onLandNoImpact onLandNoImpactEvent;
        public delegate void onLand(Collision2D collision);
        public onLand onLandEvent;
        public delegate void onLoseGround();
        public onLoseGround onLoseGroundEvent;

        public void OnCollisionExit2D(Collision2D collision)
        {
            onLoseGroundEvent?.Invoke();
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.contacts[0].normal.y > 0.01f)
            {
                if (collision.contacts[0].normal == Vector2.up)
                {
                    onLandEvent?.Invoke(collision);
                    if (collision.relativeVelocity.y > 7)
                    {
                        if (collision.relativeVelocity.y > 20)
                            onLandBigImpactEvent?.Invoke(collision);
                        else
                            onLandSmallImpactEvent?.Invoke(collision);
                    }
                    else
                        onLandNoImpactEvent?.Invoke(collision);
                }
            }
        }
    }
}