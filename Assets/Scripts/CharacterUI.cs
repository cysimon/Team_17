using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Character m_character;

    public Text m_groupLabel;

    public Text m_characterNameLabel;

    public Text m_weaponNameLabel;

    public Text m_armorNameLabel;

    public Text m_powerLabel;

    public GameObject addTeamYes;

    public GameObject addTeamYesObj;

    public GameObject addTeamNo;

    public GameObject addTeamNoObj;

    public Vector3 m_movingDistance;

    public Vector3 m_movingAnime;

    public short m_movingStatus;

    public GameObject m_spriteRendererPrefab;

    public SpriteRenderer m_spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        if (m_character == null)
        {
            m_character = new Character(0, this);
        }
        m_groupLabel.text = "GROUP: " + m_character.m_group.ToString();
        m_characterNameLabel.text = m_character.m_name;
        m_weaponNameLabel.text = "WEAPON: " + m_character.m_weapon.m_name;
        m_armorNameLabel.text = "ARMOR: " + m_character.m_armor.m_name;
        m_powerLabel.text = "POWER: " + m_character.m_strength.ToString();

        m_groupLabel.gameObject.SetActive(false);
        m_weaponNameLabel.gameObject.SetActive(false);
        m_armorNameLabel.gameObject.SetActive(false);
        m_powerLabel.gameObject.SetActive(false);


        GameObject spriteObj = Instantiate(m_spriteRendererPrefab);
        m_spriteRenderer = spriteObj.GetComponent<SpriteRenderer>();
        m_spriteRenderer.transform.position = new Vector3(6.1f, -2.88f, 0);
        Color temp = m_spriteRenderer.color;

        if (m_character.m_group == 1)
        {
            temp.a = 0.3f;
        }
        
        m_spriteRenderer.color = temp;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_spriteRenderer.color.a < 1.0f && m_movingStatus == 1)
        {
            Color temp = m_spriteRenderer.color;
            temp.a += 0.01f;
            m_spriteRenderer.color = temp;
        }
        //else if (m_spriteRenderer.color.a > 0.0f && m_movingStatus == 3)
        //{
        //    Color temp = m_spriteRenderer.color;
        //    temp.a -= 0.01f;
        //    m_spriteRenderer.color = temp;
        //}

        short speed = 1000;
        if (m_movingStatus != 0)
        {
            if ((m_movingAnime.x <= 0 && m_movingDistance.x <= 0) || (m_movingAnime.x >= 0 && m_movingDistance.x > 0) &&
                (m_movingAnime.y <= 0 && m_movingDistance.y <= 0) || (m_movingAnime.y >= 0 && m_movingDistance.y > 0))
            {
                Debug.Log("?????????");
                EventBus.Publish<CharacterEvent>(new CharacterEvent(m_movingStatus, this));
                m_movingStatus = 0;
            }
            else
            {
                if ((m_movingAnime.x <= 0 && m_movingDistance.x > 0) || (m_movingAnime.x >= 0 && m_movingDistance.x < 0))
                {
                    float movingX = m_movingAnime.x * speed * Time.deltaTime;
                    m_movingDistance.x += movingX;
                    this.gameObject.transform.localPosition += new Vector3(movingX, 0, 0);
                    m_spriteRenderer.transform.position += new Vector3(movingX / 100, 0f, 0f);
                }
                if ((m_movingAnime.y <= 0 && m_movingDistance.y > 0) || (m_movingAnime.y >= 0 && m_movingDistance.y < 0))
                {
                    float movingY = m_movingAnime.y * speed * Time.deltaTime;
                    m_movingDistance.y += movingY;
                    this.gameObject.transform.localPosition += new Vector3(0, movingY, 0);
                    m_spriteRenderer.transform.position += new Vector3(0f, movingY / 100, 0f);

                }
            }
        }
    }

    public void MoveTo(Vector3 distance, Vector3 anime, short type)
    {
        m_movingDistance = distance;
        m_movingAnime = anime;
        m_movingStatus = type;
    }
}
