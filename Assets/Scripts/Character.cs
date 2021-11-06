using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Component
{
    public short m_id;
    public string m_name;
    public short m_group;
    public Equipment m_armor;
    public Equipment m_weapon;
    public short strength;

    public Character(short group)
    {
        m_id = Global.characterIDCounter++;

        System.Random rand = new System.Random();
        m_name = Utility.firstNames[rand.Next(8)] + " " + Utility.SurNames[rand.Next(8)];

        m_group = group;


    }
}
