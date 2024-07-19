using UnityEngine;

namespace WTF_GameJam.Player
{
	public class PlayerAnimation : MonoBehaviour
	{
		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public PlayerMovement PlayerMovement { get; private set; }

		private int VelocityXHash = Animator.StringToHash( "VelocityX" );
		private int VelocityZHash = Animator.StringToHash( "VelocityZ" );

		// Update is called once per frame
		void Update()
		{
			var blendVelocity = PlayerMovement.Velocity;
			var angle = Vector3.SignedAngle( PlayerMovement.LookDirection, Vector3.forward, Vector3.up );
			blendVelocity = Quaternion.AngleAxis( angle, Vector3.up ) * blendVelocity;
			Animator.SetFloat( VelocityXHash, blendVelocity.x );
			Animator.SetFloat( VelocityZHash, blendVelocity.z );
		}
	}
}