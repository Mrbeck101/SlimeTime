using UnityEngine;
//this interface is used in other scripts where the data must be saved to load back later
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);

}

