using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string CurrentScene;
    public List<InventoryItem> ItemsObtained;

    public GameData()
    {
        ItemsObtained = new List<InventoryItem>();
    }
}
