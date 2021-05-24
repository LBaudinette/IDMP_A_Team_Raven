using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public NecromancerScript bossScript;
    public Slider healthSlider;
    public GameObject bossHealthHolder;

    // Start is called before the first frame update
    void OnEnable()
    {
        healthSlider.maxValue = bossScript.health;
        healthSlider.value = bossScript.health;
    }

    public void DecreaseHealth()
    {
        healthSlider.value = bossScript.health;
        if (healthSlider.value < 0)
        {
            healthSlider.value = 0;
            bossHealthHolder.SetActive(false);
        }
    }
}
