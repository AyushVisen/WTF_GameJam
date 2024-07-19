using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeNode : Node
{
	[HideInInspector]
	public List<Node> children = new List<Node>();

	public override Node Clone()
	{
		var node = Instantiate( this );
		node.children = children.ConvertAll( child => child.Clone() );
		return node;
	}
}
