using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class PanelController : MonoBehaviour
{
    private IEnumerable<Button> UIButtons;
    void Start()
    {
        UIButtons = GetComponentsInChildren<Button>();
        
        foreach (Button button in UIButtons)
        {
            if (File.Exists(Application.persistentDataPath + "/screenshots/" + button.name + ".png"))
            {
                button.image.sprite = CreateSprite(Application.persistentDataPath + "/screenshots/" + button.name + ".png");
                button.enabled = true;
                button.onClick.AddListener(() => TaskOnClick(button.name));
            }
            else 
            { 
                button.image.color = Color.clear;
                button.enabled = false; 
            } 
        }
    }

    void TaskOnClick(string version)
    {
        var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dpm.LoadPast(version);
    }
    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
        //    Time.timeScale = 1;
        //    SceneManager.UnloadSceneAsync(1);
        //}
    }

    private Sprite CreateSprite(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(909, 544);
            if (texture.LoadImage(fileData))
            {
                Sprite newSprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    100.0f
                );

                //newSprite.texture.filterMode = FilterMode.Bilinear;
                return newSprite;
            }
            else
            {

                //Debug.LogError("Failed to load image from: " + filePath);
                return null;
            }
        }
        else
        {

            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}
