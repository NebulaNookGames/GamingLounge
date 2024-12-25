using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleCostChecking : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO dataBase;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private int[] indexes;
    [SerializeField] private Color validColor = Color.green;
    [SerializeField] private Color invalidColor = Color.red;
    [SerializeField] private ObjectPlacer objectPlacer;
    
    private void OnEnable()
    {
        MoneyManager.instance.OnMoneyChanged += UpdateButtonInteractable;
        UpdateButtonInteractable(MoneyManager.instance.MoneyAmount);

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = dataBase.objectsData[indexes[i]].Name + "\n" + dataBase.objectsData[indexes[i]].cost.ToString();
        }
    }

    private void OnDisable()
    {
        MoneyManager.instance.OnMoneyChanged -= UpdateButtonInteractable;
    }

    void UpdateButtonInteractable(int amount)
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            if (dataBase.objectsData[indexes[i]].cost <= amount)
            {
                buttons[i].GetComponent<Button>().interactable = true;
                buttons[i].GetComponent<Button>().image.color = validColor;

            }

            else
            {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponent<Button>().image.color = invalidColor;
            }
            
            if (i == 8 && objectPlacer.upgradePCIsPlaced)
            {
                buttons[i].GetComponent<Button>().interactable = false;
                buttons[i].GetComponent<Button>().image.color = invalidColor;
            }
        }
    }
}