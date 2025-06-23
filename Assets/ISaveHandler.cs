public interface ISaveHandler
{
    void Save(SaveData data);
    SaveData Load();
}