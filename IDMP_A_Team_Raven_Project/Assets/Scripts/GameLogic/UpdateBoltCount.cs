using UnityEngine;
using TMPro;

public class UpdateBoltCount : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem boltInventoryItem;
    [SerializeField] private TextMeshProUGUI boltCountText;

    private void Start()
    {
        boltCountText.text = "Bolts: " + boltInventoryItem.numberHeld.ToString();
    }

    public void UpdateBoltCountText()
    {
        boltCountText.text = "Bolts: " + boltInventoryItem.numberHeld.ToString();
    }
}
