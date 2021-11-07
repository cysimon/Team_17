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

    /*Subscription<clickEvent> subscriptionClick;*/
    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.START;
        StartCoroutine(SetupBattle());
        /*subscriptionClick = EventBus.Subscribe<clickEvent>(ClickListener);*/
    }

    /*void ClickListener(clickEvent input)
    {
        if (input.clickEvents == ClickEvents.ARMOR) OnClickArmor(input.thisUnit);
        else if (input.clickEvents == ClickEvents.WEAPON) OnClickWeapon(input.thisUnit);
    }*/

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponentInChildren<Unit>();

        for(int i = 0; i < 3; i++)
        {
            GameObject[] teamGO = new GameObject[3];
            teamGO[i] = Instantiate(teammatePrefab, teammateBattleStations[i]);
            teammateUnit[i] = teamGO[i].GetComponentInChildren<Unit>();
            teammateUnit[i].unitId = i;
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
            if (isTeamDeadArmor[i]) continue;
            int rand = Random.Range(1, 2);

            if (rand == 1)
            {
                isEnemyDeadArmor = enemyUnit.TakeDamageArmor(teammateUnit[i].unitDamage);
                enemyHUD.SetArmor(enemyUnit);
            }
            else if (rand == 2)
            {
                isEnemyDeadWeapon = enemyUnit.TakeDamageWeapon(teammateUnit[i].unitDamage);
                enemyHUD.SetWeapon(enemyUnit);
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
        int rand1;
        while (true)
        {
            rand1 = Random.Range(1, 3);
            if (!isTeamDeadArmor[rand1]) break;
        }
        
        int rand2 = Random.Range(1, 2);

        if (rand2 == 1)
        {
            isTeamDeadArmor[rand1] = teammateUnit[rand1].TakeDamageArmor(enemyUnit.unitDamage);
            teammateHUD[rand1].SetArmor(teammateUnit[rand1]);
        }
        else if (rand2 == 2)
        {
            isTeamDeadWeapon[rand1] = teammateUnit[rand1].TakeDamageWeapon(enemyUnit.unitDamage);
            teammateHUD[rand1].SetWeapon(teammateUnit[rand1]);
        }

        yield return new WaitForSeconds(2f);

        if (isTeamDeadArmor[0] && isTeamDeadArmor[1] && isTeamDeadArmor[2])
        {
            battleState = BattleState.LOST;
            EndBattle();
        }
        else
        {
            if (isTeamDeadArmor[rand1])
            {
                Debug.Log("Teammate " + rand1 + " dead!");
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
