public class SwitchSaveHandler : ISaveHandler
{
    private SwitchSaveSystem<SaveData> switchSaveSystem;

    public SwitchSaveHandler(string filename)
    {
        switchSaveSystem = new SwitchSaveSystem<SaveData>(filename);
    }

    public void Save(SaveData data)
    {
        switchSaveSystem.GetData(0).CopyFrom(data);
        switchSaveSystem.Save();
    }

    public SaveData Load()
    {
        switchSaveSystem.Load();
        return switchSaveSystem.GetData(0);
    }

    public void Unmount()
    {
        switchSaveSystem.Unmount();
    }
}