using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;


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
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene() != curScene && !inPast)
        {
            var temp = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(curScene);
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync(temp);
        } 
        else if(Input.GetKeyDown(KeyCode.Escape) && inPast)
        {
            var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
            dpm.LoadCurrent();
            
        }

    }
    public void RevisitPastMenu()
    {
        Time.timeScale = 0;
        curScene = SceneManager.GetActiveScene();

        var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dpm.CurrentVersionSave();

        StartCoroutine(LoadLevel("RevisitPast", LoadSceneMode.Additive));
        
    }

    public static IEnumerator LoadLevel(string sceneName, LoadSceneMode lsm)
    {
        var asyncLoadLevel = SceneManager.LoadSceneAsync(sceneName, lsm);
        while (!asyncLoadLevel.isDone)
        {
            //Debug.Log("Loading the Scene");
            yield return null;
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
    }
    public void LoadData(GameData data)
    {
        Time.timeScale = 1;
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1)) {SceneManager.UnloadSceneAsync(1);}
        

        if (data.sceneName != SceneManager.GetActiveScene().name)
        {
            StartCoroutine(LoadLevel(data.sceneName, LoadSceneMode.Single));
        }

        inPast = inPast ? false : true;

        //var invisbleWall = GameObject.Find("InvisibleWall");
        //IEnumerable<Collider2D> walls = invisbleWall.GetComponentsInChildren<Collider2D>();
        //Rigidbody2D wallBody = invisbleWall.GetComponent<Rigidbody2D>();

        //foreach(Collider2D wall in walls)
        //{
        //    wall.enabled = inPast ? true: false;
            
        //}
        //wallBody.bodyType = inPast ? RigidbodyType2D.Static : RigidbodyType2D.Kinematic;
        
    }
    public void SaveData(ref GameData data)
    {
        var curScene = SceneManager.GetActiveScene();
        data.sceneName = curScene.name;
    }

 
}