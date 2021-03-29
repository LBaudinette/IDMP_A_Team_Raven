﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Information")]
    public PlayerInventory playerInventory;

    [SerializeField] private GameObject blankInventorySlot;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image itemSprite;
    public InventoryItem currentItem;

    public void SetTextAndImage(string description, Image itemSprite)
    {
        descriptionText.text = description;
        this.itemSprite.sprite = itemSprite.sprite;
    }

    void MakeInventorySlots()
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.playerInventory.Count; i++)
            {
                if (playerInventory.playerInventory[i].numberHeld > 0)
                {
                    GameObject temporaryInventorySlot = Instantiate(blankInventorySlot, inventoryPanel.transform.position, Quaternion.identity, inventoryPanel.transform);
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
        SetTextAndImage("", null);
    }

    public void SetupDescriptionAndButton(string newDescriptionString, InventoryItem newItem)
    {
        currentItem = newItem;
        descriptionText.text = newDescriptionString;
    }

    private void ClearInventorySlots()
    {
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            Destroy(inventoryPanel.transform.GetChild(i).gameObject);
        }
    }
}
