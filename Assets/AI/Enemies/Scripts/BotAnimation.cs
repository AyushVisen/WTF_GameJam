using UnityEngine;

namespace WTF_GameJam.AI
{
	public class BotAnimation : MonoBehaviour
	{
		[SerializeField]
		private ExtendedBehaviorTreeProcessor _extentedBehaviorTreeProcessor;

		public void AttackEnd()
		{
			_extentedBehaviorTreeProcessor.AttackEnd();
		}

		public void OnDeath()
		{
			_extentedBehaviorTreeProcessor.OnDeath();
		}

		public void OnHitStart()
		{
			_extentedBehaviorTreeProcessor.SetIsHit(true);
		}

		public void OnHitEnd()
		{
			_extentedBehaviorTreeProcessor.SetIsHit(false);
		}
	}
}