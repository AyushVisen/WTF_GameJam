using UnityEngine;

public class RootNode : Node
{
	[HideInInspector]
	public Node child;
	protected override void OnStart()
	{
	}

	protected override NodeStatus OnUpdate()
	{
		return child.Update();
	}

	protected override void OnStop()
	{
	}

	public override Node Clone()
	{
		var node = Instantiate( this );
		node.child = child.Clone();
		return node;
	}
}
