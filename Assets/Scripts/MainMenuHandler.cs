using Ayush;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WTF_GameJam.Menu
{
	public class MainMenuHandler : MonoBehaviour
	{
		[field: SerializeField]
		public Toggle MusicToggle { get; private set; }

		[field: SerializeField]
		public Toggle SfxToggle { get; private set; }

		[field: SerializeField]
		public AudioClip BGM { get; private set; }

		[field: SerializeField]
		public List<string> Levels { get; private set; }

	#if UNITY_EDITOR
		[field: SerializeField]
		public string SceneToLoad { get; private set; }
	#endif

		private AudioService _audioService;
		private PlayerDataService _playerDataService;

		private void Start()
		{
			if (GameManager.Instance.TryGetService<PlayerDataService>( out _playerDataService ))
			{
				MusicToggle.isOn = _playerDataService.PlayerData.Music_On;
				SfxToggle.isOn = _playerDataService.PlayerData.SFX_On;
			}

			if (GameManager.Instance.TryGetService<AudioService>(out _audioService))
			{
				_audioService.SetMusicVolume(MusicToggle.isOn);
				_audioService.SetSfxVolume( SfxToggle.isOn);
			}

			_audioService.PlayBGM( true, BGM );
		}

		public void OnMusicToggle(bool value)
		{
			_audioService.SetMusicVolume( MusicToggle.isOn );
		}

		public void OnSfxToggle(bool value)
		{
			_audioService.SetSfxVolume( SfxToggle.isOn );
		}

		public void OnStartGame()
		{
			var randomIndex = Random.Range( 0, Levels.Count );
			SceneManager.LoadScene( Levels[randomIndex] );
		}

		public void OnQuitGame()
		{
			Application.Quit();
		}

		#if UNITY_EDITOR
		[ContextMenu( "Load Scene" )]
		public void LoadScene()
		{
			SceneManager.LoadScene( SceneToLoad );
		}
		#endif
	}
}