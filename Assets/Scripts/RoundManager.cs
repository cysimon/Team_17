using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public List<Round> m_roundRecord = new List<Round> {};

    public List<short> m_testList = new List<short> { 1, 0, 1, 1, 0 };

    public Game instance;

    public void NextRound()
    {
        Round newRound = new Round(m_testList[m_roundRecord.Count]);
        m_roundRecord.Add(newRound);
        instance.addNewCharacter();
    }
}

public class Round
{
    // 0: come enemy 1: come friend
    public short m_type;

    public Round(short type)
    {
        m_type = type;
    }
}