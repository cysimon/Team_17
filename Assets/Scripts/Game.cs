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

    public RoundManager m_roundManager;

    public GameObject characterPrefab;


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

            m_roundManager.instance = this;
            m_roundManager.NextRound();

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

    //public Game getInstance()
    //{
    //    return instance;
    //}

    public void addNewCharacter()
    {
        GameObject newCharacter = Instantiate(characterPrefab);
        GameObject canvas = GameObject.Find("UICanvas");
        newCharacter.transform.parent = canvas.transform;
        newCharacter.transform.localPosition = new Vector3(610, -120, 0);

    }
}
