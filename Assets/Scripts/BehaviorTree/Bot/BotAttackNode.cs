using System.Linq;
using UnityEngine;

public class BotAttackNode : LeafNode
{
	public Animator BotAnimator { get; set; }
	public bool IsAttacking { get; set; }

	private int AttackHash = Animator.StringToHash( "Attack" );
	private float _attackDuration;
	private float _attackRemaingTime = 0;

	protected override void OnStart()
	{
		var attackClip = BotAnimator.runtimeAnimatorController.animationClips.ToList().Find( x => x.name == "EnemyAttack" );
		_attackDuration = attackClip.length;
	}

	protected override void OnStop()
	{
		
	}

	protected override NodeStatus OnUpdate()
	{
		if(_attackRemaingTime > 0)
		{
			_attackRemaingTime -= Time.deltaTime;
			if(_attackRemaingTime <= 0)
			{
				return NodeStatus.Succeeded;
			}

			return NodeStatus.Processing;
		}

		BotAnimator.SetTrigger( AttackHash );
		IsAttacking = true;
		_attackRemaingTime = _attackDuration;

		return NodeStatus.Processing;
	}
}