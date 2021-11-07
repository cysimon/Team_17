using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Slider weaponSlider;
    public Slider armorSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;

        weaponSlider.maxValue = unit.maxWeaponHealth;
        weaponSlider.value = unit.currentWeaponHealth;

        armorSlider.maxValue = unit.maxArmor;
        armorSlider.value = unit.currentArmor;
    }

    public void SetWeapon(Unit unit)
    {
        weaponSlider.value = unit.currentWeaponHealth;
    }

    public void SetArmor(Unit unit)
    {
        armorSlider.value = unit.currentArmor;
    }
}
