using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;


    public bool CanUseMagic(int amountToUse)
    {
        if (currentAmmo >= amountToUse)
        {
            return true;
        }
        return false;
    }

    public void UseAmmo(int amountToUse)
    {
        currentAmmo -= amountToUse;
        if (currentAmmo <= 0)
        {
            currentAmmo = 0;
        }
    }

    public void UseAllAmmo()
    {
        currentAmmo = 0;
    }

    public void FillAmmo()
    {
        currentAmmo = maxAmmo;
    }

    public void AddAmmo(int amountToAdd)
    {
        currentAmmo += amountToAdd;
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
    }
}
