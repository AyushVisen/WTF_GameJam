using UnityEngine;

public class DebugNode : LeafNode
{
	public string message;
	protected override void OnStart()
	{
	}

	protected override NodeStatus OnUpdate()
	{
		Debug.Log( $"Message: {message}" );
		return NodeStatus.Succeeded;
	}

	protected override void OnStop()
	{
	}
}
