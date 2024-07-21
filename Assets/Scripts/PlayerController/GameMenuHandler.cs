using System;
using UnityEngine;
using WTF_GameJam.Health;

namespace WTF_GameJam.Player
{
	public class GameMenuHandler : MonoBehaviour
	{
		[field: SerializeField]
		public HealthBehavior PlayerHealthBehaviour { get; private set; }

		[field: SerializeField]
		public GameObject GameOverMenu { get; private set; }

		[field: SerializeField]
		public GameObject PauseMenu { get; private set; }

		[field: SerializeField]
		public GameObject WinMenu { get; private set; }

		private void Start()
		{
			GameOverMenu.SetActive( false );
			PauseMenu.SetActive( false );

			PlayerHealthBehaviour = FindFirstObjectByType<HealthBehavior>();
		}

		private void Update()
		{
			if (PlayerHealthBehaviour.IsDead && GameOverMenu.activeSelf == false)
			{
				ShowGameOverMenu();
				return;
			}

			if(Input.GetKeyDown(KeyCode.Escape))
			{
				if (PauseMenu.activeSelf)
				{
					PauseMenu.SetActive( false );
					Time.timeScale = 1;
				}
				else
				{
					PauseMenu.SetActive( true );
					Time.timeScale = 0;
				}
			}
		}

		private void ShowGameOverMenu()
		{
			GameOverMenu.SetActive( true );
		}

		public void GoToHome()
		{
			Time.timeScale = 1;
			UnityEngine.SceneManagement.SceneManager.LoadScene( 0 );
		}
	}
}