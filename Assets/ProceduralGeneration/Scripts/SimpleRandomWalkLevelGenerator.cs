using System.Collections.Generic;
using UnityEngine;

namespace Ayush.ProcGen
{
	public class SimpleRandomWalkLevelGenerator : MonoBehaviour
	{
		[field: SerializeField, AssetPreview]
		public LevelGeneratorParametersScriptableObject LevelGeneratorParameters { get; private set; }

		[field: SerializeField]
		public FloorTileManager FloorTileManager { get; private set; }

		public virtual void GenerateLevel()
		{
			var path = Walk( LevelGeneratorParameters );
			FloorTileManager.SpawnFloorTiles( path, LevelGeneratorParameters.StepSize );
			FloorTileManager.SetWalls();
		}

		private HashSet<Vector3Int> Walk( LevelGeneratorParametersScriptableObject levelGeneratorParameters )
		{
			return ProceduralGeneration.RandomWalk( levelGeneratorParameters.NumberOfWalks, levelGeneratorParameters.StepsToWalk, levelGeneratorParameters.StartPoint, levelGeneratorParameters.StepSize, levelGeneratorParameters.EnsureUniqueStep );
		}
	}
}