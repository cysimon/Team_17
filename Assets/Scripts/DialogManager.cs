using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class DialogEvent
{
    /** 
     * 0: nothing
     * 1: next
     * */
    public short m_type = 0;

    public DialogEvent(short type)
    {
        m_type = type;
    }

}

public class DialogManager : MonoBehaviour
{

    // 1: in chose making stage
    public short m_status = 0;

    // 1: talk with enemey
    public short m_type = 0;

    public GameObject m_dialogPrefab;

    public GameObject m_dialogObj;

    public Game instance;

    public List<List<string>> m_dialogInfo;

    public Subscription<DialogEvent> dialogSub;

    public Character m_other;

    public short curIdx = 0;

    public void showDialog(List<List<string>> dialogInfo, Character other, short type)
    {
        curIdx = 0;
        m_other = other;
        m_dialogInfo = dialogInfo;
        m_type = type;

        GameObject newDialog = Instantiate(m_dialogPrefab);
        m_dialogObj = newDialog.gameObject;
        GameObject canvas = GameObject.Find("UICanvas");
        newDialog.transform.parent = canvas.transform;
        newDialog.transform.localPosition = new Vector3(0, (float)-371.53, 0);

        m_dialogObj.GetComponent<DialogUI>().m_next.onClick.AddListener(goNext);
        m_dialogObj.GetComponent<DialogUI>().m_introWeapon.onClick.AddListener(delegate { introWeapon(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_introArmor.onClick.AddListener(delegate { introArmor(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_introSelf.onClick.AddListener(delegate { introSelf(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_yes.onClick.AddListener(delegate { addTeammateYES(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_no.onClick.AddListener(addTeammateNO);

        dialogExecute();
    }

    public void leaveDialog()
    {
        m_dialogObj.GetComponent<DialogUI>().m_next.onClick.RemoveListener(goNext);
        m_dialogObj.GetComponent<DialogUI>().m_introWeapon.onClick.RemoveListener(delegate { introWeapon(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_introArmor.onClick.RemoveListener(delegate { introArmor(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_introSelf.onClick.RemoveListener(delegate { introSelf(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_yes.onClick.RemoveListener(delegate { addTeammateYES(m_other.m_characterUI); });
        m_dialogObj.GetComponent<DialogUI>().m_no.onClick.RemoveListener(addTeammateNO);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void goNext()
    {
        if (curIdx == m_dialogInfo.Count)
        {

            return;
        }

        curIdx++;
        if (m_status == 0)
        {
            dialogExecute();
        }   
    }

    private void introWeapon(CharacterUI ch)
    {
        m_status = 0;
        List<List<string>> newDialogInfo = new List<List<string>> { };
        newDialogInfo.Add(new List<string> { "", "Of course! My weapon is " + ch.m_character.m_weapon.m_name + "." });
        newDialogInfo.Add(new List<string> { "", "The weapon power is " + ch.m_character.m_weapon.m_power.ToString() + "." });
        newDialogInfo.Add(new List<string> { "C", "JOIN" });
        m_dialogInfo = newDialogInfo;
        curIdx = 0;
        dialogExecute();
    }

    private void introArmor(CharacterUI ch)
    {
        m_status = 0;
        List<List<string>> newDialogInfo = new List<List<string>> { };
        newDialogInfo.Add(new List<string> { "", "No problem! My armor is " + ch.m_character.m_armor.m_name + "."});
        newDialogInfo.Add(new List<string> { "", "The armor protect ability is " + ch.m_character.m_armor.m_power.ToString() + "." });
        newDialogInfo.Add(new List<string> { "C", "JOIN" });
        m_dialogInfo = newDialogInfo;
        curIdx = 0;
        dialogExecute();
    }

    private void introSelf(CharacterUI ch)
    {
        m_status = 0;
        List<List<string>> newDialogInfo = new List<List<string>> { };
        newDialogInfo.Add(new List<string> { "", "Well, my name is " + ch.m_character.m_name + "." });
        newDialogInfo.Add(new List<string> { "C", "JOIN" });
        m_dialogInfo = newDialogInfo;
        curIdx = 0;
        dialogExecute();
    }

    private void addTeammateYES(CharacterUI ch)
    {
        m_status = 0;
        List<List<string>> newDialogInfo = new List<List<string>> { };
        
        if (instance.scraps >= m_other.m_cost)
        {
            instance.scraps -= m_other.m_cost;
            instance.scrapsLable.text = instance.scraps.ToString();
            instance.m_teammate.Add(ch.m_character);
            newDialogInfo.Add(new List<string> { "You", "Here you go! Nice and clean again!" });
            newDialogInfo.Add(new List<string> { "", "Many thanks! God bless you." });
            newDialogInfo.Add(new List<string> { "T", "NEXTROUND-Y" });
        }
        else
        {
            newDialogInfo.Add(new List<string> { "You", "Oh shit! I don't have enough scraps for your equipment." });
            newDialogInfo.Add(new List<string> { "You", "I am so sorry man." });
            newDialogInfo.Add(new List<string> { "", "Ok. F**k you and have a nice day." });
            newDialogInfo.Add(new List<string> { "T", "NEXTROUND-N" });
        }
        
        m_dialogInfo = newDialogInfo;
        curIdx = 0;
        dialogExecute();
    }

    private void addTeammateNO()
    {
        m_status = 0;

        // TODO: 一定概率进战斗
        if (true)
        {
            List<List<string>> newDialogInfo = new List<List<string>> { };
            newDialogInfo.Add(new List<string> { "You", "Sorry maybe next time." });
            newDialogInfo.Add(new List<string> { "", "F**k you!" });
            newDialogInfo.Add(new List<string> { "T", "NEXTROUND-N" });
            m_dialogInfo = newDialogInfo;
            curIdx = 0;
            dialogExecute();
        }
    }

    private void dialogExecute()
    {
        if (m_dialogInfo[curIdx][0] == "You")
        {
            m_dialogObj.GetComponent<DialogUI>().youTalk(m_dialogInfo[curIdx][1]);
        }
        else if (m_dialogInfo[curIdx][0] == "")
        {
            m_dialogObj.GetComponent<DialogUI>().otherTalk(m_dialogInfo[curIdx][1], m_other.m_name);
        }
        else if (m_dialogInfo[curIdx][0] == "C")
        {
            m_dialogObj.GetComponent<DialogUI>().youChoose(m_dialogInfo[curIdx][1]);
            m_dialogObj.GetComponent<DialogUI>().m_yes.gameObject.GetComponentInChildren<Text>().text = "Yes (Use " + m_other.m_cost.ToString() + " scrap. Get teammate)";
            m_dialogObj.GetComponent<DialogUI>().m_no.gameObject.GetComponentInChildren<Text>().text = "No (This guy may become a rubber)";
            m_status = 1;
        }
        else if (m_dialogInfo[curIdx][0] == "T")
        {
            leaveDialog();
            Destroy(m_dialogObj);

            if (m_dialogInfo[curIdx][1] == "NEXTROUND-Y")
            {
                CharacterUI curCharacter = instance.m_characters[instance.m_characters.Count - 1].GetComponent<CharacterUI>();
                curCharacter.m_spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
                StartCoroutine(waitForNextRound());
            }
            else if (m_dialogInfo[curIdx][1] == "NEXTROUND-N")
            {
                CharacterUI curCharacter = instance.m_characters[instance.m_characters.Count - 1].GetComponent<CharacterUI>();
                curCharacter.m_spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
                Vector3 dest = new Vector3(curCharacter.transform.localPosition.x - 630, 0, 0);
                Vector3 anime = new Vector3(1, 0, 0);
                curCharacter.MoveTo(dest, anime, 3);
            }
            else if (m_dialogInfo[curIdx][1] == "GOBATTLE")
            {
                Debug.Log("进入战斗了卧槽");
                instance.m_enemy.Add(m_other);
                SceneManager.LoadScene("YiyangLab");
            }
            else if (m_dialogInfo[curIdx][1] == "LOSEGAME")
            {
                EventBus.Publish<GameEvent>(new GameEvent(99));
            }
        }
    }

    private IEnumerator waitForNextRound()
    {
        yield return new WaitForSeconds(1f);
        instance.m_roundManager.NextRound();
    }
}
