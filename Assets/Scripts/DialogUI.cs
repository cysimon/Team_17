using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    public Text m_yourName;

    public Text m_otherName;

    public Text m_content;

    public Image m_yourAvatar;

    public Image m_otherAvatar;

    public Button m_introArmor;

    public Button m_introWeapon;

    public Button m_introSelf;

    public Button m_yes;

    public Button m_no;

    public Button m_next;

    public void youTalk(string content)
    {
        hideAll();
        m_yourName.gameObject.SetActive(true);
        m_yourName.text = "You";
        m_yourAvatar.gameObject.SetActive(true);
        m_content.gameObject.SetActive(true);
        m_content.text = content;
    }

    public void youChoose(string type)
    {
        hideAll();
        m_yourName.gameObject.SetActive(true);
        m_yourName.text = "You";
        m_yourAvatar.gameObject.SetActive(true);
        m_content.gameObject.SetActive(true);
        m_content.text = "Emmmmmm";
        m_introArmor.gameObject.SetActive(true);
        m_introWeapon.gameObject.SetActive(true);
        m_introSelf.gameObject.SetActive(true);
        m_yes.gameObject.SetActive(true);
        m_no.gameObject.SetActive(true);
        if (type == "JOIN")
        {
            m_yes.gameObject.GetComponentInChildren<Text>().text = "Yes (Use 100 scrap. Get teammate)";
            m_no.gameObject.GetComponentInChildren<Text>().text = "No (This guy may become a rubber)";
        }
    }

    public void otherTalk(string content, string otherName)
    {
        hideAll();
        m_otherName.gameObject.SetActive(true);
        m_otherName.text = otherName;
        m_otherAvatar.gameObject.SetActive(true);
        m_content.gameObject.SetActive(true);
        m_content.text = content;
    }

    public void hideAll()
    {
        m_yourName.gameObject.SetActive(false);
        m_otherName.gameObject.SetActive(false);
        m_content.gameObject.SetActive(false);
        m_yourAvatar.gameObject.SetActive(false);
        m_otherAvatar.gameObject.SetActive(false);
        m_yes.gameObject.SetActive(false);
        m_no.gameObject.SetActive(false);
        m_introArmor.gameObject.SetActive(false);
        m_introWeapon.gameObject.SetActive(false);
        m_introSelf.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
