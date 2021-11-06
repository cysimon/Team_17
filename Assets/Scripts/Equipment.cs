using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment
{
    public short m_id;

    public string m_name;

    public Character m_owner;

    // 0: weapon 1: armor
    public short m_type;

    // 0 - 100
    public short m_durable;

    public short m_power;


    public Equipment(Character owner, short type)
    {
        m_id = Global.equipmentIDCounter++;
        m_owner = owner;
        m_type = type;
        System.Random rand = new System.Random();
        m_name = Utility.equipmentNames[rand.Next(5)] + " " + (type == 0? Utility.weaponTypes[rand.Next(3)] : Utility.armorTypes[rand.Next(3)]);
        m_durable = 100;
        m_power = (short)rand.Next(21, 41);
    }
}
