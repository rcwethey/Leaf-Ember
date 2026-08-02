using System;
using System.IO;
using LeafEmber.Save;
using NUnit.Framework;
using UnityEngine;

namespace LeafEmber.Tests
{

public sealed class JsonSaveServiceTests
{
    private string savePath;
    private JsonSaveService saveService;

    [SetUp]
    public void SetUp()
    {
        savePath = Path.Combine(
            Application.temporaryCachePath,
            $"leaf-ember-test-{Guid.NewGuid():N}",
            "save.json");
        saveService = new JsonSaveService(savePath);
    }

    [TearDown]
    public void TearDown()
    {
        saveService.Delete();

        string directory = Path.GetDirectoryName(savePath);
        if (!string.IsNullOrEmpty(directory) && Directory.Exists(directory))
        {
            Directory.Delete(directory, true);
        }
    }

    [Test]
    public void Save_ThenLoad_RoundTripsData()
    {
        SaveGameData expected = SaveGameData.CreateNew("test-save");
        expected.sections.Add(new SaveSectionData
        {
            key = "farm",
            json = "farm-state",
        });

        saveService.Save(expected);
        SaveGameData actual = saveService.Load();

        Assert.That(actual.saveId, Is.EqualTo(expected.saveId));
        Assert.That(actual.schemaVersion, Is.EqualTo(SaveGameData.CurrentSchemaVersion));
        Assert.That(actual.sections, Has.Count.EqualTo(1));
        Assert.That(actual.sections[0].key, Is.EqualTo("farm"));
    }
}
}
