using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents
{
	public class AddItem
	{
		public readonly InventoryItem Item;
		public AddItem(InventoryItem item)
		{
			Item = item;
		}
	}

	public class RemoveItem
	{
		public RemoveItem()
		{
			// TODO: Implement
		}
	}
}
