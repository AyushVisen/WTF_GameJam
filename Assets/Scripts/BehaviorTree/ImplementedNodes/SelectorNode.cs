public class SelectorNode : CompositeNode
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
				return NodeStatus.Succeeded;
			case NodeStatus.Failed:
				childCounter++;
				break;
			default:
				return NodeStatus.Failed;
		}

		return (childCounter == children.Count) ? NodeStatus.Failed : NodeStatus.Processing;
	}

	protected override void OnStop()
	{
	}
}
