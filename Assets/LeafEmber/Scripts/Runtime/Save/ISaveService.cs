namespace LeafEmber.Save
{

public interface ISaveService
{
    bool SaveExists { get; }

    SaveGameData Load();

    void Save(SaveGameData data);

    void Delete();
}
}
