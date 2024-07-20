using UnityEngine;

namespace WTF_GameJam.AI
{
	public class BotAnimation : MonoBehaviour
	{
		[SerializeField]
		private ExtentedBehaviorTreeProcessor _extentedBehaviorTreeProcessor;

		public void AttackEnd()
		{
			_extentedBehaviorTreeProcessor.AttackEnd();
		}
	}
}