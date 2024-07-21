using Ayush;
using UnityEngine;

namespace WTF_GameJam.Player
{
	public class PlayerAnimation : MonoBehaviour
	{
		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public PlayerMovement PlayerMovement { get; private set; }

		[field: SerializeField]
		public GameObject AoeVolume { get; private set; }

		[field: SerializeField]
		public AudioClip SwordSFX { get; private set; }

		[field: SerializeField]
		public AudioClip AoeSFX { get; private set; }

		private AudioService _audioService;

		private int VelocityXHash = Animator.StringToHash( "VelocityX" );
		private int VelocityZHash = Animator.StringToHash( "VelocityZ" );
		private int SwingAttackHash = Animator.StringToHash( "SwingAttack" );
		private int AOEAttackHash = Animator.StringToHash( "AOEAttack" );
		private int DashHash = Animator.StringToHash( "Dash" );

		private void Start()
		{
			GameManager.Instance.TryGetService( out _audioService );
		}

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

			if (PlayerMovement.SwingAttackInput)
			{ 
				Animator.SetTrigger( SwingAttackHash );
				PlayerMovement.SetIsAttacking( true );
				if (_audioService != null)
				{
					_audioService.PlaySfx( SwordSFX );
				}
			}
			if (PlayerMovement.AOEAttackInput)
			{
				Animator.SetTrigger( AOEAttackHash );
				PlayerMovement.SetIsAttacking( true );
			}

			Animator.SetBool( DashHash, PlayerMovement.IsDashing );
		}

		public void AttackEnd()
		{
			PlayerMovement.SetIsAttacking( false );
		}

		public void EnableAoeVolume()
		{
			AoeVolume.SetActive( true );
			if(_audioService != null)
			{
				_audioService.PlaySfx( AoeSFX );
			}
		}

		public void DisableAoeVolume()
		{
			AoeVolume.SetActive( false );
		}
	}
}