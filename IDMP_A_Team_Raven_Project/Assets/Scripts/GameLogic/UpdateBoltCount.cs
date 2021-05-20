using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateBoltCount : MonoBehaviour
{
    [SerializeField] private Image[] arrowImages;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventoryItem boltInventoryItem;

    private void Start()
    {
        UpdateBoltImages();
    }

    public void UpdateBoltImages()
    {
        // clear arrow images
        for (int i = 0; i < arrowImages.Length; i++)
        {
            arrowImages[i].fillAmount = 0;
        }

        // fill the images with the corresponding amount of bolts 
        int j = 0;
        for (j = 0; j < boltInventoryItem.numberHeld; j++)
        {
            arrowImages[j].fillAmount = 1;
        }

        // add a percentage of a full bolt
        if (j < arrowImages.Length)
        {
            if (playerMovement.percentageOfAFullBolt > 0)
                arrowImages[j].fillAmount = playerMovement.percentageOfAFullBolt;
        }
    }
}
