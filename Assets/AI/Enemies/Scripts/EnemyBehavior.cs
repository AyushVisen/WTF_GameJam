using UnityEngine;
using WTF_GameJam.Player;

namespace WTF_GameJam.AI
{
	public class EnemyBehavior : MonoBehaviour
	{
		[SerializeField]
		private ExtendedBehaviorTreeProcessor _extennedBehaviorTreeProcessor;

		public ExtendedBehaviorTreeProcessor ExtendedBehaviorTreeProcessor => _extennedBehaviorTreeProcessor;

		[field: SerializeField]
		public int RequiredAttackToKillMe { get; private set; }

		private int DieHash = Animator.StringToHash("Die");
		private int HitHash = Animator.StringToHash("Hit");

		private void OnTriggerEnter( Collider other )
		{
			if (_extennedBehaviorTreeProcessor.IsDead || _extennedBehaviorTreeProcessor.IsHit)
				return;

			var playerMovement = other.transform.root.GetComponentInChildren<PlayerMovement>();
			if(playerMovement != null )
			{
				if (playerMovement.CurrentAttackType == TypeOfAttack.SwordSwing)
				{ 
					var directionFromPlayer = transform.position - playerMovement.transform.position;
					if(Vector3.Dot(directionFromPlayer, playerMovement.transform.forward) > 0f && directionFromPlayer.sqrMagnitude < playerMovement.SwordSwingDamageRange * playerMovement.SwordSwingDamageRange)
					{
						RequiredAttackToKillMe--;
						if(RequiredAttackToKillMe <= 0)
						{
							_extennedBehaviorTreeProcessor.SetIsDead(true);
							_extennedBehaviorTreeProcessor.Animator.SetTrigger(DieHash);
						}
						else
						{
							_extennedBehaviorTreeProcessor.Animator.SetTrigger(HitHash);
						}
					}
				}
			}
		}		
	}
}