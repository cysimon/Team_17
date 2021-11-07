using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CharacterEvent
{
    /** 
     * 0: nothing
     * 1: finishedMovingToShelf
     * 2: enter battle round
     * 3: dead
     * */
    public short m_type = 0;

    public CharacterUI m_sender;

    public CharacterEvent(short type, CharacterUI sender)
    {
        m_type = type;
        m_sender = sender;
    }

}

public class GameEvent
{
    /** 
     * 0: nothing
     * 1: finishedBattle
     * 3: dead
     * */
    public short m_type = 0;

    public GameEvent(short type)
    {
        m_type = type;
    }

}

public class Game : MonoBehaviour
{
    public static Game instance;

    public static short characterIDCounter = 0;

    public static short equipmentIDCounter = 0;

    public short scraps = 200;

    public short m_days = 0;

    public Text scrapsLable;

    public Text daysLable;

    public List<GameObject> m_characters;

    public List<Character> m_teammate;

    public List<Character> m_enemy;

    public RoundManager m_roundManager;

    public DialogManager m_dialogManager;

    public GameObject characterPrefab;

    public Subscription<CharacterEvent> characterSub;

    public Subscription<GameEvent> gameSub;

    public GameObject loseGamePrefab;

    public GameObject loseGameObj;

    public GameObject dayBoardPrefab;

    public GameObject dayBoardObj;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {

            m_roundManager.instance = this;

            m_days = 0;

            m_dialogManager.instance = this;

            characterSub = EventBus.Subscribe<CharacterEvent>(CharacterEventListener);

            gameSub = EventBus.Subscribe<GameEvent>(GameEventListener);

            GameObject dayboard = Instantiate(dayBoardPrefab);
            dayBoardObj = dayboard.gameObject;
            dayBoardObj.SetActive(false);

            instance = this;

            m_dialogManager.showDialog(Utility.dialogIntro, new Character(0, null), 1);
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

    public void addNewCharacter(short type)
    {
        GameObject newCharacter = Instantiate(characterPrefab);
        newCharacter.GetComponent<CharacterUI>().m_character = new Character(type, newCharacter.GetComponent<CharacterUI>());
        m_characters.Add(newCharacter);
        GameObject canvas = GameObject.Find("UICanvas");
        newCharacter.transform.parent = canvas.transform;
        newCharacter.transform.localPosition = new Vector3(610, -213, 0);
        Debug.Log(type);
        Debug.Log(m_teammate.Count);
        if (type == 1)
        {
            Vector3 dest = new Vector3(630 - (-235) - 200 * m_teammate.Count, 0, 0);
            Vector3 anime = new Vector3(-1, 0, 0);
            newCharacter.GetComponent<CharacterUI>().MoveTo(dest, anime, 1);
        }
        else if (type == 0)
        {
            Vector3 dest = new Vector3(630 - (600), 0, 0);
            Vector3 anime = new Vector3(-1, 0, 0);
            newCharacter.GetComponent<CharacterUI>().MoveTo(dest, anime, 2);
        }
        
    }

    void CharacterEventListener(CharacterEvent eventIn)
    {
        if (eventIn.m_type == 1)
        {
            m_dialogManager.showDialog(Utility.dialogSetWithOthers, eventIn.m_sender.m_character, 0);
        }
        else if (eventIn.m_type == 2)
        {
            StartCoroutine(waitForBattle(eventIn.m_sender.m_character));
        }
        else if (eventIn.m_type == 3)
        {
            Debug.Log("吸溜吸溜");
            eventIn.m_sender.gameObject.SetActive(false);
            Destroy(eventIn.m_sender.m_spriteRenderer);
            StartCoroutine(waitForNextRound());
        }
        else if (eventIn.m_type == 99)
        {
            GameObject loseGame = Instantiate(loseGamePrefab);
            loseGameObj = loseGame.gameObject;
            GameObject canvas = GameObject.Find("UICanvas");
            loseGameObj.transform.parent = canvas.transform;
            loseGameObj.transform.localPosition = new Vector3(610, -213, 0);
        }

        return;
    }

    public void NextDay()
    {
        if (instance.m_teammate != null)
        {
            for (int i = 0; i < instance.m_teammate.Count; i++)
            {
                Destroy(instance.m_teammate[i].m_characterUI.m_spriteRenderer);
                Destroy(instance.m_teammate[i].m_characterUI.gameObject);
            }

            for (int i = 0; i < instance.m_enemy.Count; i++)
            {
                Destroy(instance.m_enemy[i].m_characterUI.m_spriteRenderer);
                Destroy(instance.m_enemy[i].m_characterUI.gameObject);
            }
        }


        instance.m_teammate = new List<Character>
        {
            //new Character(1), new Character(1), new Character(1)
        };

        instance.m_enemy = new List<Character>
        {
            //new Character(0)
        };
        instance.m_days += 1;
        instance.daysLable.text = m_days.ToString();
        Debug.Log("来了");
        instance.dayBoardObj.SetActive(true);
        instance.dayBoardObj.GetComponentInChildren<Text>().text = "DAY " + m_days.ToString();
        GameObject canvas = GameObject.Find("UICanvas");
        dayBoardObj.transform.parent = canvas.transform;
        dayBoardObj.transform.localPosition = new Vector3(0, 0, 0);
        StartCoroutine(waitForNextDay());

    }

    void GameEventListener(GameEvent eventIn)
    {
        if (eventIn.m_type == 99)
        {
            GameObject loseGame = Instantiate(loseGamePrefab);
            loseGameObj = loseGame.gameObject;
            GameObject canvas = GameObject.Find("UICanvas");
            loseGameObj.transform.parent = canvas.transform;
            loseGameObj.transform.localPosition = new Vector3(0, 0, 0);
        }
        else if (eventIn.m_type == 1)
        {
            NextDay();
        }

        return;
    }

    private IEnumerator waitForNextDay()
    {
        yield return new WaitForSeconds(1.5f);
        instance.dayBoardObj.SetActive(false);
        instance.m_roundManager.m_roundRecord = new List<Round> { };
        instance.m_roundManager.NextRound();
    }

    private IEnumerator waitForNextRound()
    {
        yield return new WaitForSeconds(1f);
        instance.m_roundManager.NextRound();
    }

    private IEnumerator waitForBattle(Character enemey)
    {
        yield return new WaitForSeconds(1.5f);
        List<List<string>> dialogSetWithEnemey = new List<List<string>> {
            new List<string> { "", "Hey! What a nice day ... for robbery!" },
            new List<string> { "You", "Yo what's wrong with you man?" },
            new List<string> { "", "Shut up and give me all your scraps!" },
        };

        if (instance.m_teammate.Count > 0)
        {
            dialogSetWithEnemey.Add(new List<string> { "You", "You know what? Suck my d**k!" });
            dialogSetWithEnemey.Add(new List<string> { "T", "GOBATTLE" });
        }
        else
        {
            dialogSetWithEnemey.Add(new List<string> { "You", "Damn it! I don't have any teammate." });
            dialogSetWithEnemey.Add(new List<string> { "You", "God! Please no!!" });
            dialogSetWithEnemey.Add(new List<string> { "You", "NOOOOOOOOOOOO!!!!!" });
            dialogSetWithEnemey.Add(new List<string> { "T", "LOSEGAME" });
        }

        m_dialogManager.showDialog(dialogSetWithEnemey, enemey, 1);
    }
}
