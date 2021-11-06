using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Character m_character;

    public Text m_groupLabel;

    public Text m_characterNameLabel;

    public Text m_weaponNameLabel;

    public Text m_armorNameLabel;

    public Text m_powerLabel;


    // Start is called before the first frame update
    void Start()
    {
        m_character = new Character(1);
        m_groupLabel.text = "GROUP: " + (1).ToString();
        m_characterNameLabel.text = "NAME: " + m_character.m_name;
        m_weaponNameLabel.text = "WEAPON: " + m_character.m_weapon.m_name;
        m_armorNameLabel.text = "ARMOR: " + m_character.m_armor.m_name;
        m_powerLabel.text = "POWER: " + m_character.m_strength.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
