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

    public Text scrapsLable;

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


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // TODO: for test currently
            m_teammate = new List<Character> {
                //new Character(1), new Character(1), new Character(1)
            };
            m_enemy = new List<Character> {
                //new Character(0)
            };

            m_roundManager.instance = this;
            m_roundManager.NextRound();

            m_dialogManager.instance = this;

            characterSub = EventBus.Subscribe<CharacterEvent>(CharacterEventListener);

            gameSub = EventBus.Subscribe<GameEvent>(GameEventListener);

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

        return;
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
