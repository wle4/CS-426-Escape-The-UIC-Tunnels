using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> inventory = new HashSet<string>();

    public void AddItem(string itemName)
    {
        inventory.Add(itemName);
        Debug.Log($"Picked up: {itemName}");
    }

    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
}
