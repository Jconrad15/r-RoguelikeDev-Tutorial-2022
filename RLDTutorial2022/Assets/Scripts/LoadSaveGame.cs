using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSaveGame : MonoBehaviour
{
    public static void Save()
    {
        TileGrid tileGrid = GameManager.Instance.Grid;
        SaveObject saveObject = new SaveObject(tileGrid);

        string path = Application.persistentDataPath + "/save.dat";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(
            path, FileMode.OpenOrCreate);

        bf.Serialize(file, saveObject);
        file.Close();
    }

    public static SaveObject Load()
    {
        string path = Application.persistentDataPath + "/save.dat";
        bool pathExists = File.Exists(path);

        if (pathExists)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                File.Open(path, FileMode.Open);
            SaveObject saveObject =
                (SaveObject)bf.Deserialize(file);
            file.Close();

            return saveObject;
        }
        
        return null;
    }

}
