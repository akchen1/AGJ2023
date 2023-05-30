using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceSystem
{
    public GameData gameData;

    [Header("File Saving Parameters")]
    [SerializeField] private string fileName = "data.game";
    private FileDataHandler dataHandler;

    public bool SaveOnQuit;
    public bool LoadOnStart;

    public DataPersistenceSystem()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }

    public bool HasSaveFile()
    {
        return dataHandler.FileExists();
    }

    public void DeleteSaveFile()
    {
        dataHandler.DeleteSaveFile();
    }

    public void NewGame()
    {
        DeleteSaveFile();
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if (gameData == null)
        {
            NewGame();
        }
    }

    public void SaveGame()
    {
        dataHandler.Save(gameData);
    }
}
