using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using System.Collections;
public class ScreenShot : MonoBehaviour
{   
    private static ScreenShot Instance;
    [SerializeField] private Text VersionNum;
    [SerializeField] private Canvas OnScreen;
    private string path;
    public void Start()
    {
        if (Instance != null)
        {
            initializePath();
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
    private void initializePath()
    {
        path = Application.persistentDataPath + "/screenshots/";
        if (Directory.Exists(path)) { Directory.Delete(path, true); }
        Directory.CreateDirectory(path);
    }
    IEnumerator getScreenCapture()
    {
        var version = VersionNum.text.ToString();
        OnScreen.sortingOrder = -1;

        yield return new WaitForEndOfFrame();
        ScreenCapture.CaptureScreenshot(path + version + ".png");
        UnityEditor.AssetDatabase.Refresh();
        OnScreen.sortingOrder = 3;
    }

    public void getScreenShot()
    {
        StartCoroutine(getScreenCapture());
    }
}
