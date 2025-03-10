using UnityEditor;
using UnityEngine.UIElements;

public class InspectorView : VisualElement
{
	private Editor editor;
	public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits>
	{

	}
	public InspectorView()
	{

	}

	public void UpdateSelection( NodeView nodeView )
	{
		Clear();

		UnityEngine.Object.DestroyImmediate( editor );

		editor = Editor.CreateEditor( nodeView.node );
		IMGUIContainer container = new IMGUIContainer( () => editor.OnInspectorGUI() );
		Add( container );
	}
}
