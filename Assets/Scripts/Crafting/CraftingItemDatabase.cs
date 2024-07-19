using UnityEngine;

namespace WTF_GameJam.Crafting
{
	[CreateAssetMenu( fileName = "CraftingItemDatabase", menuName = "Crafting/CraftingItemDatabase" )]
	public class CraftingItemDatabase : ScriptableObject
	{
		[field: SerializeField]
		public SerializedDictionary<string, CraftingItem> CraftingItems { get; private set; }

#if UNITY_EDITOR
		[SerializeField, EditorButton( nameof( LoadCraftingItems ) )]
		private int _loadCraftingItemsButton;

		private void LoadCraftingItems()
		{
			CraftingItems.Clear();

			var craftingItems = Resources.LoadAll<CraftingItem>( "CraftingItems" );
			foreach (var craftingItem in craftingItems)
			{
				CraftingItems.Add( craftingItem.ItemName, craftingItem );
			}
		}
#endif
	}

	public enum CraftingMaterialType
	{
		Wood,
		Stone,
		Iron,
		Brass,
		Steel,
		Gold,
		Glass,
		Silver,
		Diamond,
		Wheatflour,
		Sugar,
		Egg,
		Meat,
		Rice,
	}
}