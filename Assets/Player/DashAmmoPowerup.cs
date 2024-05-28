using UnityEngine;
using UnityEngine.Events;

namespace OutcoreInternetAdventure.Player.Dash
{
	public class DashAmmoPowerup : MonoBehaviour
	{
		public bool grantOnTrigger = true;

		public bool destroyOnCollection = true;

		public bool applyCooldownOnGrant;

		public bool resetCooldownOnLand = true;

		public bool grantOnlyIfPlayerHasNoDashes = true;

		public UnityEvent onDashNotGranted;

		public UnityEvent onDashGranted;

		public UnityEvent onCooldownOver;

		public bool isOnCooldown { get; private set; }



		public bool CanGrantDash(Dasher dasher)
		{
			if (grantOnlyIfPlayerHasNoDashes && dasher.numberOfDashesLeft > 0)
			{
				return false;
			}
			if (isOnCooldown)
			{
				return false;
			}
			return true;
		}

		public void GrantDash(Dasher dasher)
		{
			if (!CanGrantDash(dasher))
			{
				onDashNotGranted.Invoke();
				return;
			}
			dasher.GrantDash();
			onDashGranted.Invoke();
			if (applyCooldownOnGrant)
			{
				isOnCooldown = true;
			}
			if (resetCooldownOnLand)
			{
				FinishCooldown();
			}
		}

		public void Destroy()
		{
			Object.Destroy(base.gameObject);
		}

		private void OnDisable()
		{
			if (isOnCooldown)
			{
				FinishCooldown();
			}
		}

		private void FinishCooldown()
		{
			isOnCooldown = false;
			onCooldownOver.Invoke();
		}
	}
}