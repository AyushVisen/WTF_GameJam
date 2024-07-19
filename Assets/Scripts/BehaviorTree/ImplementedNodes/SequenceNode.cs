public class SequenceNode : CompositeNode
{
	private int childCounter;
	protected override void OnStart()
	{
		childCounter = 0;
	}

	protected override NodeStatus OnUpdate()
	{
		var currentChild = children[childCounter];
		switch (currentChild.Update())
		{
			case NodeStatus.Processing:
				return NodeStatus.Processing;
			case NodeStatus.Succeeded:
				childCounter++;
				break;
			case NodeStatus.Failed:
				return NodeStatus.Failed;
			default:
				return NodeStatus.Failed;
		}

		return (childCounter == children.Count) ? NodeStatus.Succeeded : NodeStatus.Processing;
	}

	protected override void OnStop()
	{

	}
}
