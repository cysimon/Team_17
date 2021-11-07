using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int unitId = 0;
    public short characterID;

    public int maxWeaponHealth;
    public int currentWeaponHealth;
    public int unitDamage;

    public int maxArmor;
    public int currentArmor;

    public bool TakeDamageArmor(int dmg)
    {
        currentArmor -= dmg;

        if (currentArmor <= 0) return true;
        else return false;
    }

    public void RestoreArmor(int heal)
    {
        currentArmor += heal;
        if (currentArmor >= maxArmor) currentArmor = maxArmor;
    }

    public bool TakeDamageWeapon(int dmg)
    {
        currentWeaponHealth -= dmg;

        if (currentWeaponHealth <= 0) return true;
        else return false;
    }

    public void RestoreWeapon(int heal)
    {
        currentWeaponHealth += heal;
        if (currentWeaponHealth >= maxWeaponHealth) currentWeaponHealth = maxWeaponHealth;
    }
}
