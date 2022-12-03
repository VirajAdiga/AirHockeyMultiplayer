using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadSaveManager : MonoBehaviour
{
    private LoadSaveManager()
    {

    }
    static LoadSaveManager instance;
    public static LoadSaveManager loadSaveManager
    {
        get
        {
            return instance;
        }
    }
    private string dataSavePath;
    
    private void Awake()
    {
        instance = this;
        dataSavePath = Application.persistentDataPath + "/gamedata.gd";
    }

    public void SaveGameData(int wins,int losses)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(dataSavePath, FileMode.Create);

        WinLossData wn = new WinLossData(wins, losses);

        bf.Serialize(stream, wn);
        stream.Close();
    }

    public WinLossData LoadGameData()
    {
        if (File.Exists(dataSavePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(dataSavePath, FileMode.Open);

            WinLossData wn = bf.Deserialize(stream) as WinLossData;
            return wn;
        }
        return new WinLossData(0, 0);
    }
}
