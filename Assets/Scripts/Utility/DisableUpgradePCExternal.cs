using UnityEngine;

public class DisableUpgradePCExternal : MonoBehaviour
{
   public void DisableUpgradePC()
   {
      GameObject.FindWithTag("UpgradePC").GetComponent<UpgradePCHandler>().ChangeActiveState();
   }
}   

