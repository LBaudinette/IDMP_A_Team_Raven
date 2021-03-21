using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public FloatValue playerHealth;
    public SignalSender playerHealthSignal;
    public Slider healthSlider;

    //TODO - call in whatever script handles player being hit in combat

    

    // Start is called before the first frame update
    void Start()
    {
        healthSlider.maxValue = playerHealth.initialValue;
        healthSlider.value = playerHealth.initialValue;
    }

    public void AddHealth()
    {
        //magicSlider.value += 1;
        //playerInventory.currentMagic += 1;
        healthSlider.value = playerHealth.runTimeValue;
        if (healthSlider.value > healthSlider.maxValue)
        {
            healthSlider.value = healthSlider.maxValue;
            playerHealth.runTimeValue = playerHealth.initialValue;
        }
    }

    public void DecreaseHealth()
    {
        //magicSlider.value -= 1;
        //playerInventory.currentMagic -= 1;
        healthSlider.value = playerHealth.runTimeValue;
        if (healthSlider.value < 0)
        {
            healthSlider.value = 0;
            playerHealth.runTimeValue = 0;
        }

    }
}
