using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class PanelController : MonoBehaviour
{
    private IEnumerable<Button> UIButtons;

    //gets each button in Revisit past menu and populates them with images of each version and disables buttons with no version available yet
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

    //loads that version
    void TaskOnClick(string version)
    {
        var dpm = GameObject.Find("DataPersistenceManager").GetComponent<DataPersistenceManager>();
        dpm.LoadPast(version);
    }

    //converts screenshot to sprite image to populate button
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

                return newSprite;
            }
            else
            {


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
