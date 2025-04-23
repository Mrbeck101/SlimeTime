using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;



public class SceneChanger : MonoBehaviour, IDataPersistence
{

    public static SceneChanger Instance;
    private Scene curScene;
    private bool inPast = false;
    void Start()
    {
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

    void Update()
    {
        //exit revisit past menu
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene() != curScene && !inPast)
        {
            var temp = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(curScene);
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync(temp);
        } 
        //exit the past and go back to current
        else if(Input.GetKeyDown(KeyCode.Escape) && inPast)
        {
            var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
            dpm.LoadCurrent();
            
        }

    }
    //Opens the revisit past menu, and pauses world
    public void RevisitPastMenu()
    {
        Time.timeScale = 0;
        curScene = SceneManager.GetActiveScene();

        var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dpm.CurrentVersionSave();

        StartCoroutine(LoadLevel("RevisitPast", LoadSceneMode.Additive));
        
    }

    //Used to force the game to wait until the next scene is loaded
    public static IEnumerator LoadLevel(string sceneName, LoadSceneMode lsm)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, lsm);
        while (!asyncLoadLevel.isDone)
        {
            //Debug.Log("Loading the Scene");
            yield return null;
        }

        //destroys the player character at end of game
        if (sceneName == "EndGame")
        {
            var slime = GameObject.Find("Slime").GetComponent<PlayerScript>();
            slime.endGame();
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }

    //unpauses world and loads scemeName
    public void LoadData(GameData data)
    {
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {SceneManager.UnloadSceneAsync(1);}
        

        if (data.sceneName != SceneManager.GetActiveScene().name)
        {
            StartCoroutine(LoadLevel(data.sceneName, LoadSceneMode.Single));
        }

        inPast = inPast ? false : true;

        
    }
    
    //Saves game scene name to game data type
    public void SaveData(ref GameData data)
    {
        var curScene = SceneManager.GetActiveScene();
        data.sceneName = curScene.name;
    }



 
}