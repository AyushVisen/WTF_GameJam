using UnityEngine;

namespace Ayush.Singleton
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance { get; private set; }

		private void Awake()
		{
			if (Instance != null)
			{
				Destroy( gameObject );
				return;
			}

			Instance = this as T;
			DontDestroyOnLoad( gameObject );
		}
	}
}