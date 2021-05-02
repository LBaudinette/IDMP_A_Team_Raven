using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;

    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryContentPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image itemSprite;
    [SerializeField] private Image itemSpriteContainer;
    public InventoryItem currentItem;

    private void SetDescriptionTextAndSprite(string description)
    {
        descriptionText.text = description;
        Color containerAlpha = itemSpriteContainer.color;
        Color itemAlpha = itemSprite.color;
        containerAlpha.a = 0f;
        itemAlpha.a = 0f;
        itemSpriteContainer.color = containerAlpha;
        this.itemSprite.color = itemAlpha;
    }
    void MakeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.playerInventory.Count; i++)
            {
                if (playerInventory.playerInventory[i].numberHeld > 0)
                {
                    GameObject temporaryInventorySlot = Instantiate(blankInventorySlot, inventoryContentPanel.transform.position, Quaternion.identity, inventoryContentPanel.transform);
                    InventorySlot newSlot = temporaryInventorySlot.GetComponent<InventorySlot>();
                    if (newSlot)
                    {
                        newSlot.Setup(playerInventory.playerInventory[i], this);
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        ClearInventorySlots();
        MakeInventorySlots();
        SetDescriptionTextAndSprite("");
    }

    public void SetupDescriptionAndSprite(string newDescriptionString, Image itemSprite, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
        Color containerAlpha = itemSpriteContainer.color;
        Color itemAlpha = this.itemSprite.color;
        containerAlpha.a = 1f;
        itemAlpha.a = 1f;
        itemSpriteContainer.color = containerAlpha;
        this.itemSprite.color = itemAlpha;
        this.itemSprite.sprite = itemSprite.sprite;
    }

    private void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryContentPanel.transform.childCount; i++)
        {
            Destroy(inventoryContentPanel.transform.GetChild(i).gameObject);
        }
    }
}
