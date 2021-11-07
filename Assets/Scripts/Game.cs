using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<GameObject> m_characters;

    public List<CharacterUI> m_teammate;

    public List<CharacterUI> m_enemy;

    public RoundManager m_roundManager;

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
        newCharacter.transform.localPosition = new Vector3(610, -120, 0);
        if (type == 1)
        {
            Vector3 dest = new Vector3(610 - (-200) - 200 * m_teammate.Count, -120 - (-250), 0);
            Vector3 anime = new Vector3(-1, -1, 0);
            newCharacter.GetComponent<CharacterUI>().MoveTo(dest, anime, 1);
        }
        else if (type == 0)
        {
            Vector3 dest = new Vector3(610 - (740), -120 - (-250), 0);
            Vector3 anime = new Vector3(1, -1, 0);
            newCharacter.GetComponent<CharacterUI>().MoveTo(dest, anime, 2);
        }
        
    }

    void CharacterEventListener(CharacterEvent eventIn)
    {
        if (eventIn.m_type == 1)
        {
            addTeammateDecision(eventIn.m_sender);
        }
        else if (eventIn.m_type == 2)
        {
            Debug.Log("进入战斗了卧槽");
        }
        return;
    }

    public void addTeammateDecision(CharacterUI ch)
    {
        ch.addTeamYesObj = Instantiate(ch.addTeamYes);
        ch.addTeamYesObj.transform.parent = ch.gameObject.transform;
        ch.addTeamYesObj.transform.localPosition = new Vector3(-120, 340, 0);
        ch.addTeamYesObj.GetComponent<Button>().onClick.AddListener(delegate { addTeammateYES(ch); });
        ch.addTeamNoObj = Instantiate(ch.addTeamNo);
        ch.addTeamNoObj.transform.parent = ch.gameObject.transform;
        ch.addTeamNoObj.transform.localPosition = new Vector3(40, 340, 0);
        ch.addTeamNoObj.GetComponent<Button>().onClick.AddListener(delegate { addTeammateNO(ch); });
    }

    private void addTeammateYES(CharacterUI ch)
    {
        m_teammate.Add(ch);
        Destroy(ch.addTeamYesObj);
        Destroy(ch.addTeamNoObj);
        m_roundManager.NextRound();
    }

    private void addTeammateNO(CharacterUI ch)
    {
        Destroy(ch.addTeamYesObj);
        Destroy(ch.addTeamNoObj);
        Destroy(ch.gameObject);
        m_roundManager.NextRound();
    }
}
