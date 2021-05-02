using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public FloatValue PlayerHealth;
    public SignalSender playerDecreaseHealthSignal;

    public void DecreaseHealthTest()
    {
        PlayerHealth.runTimeValue -= 10;
        playerDecreaseHealthSignal.Raise();
    }
}