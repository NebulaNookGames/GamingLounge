using System.Collections.Generic;
using UnityEngine; 

public class MoneyDataHandler : DataHandler
{
    MoneyManager moneyManager;
    private int savedMoney;
    
    private void Start()
    {
        moneyManager = MoneyManager.instance;
    }

    public override void ReceiveData(SaveData saveData)
    {
        moneyManager.SetMoney(saveData.savedMoney);
        
        WorldInteractables worldinteractables = WorldInteractables.instance;
        for (int i = 0; i < saveData.arcadeMachinePositions.Count; i++)
        {
            if (worldinteractables.allAracadeMachines[i] != null &&
                worldinteractables.allAracadeMachines[i].transform.position == saveData.arcadeMachinePositions[i])
            {
                worldinteractables.allAracadeMachines[i].GetComponent<MoneyHolder>().ChangeMoney(saveData.moneyInArcadeMachines[i]);
            }
        }
    }

    public override void SendData(SaveData saveData)
    {
        saveData.savedMoney = MoneyManager.instance.MoneyAmount;

        saveData.moneyInArcadeMachines = new List<int>();
        saveData.arcadeMachinePositions = new List<Vector3>();
        
        foreach (GameObject go in WorldInteractables.instance.allAracadeMachines)
        {
            saveData.moneyInArcadeMachines.Add(go.GetComponent<MoneyHolder>().moneyBeingHeld);
            saveData.arcadeMachinePositions.Add(go.transform.position);
        }
    }
}