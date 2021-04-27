using UnityEngine;
using UnityEngine.Events;

//[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public int numberHeld;
    public bool unique;
    public UnityEvent thisEvent;

    public virtual void Use()
    {
        thisEvent.Invoke();
    }

    public void DeacreaseAmount(int amountToDecrease)
    {
        numberHeld -= amountToDecrease;
        if (numberHeld < 0)
        {
            numberHeld = 0;
        }
    }
}
