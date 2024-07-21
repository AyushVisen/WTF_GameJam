using UnityEngine;
using WTF_GameJam.Player;

namespace WTF_GameJam.Camera
{
	public class CameraFollow : MonoBehaviour
	{
		[field: SerializeField]
		public Transform Target { get; private set; }

		[field: SerializeField]
		public Vector3 Offset { get; private set; } = Vector3.up * 12;

		private Vector3 _velocity = Vector3.zero;

		private void Start()
		{
			if(Target == null)
			{
				Target = FindFirstObjectByType<PlayerMovement>().transform;
			}
		}

		private void LateUpdate()
		{
			transform.position += Offset;
			transform.LookAt( Target.position );
		}
	}
}