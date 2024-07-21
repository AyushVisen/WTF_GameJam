using System.Collections.Generic;
using UnityEngine;

namespace WTF_GameJam.AI
{
	public class EnemySpawnner : MonoBehaviour
	{
		[field: SerializeField]
		public List<GameObject> EnemyPrefabs { get; private set; }

		[field: SerializeField]
		public float MinDistanceFromPlayer { get; private set; } = 15f;

		[field: SerializeField]
		public Vector2 SpawnDurationIntervalMinMax { get; private set; } = new Vector2( 5, 10 );

		private float _lastSpawnCounter = 0f;

		private Transform _playerTransform;

		private void Start()
		{
			_playerTransform = GameObject.FindGameObjectWithTag( "Player" ).transform;
		}

		private void Update()
		{
			if(Vector3.Distance( _playerTransform.position, transform.position ) > MinDistanceFromPlayer)
			{
				return;
			}

			if(_lastSpawnCounter <= 0)
			{
				Instantiate( GetRandomPrefabToSpawn(), transform.position, Quaternion.identity );
				_lastSpawnCounter = Random.Range( SpawnDurationIntervalMinMax.x, SpawnDurationIntervalMinMax.y );
			}
			else
			{
				_lastSpawnCounter -= Time.deltaTime;
			}

		}

		private GameObject GetRandomPrefabToSpawn()
		{
			return EnemyPrefabs[Random.Range( 0, EnemyPrefabs.Count )];
		}
	}
}