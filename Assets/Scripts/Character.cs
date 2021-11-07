using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public short m_id;

    public string m_name;

    public short m_group;

    public Equipment m_armor;

    public Equipment m_weapon;

    public short m_strength;

    public CharacterUI m_characterUI;


    public Character(short group, CharacterUI ui)
    {
        m_id = Game.characterIDCounter++;
        System.Random rand1 = new System.Random(9898);
        System.Random rand2 = new System.Random(9797);
        m_name = Utility.firstNames[rand1.Next(8)] + " " + Utility.SurNames[rand2.Next(8)];
        m_group = group;
        m_armor = new Equipment(this, 1);
        m_weapon = new Equipment(this, 0);
        m_strength = (short)rand1.Next(21, 41);
        m_characterUI = ui;
    }


    public void receiveDamage(short damage, short type)
    {
        if (type == 1)
        {
            m_armor.m_durable = Utility.Max((short)(m_armor.m_durable - damage), 0);
        }
        else if (type == 0)
        {
            m_weapon.m_durable = Utility.Max((short)(m_weapon.m_durable - damage), 0);
        }
    }

    // return (type, damage)
    public (short, short) doAttack(Character enemy)
    {
        System.Random rand = new System.Random(8888);
        short attackType = (short)rand.Next(2);
        short baseAttackVal = (short)(m_strength + m_weapon.m_durable * m_weapon.m_power);
        short damage = (short)rand.Next(baseAttackVal - 5, baseAttackVal + 5);

        return (attackType, damage);
    }

    public Equipment repairRequest()
    {
        return m_weapon;
    }
}
