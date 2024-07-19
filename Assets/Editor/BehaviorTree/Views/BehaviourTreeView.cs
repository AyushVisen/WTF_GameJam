using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class BehaviourTreeView : GraphView
{
	public Action<NodeView> OnNodeSelected;
	private BehaviourTree tree;
	public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits>
	{
	}
	public BehaviourTreeView()
	{
		Insert( 0, new GridBackground() );

		this.AddManipulator( new ContentZoomer() );
		this.AddManipulator( new ContentDragger() );
		this.AddManipulator( new SelectionDragger() );
		this.AddManipulator( new RectangleSelector() );

		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>( "Assets/Editor/BehaviorTree/BehaviourTreeEditor.uss" );
		styleSheets.Add( styleSheet );
	}

	private NodeView FindNodeView( Node node )
	{
		return GetNodeByGuid( node.guid ) as NodeView;
	}
	public void PopulateView( BehaviourTree behaviourTree )
	{
		this.tree = behaviourTree;

		graphViewChanged -= OnGraphViewChanged;
		DeleteElements( graphElements );
		graphViewChanged += OnGraphViewChanged;

		if (tree.rootNode == null)
		{
			tree.rootNode = tree.CreateNode( typeof( RootNode ) ) as RootNode;
			EditorUtility.SetDirty( tree );
			AssetDatabase.SaveAssets();
		}

		tree.nodes.ForEach( CreateNodeView );

		tree.nodes.ForEach( node =>
		{
			var children = tree.GetChildren( node );
			var parentView = FindNodeView( node );
			children.ForEach( child =>
			{
				var childView = FindNodeView( child );
				var edge = parentView.output.ConnectTo( childView.input );
				AddElement( edge );
			} );
		} );
	}

	private GraphViewChange OnGraphViewChanged( GraphViewChange graphviewchange )
	{
		if (graphviewchange.elementsToRemove != null)
		{
			graphviewchange.elementsToRemove.ForEach( element =>
			{
				if (element is NodeView nodeView)
				{
					tree.DeleteNode( nodeView.node );
				}

				if (element is Edge edge)
				{
					var parentView = edge.output.node as NodeView;
					var childView = edge.input.node as NodeView;
					tree.RemoveChild( parentView.node, childView.node );
				}
			} );
		}

		if (graphviewchange.edgesToCreate != null)
		{
			graphviewchange.edgesToCreate.ForEach( edge =>
			{
				var parentView = edge.output.node as NodeView;
				var childView = edge.input.node as NodeView;
				tree.AddChild( parentView.node, childView.node );
			} );
		}

		return graphviewchange;
	}

	private void CreateNodeView( Node node )
	{
		var nodeView = new NodeView( node );
		nodeView.OnNodeSelected = OnNodeSelected;
		AddElement( nodeView );
	}

	public override void BuildContextualMenu( ContextualMenuPopulateEvent evt )
	{
		var leafNodes = TypeCache.GetTypesDerivedFrom<LeafNode>();
		foreach (var leafNode in leafNodes)
		{
			evt.menu.AppendAction( $"[{leafNode.BaseType.Name}]{leafNode.Name}", a => CreateNode( leafNode ) );
		}

		var compositeNodes = TypeCache.GetTypesDerivedFrom<CompositeNode>();
		foreach (var compositeNode in compositeNodes)
		{
			evt.menu.AppendAction( $"[{compositeNode.BaseType.Name}]{compositeNode.Name}", a => CreateNode( compositeNode ) );
		}

		var decoratorNodes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
		foreach (var decoratorNode in decoratorNodes)
		{
			evt.menu.AppendAction( $"[{decoratorNode.BaseType.Name}]{decoratorNode.Name}", a => CreateNode( decoratorNode ) );
		}
	}

	private void CreateNode( Type type )
	{
		var node = tree.CreateNode( type );
		CreateNodeView( node );
	}

	public override List<Port> GetCompatiblePorts( Port startPort, NodeAdapter nodeAdapter )
	{
		var compatiblePorts = new List<Port>();
		ports.ForEach( port =>
		{
			if (port.direction != startPort.direction && port.node != startPort.node)
			{
				compatiblePorts.Add( port );
			}
		} );
		return compatiblePorts;
	}
}
