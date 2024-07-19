using UnityEditor;
using UnityEngine;

namespace WTF_GameJam.Crafting
{
	[CreateAssetMenu( fileName = "CraftingItem", menuName = "Crafting/CraftingItem" )]
	public class CraftingItem : ScriptableObject
	{
		[field: SerializeField]
		public string ItemName { get; private set; }

		[field: SerializeField]
		public SerializedDictionary<CraftingMaterialType, int> RequiredMaterialsAndCount { get; private set; }

		[field: SerializeField]
		public int SpaceRequired { get; private set; }

		[field: SerializeField]
		public float CraftingTime { get; private set; }

		[field: SerializeField, AssetPreview]
		public GameObject CraftedItem { get; private set; }

		[field: SerializeField, AssetPreview]
		public Sprite ItemIcon { get; private set; }


#if UNITY_EDITOR
		[SerializeField, EditorButton( nameof( Setup ) )]
		private int _result;

		private void Setup()
		{
			if (string.IsNullOrEmpty( ItemName ))
			{
				return;
			}

			AssetDatabase.RenameAsset( AssetDatabase.GetAssetPath( this ), ItemName + "_Crafting_Item" );
		}
#endif
	}
}