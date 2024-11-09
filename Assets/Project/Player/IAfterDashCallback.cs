namespace OutcoreInternetAdventure.Player.Dash
{
	public interface IAfterDashCallback
	{
		void OnAfterDash(Dasher dasher, Dasher.DashPoint dashPoint);
	}
}