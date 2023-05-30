using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceEvents
{
    public class SaveGame
    {

    }

    public class UpdateGameData<T>
    {
        public readonly T Data;

        public UpdateGameData(T data)
        {
            Data = data;
        }
    }
}
