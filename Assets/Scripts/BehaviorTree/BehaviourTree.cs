using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
	public Node rootNode;
	public NodeStatus currentTreeStatus = NodeStatus.Processing;
	public List<Node> nodes = new List<Node>();

	public NodeStatus Update()
	{
		if (currentTreeStatus is NodeStatus.Processing)
		{
			currentTreeStatus = rootNode.Update();
		}

		return currentTreeStatus;
	}

	public Node CreateNode( Type type )
	{
		var node = CreateInstance( type ) as Node;
		node.name = type.Name;
#if UNITY_EDITOR
		node.guid = GUID.Generate().ToString();
		AssetDatabase.AddObjectToAsset( node, this );
		AssetDatabase.SaveAssets();
#endif
		nodes.Add( node );

		return node;
	}

	public void DeleteNode( Node node )
	{
		nodes.Remove( node );
#if UNITY_EDITOR
		AssetDatabase.RemoveObjectFromAsset( node );
		AssetDatabase.SaveAssets();
#endif
	}

	public void AddChild( Node parent, Node child )
	{
		var decoratorNode = parent as DecoratorNode;
		if (decoratorNode)
		{
			decoratorNode.child = child;
		}

		var rootNode = parent as RootNode;
		if (rootNode)
		{
			rootNode.child = child;
		}

		var compositeNode = parent as CompositeNode;
		if (compositeNode)
		{
			compositeNode.children.Add( child );
		}
	}

	public void RemoveChild( Node parent, Node child )
	{
		var decoratorNode = parent as DecoratorNode;
		if (decoratorNode)
		{
			decoratorNode.child = null;
		}

		var rootNode = parent as RootNode;
		if (rootNode)
		{
			rootNode.child = null;
		}

		var compositeNode = parent as CompositeNode;
		if (compositeNode)
		{
			compositeNode.children.Remove( child );
		}
	}

	public List<Node> GetChildren( Node parent )
	{
		var children = new List<Node>();
		var decoratorNode = parent as DecoratorNode;
		if (decoratorNode && decoratorNode.child != null)
		{
			children.Add( decoratorNode.child );
		}

		var rootNode = parent as RootNode;
		if (rootNode && rootNode.child != null)
		{
			children.Add( rootNode.child );
		}

		var compositeNode = parent as CompositeNode;
		if (compositeNode)
		{
			return compositeNode.children;
		}

		return children;
	}

	public BehaviourTree Clone()
	{
		var tree = Instantiate( this );
		tree.rootNode = tree.rootNode.Clone();
		tree.nodes.Clear();
		tree.AddNodesOnClone( tree, tree.rootNode );
		return tree;
	}

	public void AddNodesOnClone( BehaviourTree tree, Node nodeToTraverse )
	{
		tree.nodes.Add( nodeToTraverse );
		var children = GetChildren( nodeToTraverse );
		if (children is not { Count: > 0 })
		{
			return;
		}
		children.ForEach( child => AddNodesOnClone( tree, child ) );
	}
}
