using UnityEngine;
using WTF_GameJam.Player;

namespace WTF_GameJam
{
	public class WinVolume : MonoBehaviour
	{
		public UIHandler UIHandler { get; private set; }

		private bool _isWinTriggered = false;

		private void Start()
		{
			UIHandler = FindFirstObjectByType<UIHandler>();
		}

		private void OnTriggerEnter( Collider other )
		{
			if (other.CompareTag( "Player" ) && _isWinTriggered == false)
			{
				_isWinTriggered = true;
				UIHandler.ShowWinUI();
			}
		}
	}
}