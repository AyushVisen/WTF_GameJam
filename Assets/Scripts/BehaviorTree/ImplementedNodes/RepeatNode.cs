public class RepeatNode : DecoratorNode
{
	protected override void OnStart()
	{
	}

	protected override NodeStatus OnUpdate()
	{
		child.Update();
		return NodeStatus.Processing;
	}

	protected override void OnStop()
	{
	}
}
