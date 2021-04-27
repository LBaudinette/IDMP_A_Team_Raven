using UnityEngine;

public class HealEffect : MonoBehaviour
{
    public FloatValue playerHealth;
    public SignalSender healthSignal;
    public InventoryItem healthPotion;

    public void Use(int amountToIncrease)
    {
        if (healthPotion.numberHeld > 0)
        {
            playerHealth.runTimeValue += amountToIncrease;
            if (playerHealth.runTimeValue > playerHealth.initialValue)
            {
                playerHealth.runTimeValue = playerHealth.initialValue;
            }
            healthSignal.Raise();
        }
    }
}
