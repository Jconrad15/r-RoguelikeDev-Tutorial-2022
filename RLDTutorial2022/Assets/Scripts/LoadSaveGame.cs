using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class LoadSaveGame : MonoBehaviour
{
    public static void Save()
    {
        SaveObject saveObject = new SaveObject(
            GameManager.Instance.CurrentGrid,
            GameManager.Instance.CurrentGridSeed,
            GameManager.Instance.CurrentDungeon.dungeonGridSeeds);

        string path = Path.Combine(
            Application.persistentDataPath, "save.json");

        JsonSerializerSettings settings =
            new JsonSerializerSettings
            { TypeNameHandling = TypeNameHandling.All };

        string json = JsonConvert.SerializeObject(
            saveObject, Formatting.Indented, settings);

        File.WriteAllText(path, json);
    }

    public static SaveObject Load()
    {
        string path = Path.Combine(
            Application.persistentDataPath, "save.json");
        bool pathExists = File.Exists(path);

        if (pathExists)
        {
            string json = File.ReadAllText(path);

            JsonSerializerSettings settings =
                new JsonSerializerSettings
                { TypeNameHandling = TypeNameHandling.All };

            SaveObject saveObject =
                JsonConvert.DeserializeObject<SaveObject>(json, settings);
            return saveObject;
        }
        
        return null;
    }

}
