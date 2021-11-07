using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    public List<Round> m_roundRecord = new List<Round> {};

    public static List<short> m_testList = new List<short> { 1, 1, 1, 1, 0 };

    public Text m_roundCounter;

    public Game instance;

    public void NextRound()
    {
        Round newRound = new Round(m_testList[m_roundRecord.Count]);
        Debug.Log(m_testList);
        Debug.Log(m_roundRecord.Count);
        m_roundRecord.Add(newRound);
        m_roundCounter.text = m_roundRecord.Count.ToString();
        instance.addNewCharacter(newRound.m_type);
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