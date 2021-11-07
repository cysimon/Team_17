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
     * */
    public short m_type = 0;

    public CharacterUI m_sender;

    public CharacterEvent(short type, CharacterUI sender)
    {
        m_type = type;
        m_sender = sender;
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

    public List<CharacterUI> m_teammate;

    public List<CharacterUI> m_enemy;

    public RoundManager m_roundManager;

    public DialogManager m_dialogManager;

    public GameObject characterPrefab;

    public Subscription<CharacterEvent> characterSub;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            // TODO: for test currently
            m_teammate = new List<CharacterUI> {
                //new Character(1), new Character(1), new Character(1)
            };
            m_enemy = new List<CharacterUI> {
                //new Character(0)
            };

            m_roundManager.instance = this;
            m_roundManager.NextRound();

            m_dialogManager.instance = this;

            characterSub = EventBus.Subscribe<CharacterEvent>(CharacterEventListener);

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
            m_dialogManager.showDialog(Utility.dialogSetWithOthers, eventIn.m_sender.m_character);
        }
        else if (eventIn.m_type == 2)
        {
            Debug.Log("进入战斗了卧槽");
            SceneManager.LoadScene("YiyangLab");
        }
        else if (eventIn.m_type == 3)
        {
            Debug.Log("吸溜吸溜");
            eventIn.m_sender.gameObject.SetActive(false);
            instance.m_roundManager.NextRound();
        }
        return;
    }
}
