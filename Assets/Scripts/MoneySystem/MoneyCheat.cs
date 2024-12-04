using UnityEngine;

public class MoneyCheat : MonoBehaviour
{
    public void AddMoney()
    {
        MoneyManager.instance.ChangeMoney(1000);
    }
}
