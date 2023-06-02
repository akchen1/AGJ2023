using System;
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
		public readonly InventoryItem Item;
		public RemoveItem(InventoryItem item)
		{
			Item = item;
		}
	}

	public class HasItem
	{
		public readonly InventoryItem Item;
		public Action<bool> Callback;
		public HasItem(InventoryItem item, Action<bool> callback) 
		{
			Item = item;
			Callback = callback;
		}
	}
}
