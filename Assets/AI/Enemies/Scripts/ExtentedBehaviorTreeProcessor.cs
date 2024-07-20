using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WTF_GameJam.AI
{
	public class ExtentedBehaviorTreeProcessor : BehaviourTreeProcessor
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public List<TargetType> TargetPreferenceOrder { get; private set; }

		private BotSetDestinationNode _botSetDestinationNode;
		private BotAttackNode _botAttackNode;

		private int VelocityXHash = Animator.StringToHash("VelocityX");
		private int VelocityZHash = Animator.StringToHash( "VelocityZ" );

		protected override void Start()
		{
			base.Start();
			_botSetDestinationNode = Tree.nodes.Find(x => x.GetType() == typeof(BotSetDestinationNode)) as BotSetDestinationNode;
			_botAttackNode = Tree.nodes.Find(x => x.GetType() == typeof(BotAttackNode)) as BotAttackNode;

			if (_botSetDestinationNode == null)
			{
				Debug.LogError("BotSetDestinationNode not found");
			}

			if(TargetPreferenceOrder[0] == TargetType.Player)
			{
				_botSetDestinationNode.DestinationTransform = GameObject.FindGameObjectWithTag("Player").transform;
			}
			else
			{
				var npcs = GameObject.FindGameObjectsWithTag("Npc");
				if(npcs != null && npcs.Length > 0)
				{
					var randomIndex = Random.Range(0, npcs.Length);
					_botSetDestinationNode.DestinationTransform = npcs[randomIndex].transform;
				}
				else
				{ 
					_botSetDestinationNode.DestinationTransform = GameObject.FindGameObjectWithTag( "Player" ).transform;
				}
			}

			_botSetDestinationNode.BotTransform = transform;
			_botSetDestinationNode.Agent = NavMeshAgent;
			_botAttackNode.BotAnimator = Animator;
		}

		protected override void Update()
		{
			if(_botAttackNode.IsAttacking == false)
			{
				base.Update();
			}

			if (_botAttackNode.IsAttacking == false && !Mathf.Approximately(NavMeshAgent.velocity.sqrMagnitude, 0f))
			{
				Animator.SetFloat( VelocityZHash, NavMeshAgent.velocity.sqrMagnitude );
			}
			else
			{
				Animator.SetFloat( VelocityZHash, 0 );
			}
		}

		public void AttackEnd()
		{
			_botAttackNode.IsAttacking = false;
			Debug.Log( "AttackEnd" );
		}
	}

	public enum TargetType
	{
		Player,
		Npc
	}
}