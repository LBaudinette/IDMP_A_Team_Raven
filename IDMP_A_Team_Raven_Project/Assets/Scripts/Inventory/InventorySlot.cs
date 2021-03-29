using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [Header("UI variables to change")]
    [SerializeField] private TextMeshProUGUI itemNumberText;
    [SerializeField] private Image itemImage;

    [Header("Variables from the item")]
    public InventoryItem currentItem;
    public InventoryManager inventoryManager;

    public void Setup(InventoryItem newItem, InventoryManager newMananger)
    {
        currentItem = newItem;
        inventoryManager = newMananger;
        if (currentItem)
        {
            itemImage.sprite = currentItem.itemImage;
            itemNumberText.text = currentItem.numberHeld.ToString();
        }
    }

    public void ClickedOn()
    {
        if (currentItem)
        {
            inventoryManager.SetupDescriptionAndButton(currentItem.itemDescription, currentItem);
        }
    }
}
