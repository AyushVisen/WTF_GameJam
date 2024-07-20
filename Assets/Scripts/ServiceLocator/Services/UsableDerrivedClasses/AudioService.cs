using WTF_GameJam;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Ayush
{
	public class AudioService : GlobalService
	{
		[SerializeField]
		private AudioMixer _audioMixer;

		[SerializeField]
		private AudioSource _audioSourceMusic;

		[SerializeField]
		private AudioSource _audioSourceSFX;

		[SerializeField]
		private AudioClip _buttonSound;

		private PlayerData _playerData;

		#region Overrides of GlobalService

		protected override void OnInit( bool serviceAdded )
		{
			base.OnInit( serviceAdded );
			StartCoroutine( InitAfterDependenciesResolved() );
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		protected override void OnDisposed( bool serviceRemoved )
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			base.OnDisposed( serviceRemoved );
		}

		private void OnSceneLoaded( Scene arg0, LoadSceneMode arg1 )
		{
			if (Camera.main != null)
			{
				transform.position = Camera.main.transform.position;
			}
		}

		#endregion

		#region Setup

		IEnumerator InitAfterDependenciesResolved()
		{
			yield return new WaitUntil( () =>
										{
											if (GameManager.TryGetService<PlayerDataService>( out var playerDataService ) && playerDataService.IsReady)
											{
												_playerData = playerDataService.PlayerData;
												return true;
											}

											return false;
										} );
			InitialiseSoundSystem();
			IsReady = true;
		}

		private void InitialiseSoundSystem()
		{
			SetMusicVolume( _playerData.Music_On );
			SetSfxVolume( _playerData.SFX_On );
		}

		public void SetMusicVolume( bool value )
		{
			_playerData.Music_On = value;
			_audioMixer.SetFloat( Constants.MusicVolume, GetVolume( value ) );
		}

		public void SetSfxVolume( bool value )
		{
			_playerData.SFX_On = value;
			_audioMixer.SetFloat( Constants.SfxVolume, GetVolume( value ) );
		}

		private float GetVolume( bool value )
		{
			return value ? 0f : -80f;
		}

		#endregion

		#region Public APIs

		public void PlayBGM( bool value, AudioClip audioClip = null )
		{
			if (audioClip != null)
			{
				_audioSourceMusic.clip = audioClip;
			}

			if (value)
			{
				_audioSourceMusic.Play();
			}
			else
			{
				_audioSourceMusic.Stop();
			}
		}

		public void PlaySfx( AudioClip audioClip )
		{
			_audioSourceSFX.PlayOneShot( audioClip );
		}

		public void PlayButtonSound()
		{
			if (_buttonSound == null)
				return;

			PlaySfx( _buttonSound );
		}

		#endregion
	}
}
