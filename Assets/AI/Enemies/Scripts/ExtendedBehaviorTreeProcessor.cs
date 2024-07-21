using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace WTF_GameJam.AI
{
	public class ExtendedBehaviorTreeProcessor : BehaviourTreeProcessor
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		[field: SerializeField]
		public List<TargetType> TargetPreferenceOrder { get; private set; }

		[field: SerializeField]
		public GameObject BotAttackVFX { get; private set; }

		public bool IsDead { get; private set; }
		public bool IsHit { get; private set; }
		private BotSetDestinationNode _botSetDestinationNode;
		private BotAttackNode _botAttackNode;

		private int VelocityZHash = Animator.StringToHash( "VelocityZ" );

		protected override void Start()
		{
			base.Start();
			_botSetDestinationNode = Tree.nodes.Find(x => x.GetType() == typeof(BotSetDestinationNode)) as BotSetDestinationNode;
			_botAttackNode = Tree.nodes.Find(x => x.GetType() == typeof(BotAttackNode)) as BotAttackNode;
			GetPreferedDestination();
			_botSetDestinationNode.Agent = NavMeshAgent;
			_botAttackNode.BotAnimator = Animator;
		}

		protected override void Update()
		{
			if(IsDead || IsHit)
			{
				return;
			}

			base.Update();

			if (_botAttackNode.IsAttacking == false && !Mathf.Approximately(NavMeshAgent.velocity.sqrMagnitude, 0f))
			{
				Animator.SetFloat( VelocityZHash, NavMeshAgent.velocity.sqrMagnitude );
			}
			else
			{
				Animator.SetFloat( VelocityZHash, 0 );
			}

			if(BotAttackVFX != null)
			{
				if(_botAttackNode.IsAttacking && BotAttackVFX.activeSelf == false)
				{
					BotAttackVFX.SetActive(true);
				}
				else if(_botAttackNode.IsAttacking == false && BotAttackVFX.activeSelf == true)
				{
					BotAttackVFX.SetActive(false);
				}
			}
		}

		public void GetPreferedDestination()
		{
			if (_botSetDestinationNode == null)
			{
				Debug.LogError( "BotSetDestinationNode not found" );
			}

			if (TargetPreferenceOrder[0] == TargetType.Player)
			{
				_botSetDestinationNode.DestinationTransform = GameObject.FindGameObjectWithTag( "Player" ).transform;
			}
			else
			{
				var npcs = GameObject.FindGameObjectsWithTag( "Npc" ).ToList();

				var deadNpcs = npcs.FindAll( x => x.GetComponent<NPCBehaviorTreeProcessor>().IsDead );
				foreach (var deadNpc in deadNpcs)
				{
					npcs.Remove( deadNpc );
				}

				if (npcs != null && npcs.Count > 0)
				{
					var randomIndex = Random.Range( 0, npcs.Count );
					_botSetDestinationNode.DestinationTransform = npcs[randomIndex].transform;
				}
				else
				{
					_botSetDestinationNode.DestinationTransform = GameObject.FindGameObjectWithTag( "Player" ).transform;
				}
			}

			_botSetDestinationNode.BotTransform = transform;
		}

		public void SetIsDead(bool value)
		{
			IsDead = value;
		}

		public void AttackEnd()
		{
			_botAttackNode.IsAttacking = false;
		}

		public void OnDeath()
		{
			_botAttackNode.IsDead = true;
			Animator.enabled = false;
			Destroy( gameObject );
		}

		public void SetIsHit(bool value)
		{
			IsHit = value;
			_botAttackNode.UnderAttack = IsHit;
		}
	}

	public enum TargetType
	{
		Player,
		Npc
	}
}