using UnityEditor;
using UnityEngine;

namespace Ayush.ProcGen
{
	[CustomEditor( typeof( SimpleRandomWalkLevelGenerator ), editorForChildClasses: true )]
	public class LevelGeneratorEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var levelGenerator = (SimpleRandomWalkLevelGenerator)target;
			if (GUILayout.Button( "Generate Level" ))
			{
				levelGenerator.GenerateLevel();
			}
		}
	}
}