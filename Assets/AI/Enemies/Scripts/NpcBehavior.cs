using UnityEngine;

namespace WTF_GameJam.AI
{
	public class NpcBehavior : MonoBehaviour
	{
		[SerializeField]
		private NPCBehaviorTreeProcessor _npcBehaviorTreeProcessor;

		[field: SerializeField]
		public int RequiredAttackToKillMe { get; private set; }

		private int DieHash = Animator.StringToHash( "Die" );
		private int HitHash = Animator.StringToHash( "Hit" );

		private void OnTriggerEnter( Collider other )
		{
			if (_npcBehaviorTreeProcessor.IsDead || _npcBehaviorTreeProcessor.IsHit)
				return;

			var enemyBehavior = other.transform.root.GetComponentInChildren<EnemyBehavior>();
			if (enemyBehavior != null)
			{
				RequiredAttackToKillMe--;
				if (RequiredAttackToKillMe <= 0)
				{
					_npcBehaviorTreeProcessor.SetIsDead( true );
					_npcBehaviorTreeProcessor.Animator.SetTrigger( DieHash );
					enemyBehavior.ExtendedBehaviorTreeProcessor.GetPreferedDestination();
				}
				else
				{
					_npcBehaviorTreeProcessor.Animator.SetTrigger( HitHash );
				}
			}
		}
	}
}