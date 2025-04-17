using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI Setup")]
    public Transform inventoryPanel; // Panel to parent item icons
    public GameObject inventoryIconPrefab; // Prefab of a RawImage or Image icon

    private HashSet<string> inventoryItems = new HashSet<string>();
    private Dictionary<string, GameObject> itemIcons = new Dictionary<string, GameObject>();

    public void AddItem(string itemName)
{
    if (!inventoryItems.Contains(itemName))
    {
        inventoryItems.Add(itemName);

        // Instantiate the icon prefab
        GameObject iconGO = Instantiate(inventoryIconPrefab, inventoryPanel);
        iconGO.name = itemName;

        RawImage img = iconGO.GetComponent<RawImage>();

        // Try to load a texture OR sprite as fallback
        Texture tex = Resources.Load<Texture>(itemName);
        if (tex == null)
        {
            Sprite sprite = Resources.Load<Sprite>(itemName);
            if (sprite != null)
            {
                tex = sprite.texture;
            }
        }

        if (tex != null)
        {
            img.texture = tex;
            img.enabled = true;
        }
        else
        {
            Debug.LogWarning($"❌ Texture or Sprite not found in Resources for item: {itemName}");
        }

        itemIcons[itemName] = iconGO;
        Debug.Log($"✅ Picked up: {itemName}");
    }
}

    public bool HasItem(string itemName)
    {
        return inventoryItems.Contains(itemName);
    }

    public void UseItem(string itemName)
    {
        if (inventoryItems.Contains(itemName))
        {
            inventoryItems.Remove(itemName);

            if (itemIcons.TryGetValue(itemName, out GameObject icon))
            {
                Destroy(icon);
                itemIcons.Remove(itemName);
            }
        }
    }
}
