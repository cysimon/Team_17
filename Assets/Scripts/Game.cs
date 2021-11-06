using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game instance;

    public static short characterIDCounter = 0;

    public static short equipmentIDCounter = 0;

    public static List<Character> m_teamMate;

    public static List<Character> m_enemy;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // TODO: for test currently
            m_teamMate = new List<Character> {
                new Character(1), new Character(1), new Character(1)
            };
            m_enemy = new List<Character> {
                new Character(0)
            };
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
