using UnityEngine;

public class WaitNode : LeafNode
{
	public float waitTime = 1f;
	private float startTime;
	protected override void OnStart()
	{
		startTime = Time.time;
	}

	protected override NodeStatus OnUpdate()
	{
		return Time.time - startTime > waitTime ? NodeStatus.Succeeded : NodeStatus.Processing;
	}

	protected override void OnStop()
	{
	}
}
