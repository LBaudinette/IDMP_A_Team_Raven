using UnityEngine;
using TMPro;

public class UpdateHealthPotion : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem healthPotion;
    [SerializeField] private TextMeshProUGUI healthPotionCountText;

    private void Start()
    {
        healthPotionCountText.text = healthPotion.numberHeld.ToString();
    }

    public void UpdatehealthPotionCountText()
    {
        healthPotionCountText.text = healthPotion.numberHeld.ToString();
    }
}
