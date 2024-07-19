namespace Ayush
{
	public interface IUpdateCallback
	{
		int Priority => 10;

		bool ShouldUpdate => true;

		void UpdateCallback();

		void RegisterUpdateCallback();

		void UnregisterUpdateCallback();
	}
}