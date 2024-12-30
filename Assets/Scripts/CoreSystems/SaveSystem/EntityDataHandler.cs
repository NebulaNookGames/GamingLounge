public class EntityDataHandler : DataHandler
{
    public override void ReceiveData(SaveData saveData)
    {
        EntityManager.instance.npcValues = saveData.npcValues;
        StartCoroutine(EntityManager.instance.spawner.SpawnLoadedEntities());
    }

    public override void SendData(SaveData saveData)
    {
        saveData.npcValues = EntityManager.instance.npcValues;
    }
}