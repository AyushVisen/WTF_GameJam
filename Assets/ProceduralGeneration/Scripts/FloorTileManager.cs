using System.Collections.Generic;
using UnityEngine;

namespace Ayush.ProcGen
{
	public class FloorTileManager : MonoBehaviour
	{
		[field: SerializeField]
		public Transform FloorTileParent { get; private set; }

		[SerializeField]
		private FloorTile _floorTile;

		[field: SerializeField]
		public SerializedDictionary<Vector3Int, FloorTile> FloorTileDirectory { get; private set; }

		public void SpawnFloorTiles( HashSet<Vector3Int> path, int tileSize )
		{
			ClearAllFloorTiles();

			foreach (var position in path)
			{
				SpawnFloorTile( position, tileSize );
			}
		}

		public void SpawnFloorTile( Vector3Int spawnPosition, int tileSize )
		{
			var floorTile = Instantiate( _floorTile, spawnPosition, Quaternion.identity );
			floorTile.InitTile( FloorTileParent, tileSize );
			FloorTileDirectory[spawnPosition] = floorTile;
		}

		public void RemoveFloorTile( Vector3Int position )
		{
			if (FloorTileDirectory.ContainsKey( position ))
			{
				DestroyImmediate( FloorTileDirectory[position].gameObject );
				FloorTileDirectory.Remove( position );
			}
		}

		public void ClearAllFloorTiles()
		{
			foreach (var floorTile in FloorTileDirectory.Values)
			{
				DestroyImmediate( floorTile.gameObject );
			}

			FloorTileDirectory.Clear();
		}

		public void SetWalls()
		{
			foreach (var (position, floorTile) in FloorTileDirectory)
			{
				var wallsDirection = GetMissingNeighboursDirection( position, floorTile.TileSize );

				floorTile.SetWalls( wallsDirection );
			}
		}

		public List<Vector3Int> GetNeighboursDirection( Vector3Int position, int tileSize )
		{
			var neighboursPosition = new List<Vector3Int>();

			foreach (var neighbourDirection in CardinalDirections.AllDirections)
			{
				var neighbourPosition = position + neighbourDirection * tileSize;

				if (FloorTileDirectory.ContainsKey( neighbourPosition ))
				{
					neighboursPosition.Add( neighbourDirection );
				}
			}

			return neighboursPosition;
		}

		public List<Vector3Int> GetMissingNeighboursDirection( Vector3Int position, int tileSize )
		{
			var missingNeighboursPosition = new List<Vector3Int>();

			foreach (var neighbourDirection in CardinalDirections.AllDirections)
			{
				var neighbourPosition = position + neighbourDirection * tileSize;

				if (!FloorTileDirectory.ContainsKey( neighbourPosition ))
				{
					missingNeighboursPosition.Add( neighbourDirection );
				}
			}

			return missingNeighboursPosition;
		}
	}
}