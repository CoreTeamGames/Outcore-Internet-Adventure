using UnityEngine;

namespace OutcoreInternetAdventure.Player.Dash
{
	public class DashAmmoCollector : MonoBehaviour
	{
		public Dasher dasher;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			CheckForDashPowerup(collision);
		}

		private void OnCollisionEnter2D(Collision2D collision)
		{
			CheckForDashPowerup(collision.collider);
		}

		private void CheckForDashPowerup(Collider2D collider)
		{
			DashAmmoPowerup component = collider.GetComponent<DashAmmoPowerup>();
			if (component != null && component.grantOnTrigger)
			{
				component.GrantDash(dasher);
				if (component.destroyOnCollection)
				{
					component.Destroy();
				}
			}
		}
	}
}