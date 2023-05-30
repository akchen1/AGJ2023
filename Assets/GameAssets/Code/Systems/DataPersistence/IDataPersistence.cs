using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    public void SaveData(ref GameData data);
    public void LoadData(GameData data);
}
