using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public List<InventoryItem> playerInventory = new List<InventoryItem>();
    [SerializeField] private InventoryItem healthPotion;
    [SerializeField] private InventoryItem bolt;
    [SerializeField] private int initialHealthPotion;

    // TODO DEBUG should decide how number of health potions is kept persistent between areas/ playthroughs

    public void OnEnable()
    {
        RefillInventory();
    }

    public void RefillInventory()
    {
        for (int i = 0; i < initialHealthPotion; i++)
        {
            if (healthPotion.numberHeld < healthPotion.maximumItemCount)
            {
                AddItem(healthPotion);
            }
            if (bolt.numberHeld < bolt.maximumItemCount)
            {
                AddItem(bolt);
            }
        }
    }

    public void AddItem(InventoryItem itemToAdd)
    {
        if (playerInventory.Contains(itemToAdd))
        {
            if (itemToAdd.numberHeld < itemToAdd.maximumItemCount)
            {
                itemToAdd.numberHeld += 1;
            }
        }
        else
        {
            playerInventory.Add(itemToAdd);
            itemToAdd.numberHeld += 1;
        }
    }
}

