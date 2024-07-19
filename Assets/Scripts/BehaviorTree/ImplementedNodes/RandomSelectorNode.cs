using System.Collections.Generic;
using UnityEngine;

public class RandomSelectorNode : CompositeNode
{
	private List<Node> childrenRemaining = new List<Node>();
	private int randomChildIndex;
	protected override void OnStart()
	{
		childrenRemaining.Clear();
		children.ForEach( child => childrenRemaining.Add( child ) );
		randomChildIndex = Random.Range( 0, childrenRemaining.Count );
	}

	protected override NodeStatus OnUpdate()
	{
		var currentChild = childrenRemaining[randomChildIndex];

		switch (currentChild.Update())
		{
			case NodeStatus.Processing:
				return NodeStatus.Processing;
			case NodeStatus.Succeeded:
				return NodeStatus.Succeeded;
			case NodeStatus.Failed:
				childrenRemaining.RemoveAt( randomChildIndex );
				randomChildIndex = Random.Range( 0, childrenRemaining.Count );
				break;
			default:
				return NodeStatus.Failed;
		}

		return (childrenRemaining.Count == 0) ? NodeStatus.Failed : NodeStatus.Processing;
	}

	protected override void OnStop()
	{
	}
}
