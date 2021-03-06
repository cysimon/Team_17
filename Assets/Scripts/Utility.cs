using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utility
{
    public static List<string> firstNames = new List<string> {
        "Andy", "Ben", "Charle", "Danniel", "Eddie", "Freddy", "Gelbert", "Hector", "Jose", "Alexandra", "Basil", "Carlos", "D'Antonio", "Edward", "Francis", "Gilles", "Helena", "Justinus"
    };

    public static List<string> equipmentNames = new List<string> {
        "Numen's", "Yiqi's", "Caesar's", "Shiny", "Crusader's"
    };

    public static List<string> weaponTypes = new List<string> {
        "Laser", "Cannon", "Gun"
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

    public static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);

    }

    public static List<List<string>> dialogIntro = new List<List<string>> {
        new List<string> { "You", "Another day in this post-apoc world." },
        new List<string> { "You", "My job is to fix the equipment of those scavengers." },
        new List<string> { "You", "The people I helped will help me to fight the bastards who want to destroy my shop." },
        new List<string> { "You", "Wish me luck." },
        new List<string> { "T", "GAMESTART" },
    };

    public static List<List<string>> dialogSetWithOthers = new List<List<string>> {
        new List<string> { "", "Hey dude! What a nice day!" },
        new List<string> { "You", "Hi bro. What can I do for you?" },
        new List<string> { "", "My equipments need to be repaired. Could you please help me fix them?" },
        new List<string> { "", "I will protect your store for you as a pay back." },
        new List<string> { "C", "JOIN" }
    };

    public static List<List<string>> dialogSetWithEnemey = new List<List<string>> {
        new List<string> { "", "Hey! What a nice day ... for robbery!" },
        new List<string> { "You", "Yo what's wrong with you man?" },
        new List<string> { "", "Shut up and give me all your scraps!" },
    };
}
