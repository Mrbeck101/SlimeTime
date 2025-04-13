using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class DataPersistenceManager : MonoBehaviour
{

    public static DataPersistenceManager Instance;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private string path;
    [SerializeField] private Text VersionNum;

    private void Start()
    {
        if(DataPersistenceManager.Instance != null)
        {
            initializeDataManager();
        }
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void initializeDataManager()
    {
        if(DataPersistenceManager.Instance != null)
        {
            Instance.path = Application.persistentDataPath + "/versions/";


            if (Directory.Exists(Instance.path)) { Directory.Delete(Instance.path, true); }
            Directory.CreateDirectory(Instance.path);
            gameData = new GameData();
            Instance.dataHandler = new FileDataHandler(Instance.path, "Start");
            Instance.dataPersistenceObjects = FindAllDataPersistenceObjects();
        }
    }
    public void LoadPast(string version)
    {
        Instance.gameData = Instance.dataHandler.Load(version);

        foreach (IDataPersistence dataPersistenceObj in Instance.dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(Instance.gameData);
        }
    }

    public void LoadCurrent()
    {
        Instance.gameData = Instance.dataHandler.Load("CurrentTime");

        foreach (IDataPersistence dataPersistenceObj in Instance.dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(Instance.gameData);
        }
    }

    public void NewVersion()
    {
        foreach (IDataPersistence dataPersistenceObj in Instance.dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref Instance.gameData);
        }
        
        Instance.dataHandler.Save(Instance.gameData, VersionNum.text.ToString());
    }

    public void CurrentVersionSave()
    {
        foreach (IDataPersistence dataPersistenceObj in Instance.dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(ref Instance.gameData);
        }

        Instance.dataHandler.Save(Instance.gameData, "CurrentTime");
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>();

        if (!dataPersistenceObjects.Any())
        {
            Debug.LogError("Failed to fetch data");
        }

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
