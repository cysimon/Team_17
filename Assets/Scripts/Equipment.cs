using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
        m_id = Game.equipmentIDCounter++;
        m_owner = owner;
        m_type = type;
        System.Random rand1 = new System.Random(unchecked((int)DateTime.Now.Ticks));
        System.Random rand2 = new System.Random(unchecked((int)DateTime.Now.Ticks));
        System.Random rand3 = new System.Random(unchecked((int)DateTime.Now.Ticks));
        m_name = Utility.equipmentNames[rand1.Next(5)] + " " + (type == 0? Utility.weaponTypes[rand2.Next(3)] : Utility.armorTypes[rand3.Next(3)]);
        m_durable = 100;
        System.Random rand4 = new System.Random(Utility.GetRandomSeed());
        m_power = (short)rand4.Next(21, 41);
    }
}
