using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // ✅ Required for IEnumerator



public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory UI Setup")]
    public Transform inventoryPanel; // Panel to parent item icons
    public GameObject inventoryIconPrefab; // Prefab of a RawImage or Image icon

    private HashSet<string> inventoryItems = new HashSet<string>();
    private Dictionary<string, GameObject> itemIcons = new Dictionary<string, GameObject>();

    public void AddItem(string itemName)
{
    Debug.Log("🎒 PlayerInventory script loaded correctly.");

    if (!inventoryItems.Contains(itemName))
    {
        inventoryItems.Add(itemName);

        // Instantiate the icon prefab
        GameObject iconGO = Instantiate(inventoryIconPrefab, inventoryPanel);
        iconGO.name = itemName;

        RawImage img = iconGO.GetComponent<RawImage>();

        // Try to load a texture OR sprite as fallback
        Texture tex = Resources.Load<Texture>(itemName);
        Debug.Log($"🔍 Resources.Load<Texture>('{itemName}') = {(tex == null ? "null" : tex.name)}");

        if (tex == null)
        {
            Sprite sprite = Resources.Load<Sprite>(itemName);
            Debug.Log($"🔍 Resources.Load<Sprite>('{itemName}') = {(sprite == null ? "null" : sprite.name)}");

            if (sprite != null)
            {
                tex = sprite.texture;
            }
        }

        // Fallback: create a dummy yellow key-like icon if nothing found
        if (tex == null)
        {
            Debug.LogWarning($"❌ Texture or Sprite not found in Resources for item: {itemName}");
            Debug.Log($"🟡 Using fallback dummy key texture for {itemName}");

            Texture2D dummy = new Texture2D(64, 64);
            Color[] pixels = new Color[64 * 64];

            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    bool isKeyHead = x < 20 && y > 22 && y < 42;
                    bool isKeyShaft = x > 18 && x < 26 && y > 20 && y < 44;
                    bool isTooth = (y > 42 && y < 48 && x > 24 && x < 32);
                    pixels[y * 64 + x] = (isKeyHead || isKeyShaft || isTooth) ? Color.yellow : Color.clear;
                }
            }

            dummy.SetPixels(pixels);
            dummy.Apply();
            tex = dummy;
        }

        if (tex != null)
        {
            img.texture = tex;
            img.color = Color.white; // Force visibility
            img.enabled = true;

            // 🔧 Force position/size in case prefab is messed up
            RectTransform rt = iconGO.GetComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0, 1); // Top-left
            rt.pivot = new Vector2(0, 1);
            rt.anchoredPosition = new Vector2(10, -10);
            rt.sizeDelta = new Vector2(100, 100);

            // 🔴 Add red outline for visibility
            Outline outline = iconGO.AddComponent<Outline>();
            outline.effectColor = Color.red;
            outline.effectDistance = new Vector2(2, -2);

            Debug.Log($"📦 Icon for '{itemName}' placed at {rt.anchoredPosition} with size {rt.sizeDelta}");
            Debug.Log($"🧩 img.texture = {img.texture}, img.color = {img.color}, iconGO active = {iconGO.activeInHierarchy}");
        }

        itemIcons[itemName] = iconGO;
        Debug.Log($"✅ Picked up: {itemName}");
        // Show popup message if UI exists
        var popup = GameObject.Find("PopupMessage")?.GetComponent<TextMeshProUGUI>();
        if (popup != null)
        {
            popup.text = $"Picked up: {itemName}";
            popup.enabled = true;
            StartCoroutine(HideAfterDelay(popup, 2f));
        }
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
    private IEnumerator HideAfterDelay(TextMeshProUGUI msg, float delay)
    {
        yield return new WaitForSeconds(delay);
        msg.enabled = false;
    }

}
