using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public GameObject m_dialogPrefab;

    public GameObject m_dialogObj;

    public Game instance;

    public List<List<string>> m_dialogInfo;

    public Subscription<DialogEvent> dialogSub;

    public Character m_other;

    public short curIdx = 0;

    public void showDialog(List<List<string>> dialogInfo, Character other)
    {
        curIdx = 0;
        m_other = other;
        m_dialogInfo = dialogInfo;

        GameObject newDialog = Instantiate(m_dialogPrefab);
        m_dialogObj = newDialog.gameObject;
        GameObject canvas = GameObject.Find("UICanvas");
        newDialog.transform.parent = canvas.transform;
        newDialog.transform.localPosition = new Vector3(0, (float)-371.53, 0);

        m_dialogObj.GetComponent<DialogUI>().m_next.onClick.AddListener(goNext);

        dialogExecute();
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

    private void addTeammateYES(CharacterUI ch)
    {
        m_status = 0;
        instance.m_teammate.Add(ch);
        instance.scraps -= 100;
        instance.scrapsLable.text = instance.scraps.ToString();
        List<List<string>> newDialogInfo = new List<List<string>> { };
        newDialogInfo.Add(new List<string> { "You", "Here you go! Nice and clean again!" });
        newDialogInfo.Add(new List<string> { "", "Many thanks! God bless you." });
        newDialogInfo.Add(new List<string> { "T", "NEXTROUND-Y" });
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
            m_dialogObj.GetComponent<DialogUI>().m_yes.onClick.AddListener(delegate { addTeammateYES(m_other.m_characterUI); });
            m_dialogObj.GetComponent<DialogUI>().m_no.onClick.AddListener(addTeammateNO);
            m_status = 1;
        }
        else if (m_dialogInfo[curIdx][0] == "T")
        {
            Destroy(m_dialogObj);
            if (m_dialogInfo[curIdx][1] == "NEXTROUND-Y")
            {
                instance.m_roundManager.NextRound();
            }
            else if (m_dialogInfo[curIdx][1] == "NEXTROUND-N")
            {
                CharacterUI curCharacter = instance.m_characters[instance.m_characters.Count - 1].GetComponent<CharacterUI>();
                curCharacter.transform.localScale = new Vector3(-1, 1, 1);
                Vector3 dest = new Vector3(curCharacter.transform.localPosition.x - 630, 0, 0);
                Vector3 anime = new Vector3(1, 0, 0);
                curCharacter.MoveTo(dest, anime, 3);
            }
        }
    }
}
