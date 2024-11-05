using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public GameObject FindItemWithTag(string tag)
    {
        foreach (GameObject item in items)
        {
            if (item.tag == tag)
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject itemToRemove)
    {     
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == itemToRemove)
            {
                items.RemoveAt(i);
                break; // Exit the loop after removing the item
            }
        }       
    }
}
