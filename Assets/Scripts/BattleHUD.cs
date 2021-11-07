using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHUD : MonoBehaviour
{
    public Text nameText;
    public Text weaponText;
    public Slider weaponSlider;
    public Text armorText;
    public Slider armorSlider;

    public void SetHUD(Unit unit)
    {
        nameText.text = unit.unitName;

        weaponText.text = "Weapon: " + unit.currentWeaponHealth + " / " + unit.maxWeaponHealth;
        weaponSlider.maxValue = unit.maxWeaponHealth;
        weaponSlider.value = unit.currentWeaponHealth;

        armorText.text = "Armor: " + unit.currentArmor + " / " + unit.maxArmor;
        armorSlider.maxValue = unit.maxArmor;
        armorSlider.value = unit.currentArmor;
    }

    public void SetWeapon(Unit unit)
    {
        weaponText.text = "Weapon: " + unit.currentWeaponHealth + " / " + unit.maxWeaponHealth;
        weaponSlider.value = unit.currentWeaponHealth;
    }

    public void SetArmor(Unit unit)
    {
        armorText.text = "Armor: " + unit.currentArmor + " / " + unit.maxArmor;
        armorSlider.value = unit.currentArmor;
    }
}
