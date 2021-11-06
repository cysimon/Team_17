using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility
{
    public static List<string> firstNames = new List<string> {
        "Andy", "Ben", "Charle", "Danniel", "Eddie", "Freddy", "Gelbert", "Hector", "Jose"
    };

    public static List<string> SurNames = new List<string> {
        "Alexandra", "Basil", "Carlos", "D'Antonio", "Edward", "Francis", "Gilles", "Helena", "Justinus"
    };

    public static List<string> equipmentNames = new List<string> {
        "Dracula's", "Fatih's", "Caesar's", "Shiny", "Crusader's"
    };

    public static List<string> weaponTypes = new List<string> {
        "Laser Gun", "Sonic Cannon", "Electromagnetic Gun"
    };

    public static List<string> armorTypes = new List<string> {
        "Body Armor", "Shield", "Oyoroi"
    };

    public static short Max(short i, short j) {
        if (i >= j)
        {
            return i;
        }

        return j;
    }

    public static short Min(short i, short j)
    {
        if (i <= j)
        {
            return i;
        }

        return j;
    }
}
