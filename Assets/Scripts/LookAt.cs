using UnityEngine;

namespace WTF_GameJam
{
	public class LookAt : MonoBehaviour
	{
		[field: SerializeField]
		public string TargetTag { get; private set; }

		[field: SerializeField]
		public bool OnlyYaw { get; private set; }

		private Transform Target;

		private void Start()
		{
			Target = GameObject.FindGameObjectWithTag( TargetTag ).transform;
		}

		private void LateUpdate()
		{
			var lookAt = Target.position;

			if (OnlyYaw)
			{
				lookAt.y = transform.position.y;
			}

			transform.LookAt( Target.position );
		}
	}
}