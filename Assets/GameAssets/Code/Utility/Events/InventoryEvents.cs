using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEvents
{
	public class AddItem
	{
		public readonly InventoryItem[] Items;
		public AddItem(params InventoryItem[] items)
		{
			Items = items;
		}
	}

	public class RemoveItem
	{
		public readonly InventoryItem[] Items;
		public RemoveItem(params InventoryItem[] items)
		{
			Items = items;
		}
	}

	public class HasItem
	{
		public readonly InventoryItem[] Item;
		public Action<bool> Callback;
		public HasItem(Action<bool> callback, params InventoryItem[] item) 
		{
			Item = item;
			Callback = callback;
		}
	}

	public class SelectItem
	{
		public readonly InventoryItem Item;
		public SelectItem(InventoryItem item)
		{
			Item = item;
		}
	}

	public class DragCombineItem
	{
		public readonly InventoryItem Item1;
		public readonly InventoryItem Item2;
		public DragCombineItem(InventoryItem item1, InventoryItem item2)
		{
			Item1 = item1;
			Item2 = item2;
		}
	}

	public class ToggleInventoryVisibility
	{
		public readonly bool Visible;

		public ToggleInventoryVisibility(bool visible)
		{
			Visible = visible;
		}
	}

	public class ScrollInventory
	{
		public readonly bool Down;

		public ScrollInventory(bool down)
		{
			Down = down;
		}
	}
}
