using UnityEngine;

namespace Ayush.ProcGen
{
	[CreateAssetMenu( fileName = "LevelGeneratorParametersScriptableObject", menuName = "ProcGen/LevelGeneratorParametersAsset" )]
	public class LevelGeneratorParametersScriptableObject : ScriptableObject
	{
		[field: SerializeField, Range( 0, 2000 )]
		public int StepsToWalk { get; set; }

		[field: SerializeField, Range( 0, 2000 )]
		public int NumberOfWalks { get; private set; }

		[field: SerializeField]
		public Vector3Int StartPoint { get; private set; }

		[field: SerializeField, Range( 1, 100 )]
		public int StepSize { get; private set; }

		[field: SerializeField]
		public bool EnsureUniqueStep { get; private set; }
	}
}