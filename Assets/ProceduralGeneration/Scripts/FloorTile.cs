using System.Collections.Generic;
using UnityEngine;

namespace Ayush.ProcGen
{
	public class FloorTile : MonoBehaviour
	{
		[field: SerializeField]
		private Material _material;

		[field: SerializeField]
		public SerializedDictionary<Vector3Int, GameObject> WallDictionary { get; private set; }

		public int TileSize { get; private set; }

		public void InitTile( Transform parent, int tileSize )
		{
			transform.localScale = new Vector3( tileSize, 0.1f, tileSize );
			transform.SetParent( parent );
			_material.mainTextureScale = new Vector2( tileSize, tileSize );
			TileSize = tileSize;
		}

		public void SetWalls( IEnumerable<Vector3Int> wallsDirection )
		{
			foreach (var wallDirection in wallsDirection)
			{
				WallDictionary[wallDirection].SetActive( true );
			}
		}
	}
}