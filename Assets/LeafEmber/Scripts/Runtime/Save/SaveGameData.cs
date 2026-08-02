using System;
using System.Collections.Generic;

namespace LeafEmber.Save
{

[Serializable]
public sealed class SaveGameData
{
    public const int CurrentSchemaVersion = 1;

    public int schemaVersion = CurrentSchemaVersion;
    public string saveId;
    public string createdUtc;
    public string modifiedUtc;
    public List<SaveSectionData> sections = new();

    public static SaveGameData CreateNew(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("A save identifier is required.", nameof(id));
        }

        string timestamp = DateTime.UtcNow.ToString("O");
        return new SaveGameData
        {
            saveId = id,
            createdUtc = timestamp,
            modifiedUtc = timestamp,
        };
    }

    public void MarkModified()
    {
        modifiedUtc = DateTime.UtcNow.ToString("O");
    }
}

[Serializable]
public sealed class SaveSectionData
{
    public string key;
    public string json;
}
}
