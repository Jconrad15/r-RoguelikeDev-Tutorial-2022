using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LoadSaveGame : MonoBehaviour
{
    public static void Save()
    {
        TileGrid tileGrid = GameManager.Instance.Grid;
        SaveObject saveObject = new SaveObject(tileGrid);

        string path = Path.Combine(
            Application.persistentDataPath, "save.dat");

        string json = JsonConvert.SerializeObject(saveObject, Formatting.Indented);

        File.WriteAllText(path, json);
    }

    public static SaveObject Load()
    {
        string path = Path.Combine(
            Application.persistentDataPath, "save.dat");
        bool pathExists = File.Exists(path);

        if (pathExists)
        {
            string json = File.ReadAllText(path);
            SaveObject saveObject =
                JsonConvert.DeserializeObject<SaveObject>(json);
            return saveObject;
        }
        
        return null;
    }

}
