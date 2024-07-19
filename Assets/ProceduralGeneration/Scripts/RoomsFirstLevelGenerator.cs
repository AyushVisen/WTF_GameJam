using System.Collections.Generic;
using UnityEngine;

namespace Ayush.ProcGen
{
	public class RoomsFirstLevelGenerator : SimpleRandomWalkLevelGenerator
	{
		[field: SerializeField]
		public int MinRoomWidth { get; private set; }

		[field: SerializeField]
		public int MinRoomDepth { get; private set; }

		[field: SerializeField]
		public int LevelWidth { get; private set; }

		[field: SerializeField]
		public int LevelDepth { get; private set; }

		[field: SerializeField]
		public int Offest { get; private set; }

		public override void GenerateLevel()
		{
			var roomBounds = ProceduralGeneration.BinarySplitPartioning( new BoundsInt( LevelGeneratorParameters.StartPoint, new Vector3Int( LevelWidth, 5, LevelDepth ) ), MinRoomWidth, MinRoomDepth, LevelGeneratorParameters.StepSize );

			var floorTiles = CreateSimpleRoomsFloorTiles( roomBounds, LevelGeneratorParameters.StepSize );
			FloorTileManager.SpawnFloorTiles( floorTiles, LevelGeneratorParameters.StepSize );
			FloorTileManager.SetWalls();
		}

		private HashSet<Vector3Int> CreateSimpleRoomsFloorTiles( List<BoundsInt> roomBounds, int tileSize )
		{
			var floorTiles = new HashSet<Vector3Int>();
			foreach (var room in roomBounds)
			{
				for (var col = Offest; col < room.size.x - Offest; col += tileSize)
				{
					for (var row = Offest; row < room.size.z - Offest; row += tileSize)
					{
						var position = new Vector3Int( room.min.x + col, room.min.y, room.min.z + row );
						floorTiles.Add( position );
					}
				}
			}

			return floorTiles;
		}
	}
}