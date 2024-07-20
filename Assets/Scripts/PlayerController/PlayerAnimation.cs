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
		private int AttackHash = Animator.StringToHash( "Attack" );
		private int DashHash = Animator.StringToHash( "Dash" );

		// Update is called once per frame
		void Update()
		{
			if (PlayerMovement.IsDashing == false && PlayerMovement.IsAttacking == false)
			{
				var blendVelocity = PlayerMovement.Velocity;
				var angle = Vector3.SignedAngle( PlayerMovement.LookDirection, Vector3.forward, Vector3.up );
				blendVelocity = Quaternion.AngleAxis( angle, Vector3.up ) * blendVelocity;
				Animator.SetFloat( VelocityXHash, blendVelocity.x );
				Animator.SetFloat( VelocityZHash, blendVelocity.z );
			}

			if (PlayerMovement.AttackInput && PlayerMovement.IsAttacking == false)
			{
				PlayerMovement.SetIsAttacking( true );
				Animator.SetTrigger( AttackHash );
			}

			Animator.SetBool( DashHash, PlayerMovement.IsDashing );
		}

		public void AttackEnd()
		{
			PlayerMovement.SetIsAttacking( false );
		}
	}
}