using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { START, PLAYERTURN, TEAMMATETURN, ENEMYTURN, WON, LOST }
/*public enum ClickEvents { NOCLICK, ARMOR, WEAPON}
public class clickEvent
{
    public ClickEvents clickEvents;
    public Unit thisUnit;

    public clickEvent(ClickEvents input, Unit inputUnit)
    {
        clickEvents = input;
        thisUnit = inputUnit;
    }
}*/

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
    bool[] isTeamDeadArmor = { false, false, false };
    bool[] isTeamDeadWeapon = { false, false, false };

    void Start()
    {
        battleState = BattleState.START;
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
        enemyUnit.maxArmor = m_enemy.m_armor.m_durable;
        enemyUnit.maxWeaponHealth = m_enemy.m_weapon.m_durable;
        enemyUnit.unitDamage = m_enemy.m_weapon.m_power;

        //如果多于3个，可以考虑让玩家选择3个，现在取前3个
        int maxCharacters = Game.instance.m_teammate.Count;
        Debug.Log(Game.instance.m_teammate.Count);
        if (maxCharacters >= 3) maxCharacters = 3;

        for(int i = 0; i < maxCharacters; i++)
        {
            GameObject[] teamGO = new GameObject[3];
            teamGO[i] = Instantiate(teammatePrefab, teammateBattleStations[i]);
            teammateUnit[i] = teamGO[i].GetComponentInChildren<Unit>();

            Character m_teammate = Game.instance.m_teammate[i];
            teammateUnit[i].unitName = m_teammate.m_name;
            teammateUnit[i].unitId = i;
            teammateUnit[i].characterID = m_teammate.m_id;
            teammateUnit[i].maxArmor = m_teammate.m_armor.m_durable;
            teammateUnit[i].maxWeaponHealth = m_teammate.m_weapon.m_durable;
            teammateUnit[i].unitDamage = m_teammate.m_weapon.m_power;
        }
        
        enemyHUD.SetHUD(enemyUnit);

        for(int i = 0; i < 3; i++)
        {
            teammateHUD[i].SetHUD(teammateUnit[i]);
        }

        yield return new WaitForSeconds(2f);

        battleState = BattleState.PLAYERTURN;
        StartCoroutine(TeammateAttack());
    }

    IEnumerator TeammateAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isTeamDeadArmor[i]||isTeamDeadWeapon[i]) continue;
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

            yield return new WaitForSeconds(2f);

            if (isEnemyDeadArmor)
            {
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

    IEnumerator EnemyAttack()
    {
        System.Random rand = new System.Random();
        int randNum1;
        while (true)
        {
            randNum1 = rand.Next(3);
            if (!isTeamDeadArmor[randNum1]) break;
        }

        int randNum2 = rand.Next(2);
        if (randNum2 == 0)
        {
            isTeamDeadArmor[randNum1] = teammateUnit[randNum1].TakeDamageArmor(enemyUnit.unitDamage);
            teammateHUD[randNum1].SetArmor(teammateUnit[randNum1]);
        }
        else if (randNum2 == 1&&(!isEnemyDeadWeapon))
        {
            isTeamDeadWeapon[randNum1] = teammateUnit[randNum1].TakeDamageWeapon(enemyUnit.unitDamage);
            teammateHUD[randNum1].SetWeapon(teammateUnit[randNum1]);
        }

        yield return new WaitForSeconds(2f);

        if (isTeamDeadArmor[0] && isTeamDeadArmor[1] && isTeamDeadArmor[2])
        {
            battleState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            if (isTeamDeadArmor[randNum1])
            {
                Debug.Log("Teammate " + randNum1 + " dead!");
            }
            battleState = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if(battleState == BattleState.WON)
        {
            Debug.Log("Win! Do something");

        }else if(battleState == BattleState.LOST)
        {
            Debug.Log("Game End!");
        }
    }

    void PlayerTurn()
    {
        Debug.Log("Your turn");
    }
    IEnumerator RestoreArmor(int unitID)
    {
        teammateUnit[unitID].RestoreArmor(playerHeal);
        teammateHUD[unitID].SetArmor(teammateUnit[unitID]);

        yield return new WaitForSeconds(2f);

        Debug.Log(unitID);

        battleState = BattleState.TEAMMATETURN;
        StartCoroutine(TeammateAttack());
    }
    IEnumerator RestoreWeapon(int unitID)
    {
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
