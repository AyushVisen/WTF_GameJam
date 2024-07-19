using System.Collections.Generic;
using UnityEngine;

namespace Ayush.ProcGen
{
	public static class ProceduralGeneration
	{
		public static List<BoundsInt> BinarySplitPartioning( BoundsInt entireBounds, int minWidth, int minDepth, int tileSize )
		{
			var rooms = new List<BoundsInt>();
			var roomsToSplit = new Queue<BoundsInt>();
			roomsToSplit.Enqueue( entireBounds );

			while (roomsToSplit.Count > 0)
			{
				var roomToSplit = roomsToSplit.Dequeue();

				if (roomToSplit.size.x <= minWidth && roomToSplit.size.z <= minDepth)
				{
					rooms.Add( roomToSplit );
					continue;
				}

				if (Random.value <= 0.5f)
				{
					if (roomToSplit.size.x > (minWidth * 2))
					{
						SplitWidth( roomToSplit, roomsToSplit, minWidth, tileSize );
					}
					else if (roomToSplit.size.z > (minDepth * 2))
					{
						SplitDepth( roomToSplit, roomsToSplit, minDepth, tileSize );
					}
					else
					{
						rooms.Add( roomToSplit );
					}
				}
				else
				{
					if (roomToSplit.size.z > (minDepth * 2))
					{
						SplitDepth( roomToSplit, roomsToSplit, minDepth, tileSize );
					}
					else if (roomToSplit.size.x > (minWidth * 2))
					{
						SplitWidth( roomToSplit, roomsToSplit, minWidth, tileSize );
					}
					else
					{
						rooms.Add( roomToSplit );
					}
				}
			}

			return rooms;
		}

		private static void SplitDepth( BoundsInt roomToSplit, Queue<BoundsInt> roomsToSplit, int minDepth, int tileSize )
		{
			var zSplit = Random.Range( tileSize, roomToSplit.size.z - tileSize );
			var room1 = new BoundsInt( roomToSplit.min, new Vector3Int( roomToSplit.size.x, roomToSplit.size.y, zSplit ) );
			var room2 = new BoundsInt( new Vector3Int( roomToSplit.min.x, roomToSplit.min.y, roomToSplit.min.z + zSplit ), new Vector3Int( roomToSplit.size.x, roomToSplit.size.y, roomToSplit.size.z - zSplit ) );

			roomsToSplit.Enqueue( room1 );
			roomsToSplit.Enqueue( room2 );
		}

		private static void SplitWidth( BoundsInt roomToSplit, Queue<BoundsInt> roomsToSplit, int minWidth, int tileSize )
		{
			var xSplit = Random.Range( tileSize, roomToSplit.size.x - tileSize );
			var room1 = new BoundsInt( roomToSplit.min, new Vector3Int( xSplit, roomToSplit.size.y, roomToSplit.size.z ) );
			var room2 = new BoundsInt( new Vector3Int( roomToSplit.min.x + xSplit, roomToSplit.min.y, roomToSplit.min.z ), new Vector3Int( roomToSplit.size.x - xSplit, roomToSplit.size.y, roomToSplit.size.z ) );

			roomsToSplit.Enqueue( room1 );
			roomsToSplit.Enqueue( room2 );
		}

		public static HashSet<Vector3Int> RandomWalk( int numberOfWalks, int steps, Vector3Int startPoint, int stepSize, bool ensureUniqueStep )
		{
			var path = new HashSet<Vector3Int>
			{
				startPoint
			};

			var currentPoint = startPoint;

			for (var i = 0; i < numberOfWalks; i++)
			{
				for (var j = 0; j < steps; j++)
				{
					var nextPoint = currentPoint + CardinalDirections.GetRandomDirection() * stepSize;
					if (ensureUniqueStep)
					{
						var totalDirecrionsCount = CardinalDirections.AllDirections.Length;
						var directionsTried = 1;
						while (path.Contains( nextPoint ) && directionsTried < totalDirecrionsCount)
						{
							nextPoint = currentPoint + CardinalDirections.GetRandomDirection() * stepSize;
							directionsTried++;
						}
					}
					currentPoint = nextPoint;
					path.Add( currentPoint );
				}
			}

			return path;
		}
	}

	public static class CardinalDirections
	{
		public static Vector3Int North => new( 0, 0, 1 );
		public static Vector3Int South => new( 0, 0, -1 );
		public static Vector3Int East => new( 1, 0, 0 );
		public static Vector3Int West => new( -1, 0, 0 );

		public static Vector3Int[] AllDirections => new[] { North, South, East, West };

		public static Vector3Int GetRandomDirection()
		{
			return AllDirections[Random.Range( 0, AllDirections.Length )];
		}
	}
}