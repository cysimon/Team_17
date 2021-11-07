using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, TEAMMATETURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject teammatePrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;
    public Transform[] teammateBattleStations = new Transform[3];

    public BattleState battleState;

    Unit enemyUnit;
    Unit[] teammateUnit = new Unit[3];
    int playerHeal = 20;

    public BattleHUD enemyHUD;
    public BattleHUD[] teammateHUD = new BattleHUD[3];

    bool isEnemyDeadArmor = false;
    bool isEnemyDeadWeapon = false;
    bool[] isTeamDeadArmor = { true, true, true };
    bool[] isTeamDeadWeapon = { true, true, true };

    public Text battleBeginText;
    public GameObject[] teamUIContainer;
    public Button[] restoreButtons;
    public GameObject enemyUIContainer;

    int maxCharacters;

    public Animator attackAni;
    void Start()
    {
        battleState = BattleState.START;
        StartCoroutine(BattleBegin());
    }

    IEnumerator BattleBegin()
    {
        for(int i = 0; i < 3; i++)
        {
            battleBeginText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleBeginText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.3f);
        }
        
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponentInChildren<Unit>();

        //只有一个enemy
        //需要考虑加强enemy
        Character m_enemy = Game.instance.m_enemy[0];
        enemyUnit.unitName = m_enemy.m_name;
        enemyUnit.unitId = -1;
        enemyUnit.characterID = m_enemy.m_id;
        enemyUnit.maxArmor = m_enemy.m_armor.m_durable+300;
        enemyUnit.currentArmor = enemyUnit.maxArmor;
        enemyUnit.maxWeaponHealth = m_enemy.m_weapon.m_durable;
        enemyUnit.currentWeaponHealth = enemyUnit.maxWeaponHealth;
        enemyUnit.unitDamage = m_enemy.m_weapon.m_power+20;

        //如果多于3个，可以考虑让玩家选择3个，现在取前3个
        maxCharacters = Game.instance.m_teammate.Count;
        //Debug.Log(Game.instance.m_teammate.Count);
        if (maxCharacters >= 3) maxCharacters = 3;

        for(int i = 0; i < maxCharacters; i++)
        {
            GameObject[] teamGO = new GameObject[maxCharacters];
            teamGO[i] = Instantiate(teammatePrefab, teammateBattleStations[i]);
            teammateUnit[i] = teamGO[i].GetComponentInChildren<Unit>();

            Character m_teammate = Game.instance.m_teammate[i];
            teammateUnit[i].unitName = m_teammate.m_name;
            teammateUnit[i].unitId = i;
            teammateUnit[i].characterID = m_teammate.m_id;

            teammateUnit[i].maxArmor = m_teammate.m_armor.m_durable;
            teammateUnit[i].currentArmor = teammateUnit[i].maxArmor;
            isTeamDeadArmor[i] = false;

            teammateUnit[i].maxWeaponHealth = m_teammate.m_weapon.m_durable;
            teammateUnit[i].currentWeaponHealth = teammateUnit[i].maxWeaponHealth;
            isTeamDeadWeapon[i] = false;
            
            teammateUnit[i].unitDamage = m_teammate.m_weapon.m_power;
        }
        enemyUIContainer.SetActive(true);
        enemyHUD.SetHUD(enemyUnit);

        for(int i = 0; i < maxCharacters; i++)
        {
            teamUIContainer[i].SetActive(true);
            teammateHUD[i].SetHUD(teammateUnit[i]);
        }

        yield return new WaitForSeconds(2f);

        battleState = BattleState.PLAYERTURN;
        StartCoroutine(TeammateAttack());
    }

    IEnumerator TeammateAttack()
    {
        battleBeginText.text = "Teammate's turn";
        battleBeginText.gameObject.SetActive(true);

        for (int i = 0; i < maxCharacters; i++)
        {
            if (isTeamDeadArmor[i]) continue;
            restoreButtons[i * 2].gameObject.SetActive(false);
            restoreButtons[i * 2 + 1].gameObject.SetActive(false);
        }

        for (int i = 0; i < maxCharacters; i++)
        {
            if (isTeamDeadArmor[i])
            {
                battleBeginText.text = "The teammate " + teammateUnit[i].name + " is dead and will never come back";
                yield return new WaitForSeconds(2f);
                continue;
            }

            if (isTeamDeadWeapon[i])
            {
                battleBeginText.text = "The teammate " + teammateUnit[i].name + " cannot attack with 0 weaponHP";
                yield return new WaitForSeconds(2f);
                continue;
            }

            System.Random rand = new System.Random();
            int randNum = rand.Next(2);

            if (randNum == 1)
            {
                if (isEnemyDeadWeapon) randNum = 0;
                else
                {
                    isEnemyDeadWeapon = enemyUnit.TakeDamageWeapon(teammateUnit[i].unitDamage);
                    enemyHUD.SetWeapon(enemyUnit);
                }
            }
            if (randNum == 0)
            {
                isEnemyDeadArmor = enemyUnit.TakeDamageArmor(teammateUnit[i].unitDamage);
                enemyHUD.SetArmor(enemyUnit);
            }

            attackAni = teammateUnit[i].gameObject.GetComponent<Animator>();
            attackAni.SetBool("isAttacking", true);
            yield return new WaitForSeconds(2f);
            attackAni.SetBool("isAttacking", false);

            if (isEnemyDeadArmor)
            {
                attackAni = enemyUnit.gameObject.GetComponent<Animator>();
                attackAni.SetBool("isDead", true);
                yield return new WaitForSeconds(2f);
                battleState = BattleState.WON;
                break;
            }
        }

        if (battleState == BattleState.WON) EndBattle();
        else
        {
            battleState = BattleState.ENEMYTURN;
            StartCoroutine(EnemyAttack());
        }
    }

    int[] Shuffle(int[] array)
    {
        System.Random rand = new System.Random();

        for(int i = array.Length-1; i >= 0; i--)
        {
            int randomIndex = rand.Next(i);
            
            int numInIndex = array[randomIndex];

            array[randomIndex] = array[i];
            array[i] = numInIndex;
        }

        return array;
    }

    IEnumerator DeathCheck(int i)
    {
        if (isTeamDeadArmor[i])
        {
            EventBus.Publish<CharacterEvent>(new CharacterEvent(4, Game.instance.m_teammate[i].m_characterUI));
            //死了的动画
            attackAni = teammateUnit[i].gameObject.GetComponent<Animator>();
            attackAni.SetBool("isDead", true);
            battleBeginText.text = "Teammate " + teammateUnit[i].unitName + " dead!";
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator EnemyAttack()
    {
        attackAni = enemyUnit.gameObject.GetComponent<Animator>();

        battleBeginText.text = "Enemy's turn";
        battleBeginText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        System.Random rand = new System.Random();
        int[] randNumArray = new int[maxCharacters];
        for(int i=0;i< maxCharacters; i++)
        {
            randNumArray[i] = i;
        }
        Shuffle(randNumArray);

        int randNum = rand.Next(4);
        if (isEnemyDeadWeapon)
        {
            battleBeginText.text = "Enemy healing";

            enemyUnit.RestoreWeapon(enemyUnit.maxWeaponHealth/2);
            enemyHUD.SetWeapon(enemyUnit);
            enemyUnit.RestoreArmor(enemyUnit.maxArmor/4);
            enemyHUD.SetArmor(enemyUnit);
        }
        else if (randNum == 0|| randNum == 1)
        {
            battleBeginText.text = "Enemy normal attack";

            int attackTarget = randNumArray[0];
            for(int i=0;i< maxCharacters; i++)
            {
                attackTarget = randNumArray[i];
                if (!isTeamDeadArmor[attackTarget]) break;
            }

            isTeamDeadArmor[attackTarget] = teammateUnit[attackTarget].TakeDamageArmor(enemyUnit.unitDamage);
            teammateHUD[attackTarget].SetArmor(teammateUnit[attackTarget]);

            StartCoroutine(DeathCheck(attackTarget));
        }
        else if (randNum == 2)
        {
            battleBeginText.text = "Enemy attack all";
            for (int i = 0; i < maxCharacters; i++)
            {
                if (isTeamDeadArmor[randNumArray[i]]) continue;
                isTeamDeadArmor[randNumArray[i]] = teammateUnit[randNumArray[i]].TakeDamageArmor(enemyUnit.unitDamage/2);
                teammateHUD[randNumArray[i]].SetArmor(teammateUnit[randNumArray[i]]);
                StartCoroutine(DeathCheck(randNumArray[i]));
            }
        }
        else if (randNum == 3)
        {
            battleBeginText.text = "Enemy heavy attack";

            int attackTarget = randNumArray[0];
            for (int i = 0; i < maxCharacters; i++)
            {
                attackTarget = randNumArray[i];
                if (!isTeamDeadArmor[attackTarget]) break;
            }

            isTeamDeadArmor[attackTarget] = teammateUnit[attackTarget].TakeDamageArmor(enemyUnit.unitDamage / 2);
            teammateHUD[attackTarget].SetArmor(teammateUnit[attackTarget]);
            isTeamDeadWeapon[attackTarget] = teammateUnit[attackTarget].TakeDamageWeapon(enemyUnit.unitDamage);
            teammateHUD[attackTarget].SetWeapon(teammateUnit[attackTarget]);

            StartCoroutine(DeathCheck(attackTarget));
        }

        attackAni.SetBool("isAttacking", true);
        yield return new WaitForSeconds(2f);
        attackAni.SetBool("isAttacking", false);

        if (isTeamDeadArmor[0] && isTeamDeadArmor[1] && isTeamDeadArmor[2])
        {
            battleState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            battleState = BattleState.PLAYERTURN;
            StartCoroutine(PlayerTurn());
        }
    }

    void EndBattle()
    {
        if(battleState == BattleState.WON)
        {
            EventBus.Publish<GameEvent>(new GameEvent(1));
        }else if(battleState == BattleState.LOST)
        {
            EventBus.Publish<GameEvent>(new GameEvent(2));
        }
        SceneManager.LoadScene("RuozhouLab");
    }

    IEnumerator PlayerTurn()
    {
        battleBeginText.text = "Your turn";

        for (int i = 0; i < 3; i++)
        {
            battleBeginText.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            battleBeginText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.3f);
        }
        battleBeginText.gameObject.SetActive(true);
        
        for(int i = 0; i < maxCharacters; i++)
        {
            if (isTeamDeadArmor[i]) continue;
            restoreButtons[i * 2].gameObject.SetActive(true);
            restoreButtons[i * 2+1].gameObject.SetActive(true);
        }
    }
    IEnumerator RestoreArmor(int unitID)
    {
        if (isTeamDeadArmor[unitID])
        {
            battleBeginText.text = "Teammate already dead";

            for (int i = 0; i < 3; i++)
            {
                battleBeginText.gameObject.SetActive(true);
                yield return new WaitForSeconds(.5f);
                battleBeginText.gameObject.SetActive(false);
                yield return new WaitForSeconds(.3f);
            }

            battleBeginText.text = "Your turn";
            battleBeginText.gameObject.SetActive(true);
        }
        else
        {
            teammateUnit[unitID].RestoreArmor(playerHeal);
            teammateHUD[unitID].SetArmor(teammateUnit[unitID]);

            yield return new WaitForSeconds(2f);

            Debug.Log(unitID);

            battleState = BattleState.TEAMMATETURN;
            StartCoroutine(TeammateAttack());
        }
    }
    IEnumerator RestoreWeapon(int unitID)
    {
        isTeamDeadWeapon[unitID] = false;

        teammateUnit[unitID].RestoreWeapon(playerHeal);
        teammateHUD[unitID].SetWeapon(teammateUnit[unitID]);

        yield return new WaitForSeconds(2f);

        battleState = BattleState.TEAMMATETURN;
        StartCoroutine(TeammateAttack());
    }

    public void OnClickArmor(int unitID)
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        StartCoroutine(RestoreArmor(unitID));
    }

    public void OnClickWeapon(int unitID)
    {
        if (battleState != BattleState.PLAYERTURN)
            return;

        StartCoroutine(RestoreWeapon(unitID));
    }
}
