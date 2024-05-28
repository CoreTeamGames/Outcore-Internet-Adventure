using UnityEngine;

namespace OutcoreInternetAdventure.Player.Dash
{
	public interface IDashCollisionHandler
	{
		bool shouldProcessCallbacks { get; }

		bool CanProcessCollision(Dasher dasher, Collider2D casterCollider, RaycastHit2D closestResult, Vector2 direction, float distance);

		void HandleCollision(Dasher dasher, RaycastHit2D closestResult, ref Dasher.DashPoint dashPoint);
	}
}