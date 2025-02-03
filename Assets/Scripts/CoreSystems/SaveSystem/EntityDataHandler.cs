using UnityEngine; 

public class EntityDataHandler : DataHandler
{
    public override void ReceiveData(SaveData saveData)
    {
        EntityManager.instance.npcValues = saveData.npcValues;
        StartCoroutine(EntityManager.instance.spawner.SpawnLoadedEntities());
    }

    public override void SendData(SaveData saveData)
    {
        for (int i = 0; i < EntityManager.instance.currentNPCs.Count; i++)
        { 
            EntityManager.instance.npcValues[i].lastLocation = EntityManager.instance.currentNPCs[i].transform.position;
        }
        
        saveData.npcValues = EntityManager.instance.npcValues;
    }
}