namespace OutcoreInternetAdventure.Player.Dash
{
	public interface IBeforeDashCallback
	{
		void OnBeforeDash(Dasher dasher, Dasher.DashPoint dashPoint);
	}
}