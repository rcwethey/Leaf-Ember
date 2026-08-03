using System;
using UnityEngine;

namespace LeafEmber.Save
{

public static class SaveSectionStore
{
    public static void Set<T>(SaveGameData saveGame, string key, T value)
    {
        if (saveGame == null)
        {
            throw new ArgumentNullException(nameof(saveGame));
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("A save section key is required.", nameof(key));
        }

        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        SaveSectionData section = saveGame.sections.Find(candidate => candidate.key == key);
        if (section == null)
        {
            section = new SaveSectionData { key = key };
            saveGame.sections.Add(section);
        }

        section.json = JsonUtility.ToJson(value);
    }

    public static bool TryGet<T>(SaveGameData saveGame, string key, out T value)
        where T : class
    {
        if (saveGame == null)
        {
            throw new ArgumentNullException(nameof(saveGame));
        }

        SaveSectionData section = saveGame.sections.Find(candidate => candidate.key == key);
        if (section == null || string.IsNullOrWhiteSpace(section.json))
        {
            value = null;
            return false;
        }

        value = JsonUtility.FromJson<T>(section.json);
        return value != null;
    }
}
}
