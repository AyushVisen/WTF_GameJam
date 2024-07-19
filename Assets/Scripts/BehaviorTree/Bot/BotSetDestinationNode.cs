using UnityEngine;
using UnityEngine.AI;

public class BotSetDestinationNode : LeafNode
{
	public NavMeshAgent Agent { get; set; }
	public Terrain Ground { get; set; }
	protected override void OnStart()
	{
	}

	protected override NodeStatus OnUpdate()
	{
		if (Agent == null || Ground == null)
		{
			Debug.Log( $"Agent Null: {Agent == null}, Ground Null: {Ground == null}" );
			return NodeStatus.Failed;
		}
		if (Agent.hasPath)
		{
			return NodeStatus.Succeeded;
		}

		var x = Random.Range( 0, Ground.terrainData.size.x );
		var z = Random.Range( 0, Ground.terrainData.size.z );
		var y = Ground.SampleHeight( new Vector3( x, 0, z ) );
		Agent.SetDestination( new Vector3( x, y, z ) );
		return NodeStatus.Succeeded;
	}

	protected override void OnStop()
	{
	}
}
