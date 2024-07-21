using UnityEngine;

namespace WTF_GameJam.AI
{
	public class NpcAnimation : MonoBehaviour
	{
		[SerializeField]
		private NPCBehaviorTreeProcessor _npcBehaviorTreeProcessor;

		public void OnDeath()
		{
			_npcBehaviorTreeProcessor.OnDeath();
		}

		public void OnHitStart()
		{
			_npcBehaviorTreeProcessor.SetIsHit( true );
		}

		public void OnHitEnd()
		{
			_npcBehaviorTreeProcessor.SetIsHit( false );
		}
	}
}