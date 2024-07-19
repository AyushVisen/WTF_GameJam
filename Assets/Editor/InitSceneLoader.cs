#if UNITY_EDITOR
using WTF_GameJam;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class InitSceneLoader
{
	static InitSceneLoader()
	{
		//EditorApplication.playModeStateChanged += LoadInitScene;
	}

	static void LoadInitScene( PlayModeStateChange state )
	{
		if (state == PlayModeStateChange.ExitingEditMode)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}

		if (state == PlayModeStateChange.EnteredPlayMode)
		{
			SceneManager.LoadScene( Constants.InitScene );
		}
	}
}
#endif