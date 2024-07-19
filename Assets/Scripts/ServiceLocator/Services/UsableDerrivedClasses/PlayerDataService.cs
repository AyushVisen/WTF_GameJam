using WTF_GameJam;
using UnityEngine;

namespace Ayush
{
	public class PlayerDataService : GlobalService
	{
		public PlayerData PlayerData { get; private set; }

		protected override void OnInit( bool serviceAdded )
		{
			base.OnInit( serviceAdded );
			var playerDataJson = PlayerPrefs.GetString( Constants.PlayerData, "" );
			PlayerData = string.IsNullOrEmpty( playerDataJson ) ? new PlayerData() : JsonUtility.FromJson<PlayerData>( playerDataJson );
			IsReady = true;
		}

		protected override void OnDisposed( bool serviceRemoved )
		{
			PlayerPrefs.SetString( Constants.PlayerData, JsonUtility.ToJson( PlayerData ) );
			base.OnDisposed( serviceRemoved );
		}
	}
}
