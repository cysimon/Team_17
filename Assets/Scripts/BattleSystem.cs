using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Unit playerUnit;
    Unit enemyUnit;
    Unit[] teammateUnit = new Unit[3];

    // Start is called before the first frame update
    void Start()
    {
        battleState = BattleState.START;
        SetupBattle();
    }

    void SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        for(int i = 0; i < 3; i++)
        {
            GameObject[] teamGO = new GameObject[3];
            teamGO[i] = Instantiate(teammatePrefab, teammateBattleStations[i]);
            teammateUnit[i] = playerGO.GetComponent<Unit>();
        }
    }
}
