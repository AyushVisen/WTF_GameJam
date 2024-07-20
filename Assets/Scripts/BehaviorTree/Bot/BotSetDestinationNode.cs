using UnityEngine;
using UnityEngine.AI;

public class BotSetDestinationNode : LeafNode
{
	[SerializeField]
	private float _minDistanceForRangeCheck = 1f;

	public NavMeshAgent Agent { get; set; }
	public Transform DestinationTransform { get; set; }
	public Transform BotTransform { get; set; }

	protected override void OnStart()
	{
		Agent.stoppingDistance = _minDistanceForRangeCheck;
	}

	protected override NodeStatus OnUpdate()
	{
		if (Agent == null)
		{
			Debug.Log( $"Agent Null" );
			return NodeStatus.Failed;
		}

		if ((DestinationTransform.position - BotTransform.position).sqrMagnitude <= _minDistanceForRangeCheck * _minDistanceForRangeCheck)
		{
			if (Agent.enabled == true)
			{
				Agent.enabled = false;
			}

			BotTransform.LookAt( DestinationTransform );

			return NodeStatus.Succeeded;
		}

		if (Agent.enabled == false)
		{
			Agent.enabled = true;
		}

		//if (Agent.hasPath)
		//{
		//	return NodeStatus.Succeeded;
		//}

		var destination = DestinationTransform.position;
		if(!Physics.Raycast( destination, Vector3.down, out var hit, 1000 ))
		{ 
			Debug.Log( $"Destination not found" );
			return NodeStatus.Failed;
		}

		BotTransform.LookAt( destination );
		Agent.SetDestination( hit.point );
		return NodeStatus.Processing;
	}

	protected override void OnStop()
	{
	}
}
