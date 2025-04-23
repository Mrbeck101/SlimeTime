using UnityEngine;

[System.Serializable]
public class GameData
{
    public int version;
    public Vector3 playerPosition;
    public string sceneName;
    
    //The information we want saved to the json
    public GameData()
    {
        this.sceneName = "";
        this.version = 1;
        this.playerPosition = Vector3.zero;

    }
}

