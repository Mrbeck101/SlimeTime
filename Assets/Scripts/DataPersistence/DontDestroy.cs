using UnityEngine;
//This entire class exsists to make components singleton. Didn't discover I could do this until later, so only applied to player script
public class DontDestroy : MonoBehaviour
{
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;

    private void Awake()
    {
        if (persistentObjects[objectIndex] == null)
        {
            persistentObjects[objectIndex] = gameObject;
            DontDestroyOnLoad(gameObject);
        }
        else if (persistentObjects[objectIndex] != gameObject)
        {
            Destroy(gameObject);
        }

    }
}

