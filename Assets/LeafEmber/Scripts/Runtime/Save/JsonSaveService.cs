using System;
using System.IO;
using UnityEngine;

namespace LeafEmber.Save
{

public sealed class JsonSaveService : ISaveService
{
    private const string DefaultFileName = "save.json";
    private readonly string savePath;

    public JsonSaveService()
        : this(Path.Combine(Application.persistentDataPath, DefaultFileName))
    {
    }

    public JsonSaveService(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("A save path is required.", nameof(path));
        }

        savePath = Path.GetFullPath(path);
    }

    public bool SaveExists => File.Exists(savePath);

    public SaveGameData Load()
    {
        if (!SaveExists)
        {
            throw new FileNotFoundException("No save file exists.", savePath);
        }

        string json = File.ReadAllText(savePath);
        SaveGameData data = JsonUtility.FromJson<SaveGameData>(json);
        if (data == null || data.schemaVersion <= 0)
        {
            throw new InvalidDataException("The save file is invalid.");
        }

        if (data.schemaVersion > SaveGameData.CurrentSchemaVersion)
        {
            throw new InvalidDataException(
                $"Save schema {data.schemaVersion} is newer than supported schema " +
                $"{SaveGameData.CurrentSchemaVersion}.");
        }

        return data;
    }

    public void Save(SaveGameData data)
    {
        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        string directory = Path.GetDirectoryName(savePath);
        if (string.IsNullOrEmpty(directory))
        {
            throw new InvalidOperationException("The save path has no parent directory.");
        }

        Directory.CreateDirectory(directory);
        data.MarkModified();

        string temporaryPath = savePath + ".tmp";
        File.WriteAllText(temporaryPath, JsonUtility.ToJson(data, true));

        if (File.Exists(savePath))
        {
            File.Replace(temporaryPath, savePath, null);
        }
        else
        {
            File.Move(temporaryPath, savePath);
        }
    }

    public void Delete()
    {
        if (SaveExists)
        {
            File.Delete(savePath);
        }

        string temporaryPath = savePath + ".tmp";
        if (File.Exists(temporaryPath))
        {
            File.Delete(temporaryPath);
        }
    }
}
}
