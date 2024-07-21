using UnityEngine;
using UnityEngine.AI;

namespace WTF_GameJam.AI
{
	public class NPCBehaviorTreeProcessor : BehaviourTreeProcessor
	{
		[field: SerializeField]
		public NavMeshAgent NavMeshAgent { get; private set; }

		[field: SerializeField]
		public Animator Animator { get; private set; }

		public bool IsDead { get; private set; }
		public bool IsHit { get; private set; }
		private BotSetDestinationNode _botSetDestinationNode;

		private int VelocityZHash = Animator.StringToHash( "VelocityZ" );

		protected override void Start()
		{
			base.Start();
			_botSetDestinationNode = Tree.nodes.Find( x => x.GetType() == typeof( BotSetDestinationNode ) ) as BotSetDestinationNode;

			if (_botSetDestinationNode == null)
			{
				Debug.LogError( "BotSetDestinationNode not found" );
			}

			_botSetDestinationNode.DestinationTransform = GameObject.FindGameObjectWithTag( "Player" ).transform;

			_botSetDestinationNode.BotTransform = transform;
			_botSetDestinationNode.Agent = NavMeshAgent;
		}

		protected override void Update()
		{
			if (IsDead || IsHit)
			{
				return;
			}

			base.Update();

			if (!Mathf.Approximately( NavMeshAgent.velocity.sqrMagnitude, 0f ))
			{
				Animator.SetFloat( VelocityZHash, NavMeshAgent.velocity.sqrMagnitude );
			}
			else
			{
				Animator.SetFloat( VelocityZHash, 0 );
			}
		}

		public void SetIsDead( bool value )
		{
			IsDead = value;
		}

		public void OnDeath()
		{
			Animator.enabled = false;
			Destroy( gameObject );
		}

		public void SetIsHit( bool value )
		{
			IsHit = value;
		}

	}
}