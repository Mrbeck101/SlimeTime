using UnityEngine;
using UnityEngine.UI;

public class VersionText : MonoBehaviour, IDataPersistence
{
    [SerializeField] public Text VersionNum;
    private int version = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void incrementVersion()
    {
        version++;
        VersionNum.text = "V" + version.ToString();
    }

    public void LoadData(GameData data)
    {
        
        this.version = data.version;
        VersionNum.text = "V" + this.version.ToString();
    }

    public void SaveData(ref GameData data)
    {
        data.version = this.version;
    }
}
