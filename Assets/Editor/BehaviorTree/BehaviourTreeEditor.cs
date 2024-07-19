using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
public class BehaviourTreeEditor : EditorWindow
{
	BehaviourTreeView treeView;
	InspectorView inspectorView;

	[MenuItem( "BehaviourTreeEditor/Editor ..." )]
	public static void OpenWindow()
	{
		//var wnd = GetWindow<BehaviourTreeEditor>();
		//wnd.titleContent = new GUIContent("BehaviourTreeEditor");
		GetWindow<BehaviourTreeEditor>().titleContent = new GUIContent( "BehaviourTreeEditor" );
	}

	public void CreateGUI()
	{
		var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>( "Assets/Editor/BehaviorTree/BehaviourTreeEditor.uxml" );
		visualTree.CloneTree( rootVisualElement );

		var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>( "Assets/Editor/BehaviorTree/BehaviourTreeEditor.uss" );
		rootVisualElement.styleSheets.Add( styleSheet );

		treeView = rootVisualElement.Q<BehaviourTreeView>();
		inspectorView = rootVisualElement.Q<InspectorView>();
		treeView.OnNodeSelected = OnNodeSelectionChanged;

		OnSelectionChange();
	}

	private void OnSelectionChange()
	{
		var tree = Selection.activeObject as BehaviourTree;
		if (tree && AssetDatabase.CanOpenAssetInEditor( tree.GetInstanceID() ))
		{
			treeView.PopulateView( tree );
		}
	}

	private void OnNodeSelectionChanged( NodeView nodeView )
	{
		inspectorView.UpdateSelection( nodeView );
	}
}