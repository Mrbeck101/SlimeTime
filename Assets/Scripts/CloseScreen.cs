using UnityEditor.PackageManager.UI;
using UnityEngine;

public class CloseScreen : MonoBehaviour
{
    public void CloseGame()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else 
        { 
            Application.Quit();

        }

    }
}
