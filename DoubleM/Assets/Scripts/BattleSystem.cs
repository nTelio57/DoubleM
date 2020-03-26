using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public StartTBC starter;
    public Vault vault;

    public GameObject[] playerPrefab;
    public GameObject enemyPrefab;
    public Fighter[] fighters;
    public int playerIndex = 0;
    public int enemyIndex = 0;

    public GameObject Buttons;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    GameObject player, enemy;

    // Start is called before the first frame update
    void Start()
    {
        fighters = starter.getFighters();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        player = Instantiate(playerPrefab[playerIndex], playerBattleStation);
        playerUnit = player.GetComponent<Unit>();

        //GameObject enemy = Instantiate(enemyPrefab, enemyBattleStation);
        enemy = Instantiate(fighters[enemyIndex].enemyPrefab, enemyBattleStation);
       // enemy.GetComponent<SpriteRenderer>().sprite = fighters[enemyIndex].sprite;
        enemy.GetComponent<SpriteRenderer>().flipX = fighters[enemyIndex].flipSpriteOnX;
        enemyUnit = enemy.GetComponent<Unit>();
        enemyUnit.unitName = fighters[enemyIndex].name;
        enemyUnit.maxHP = fighters[enemyIndex].maxHP;
        enemyUnit.currentHP = fighters[enemyIndex].maxHP;
        enemyUnit.damage = fighters[enemyIndex].attackDamage;

        playerHUD.setHUD(playerUnit);
        enemyHUD.setHUD(enemyUnit);

        EnableButtons(false);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        player.GetComponent<Animator>().SetBool("IsAttacking", true);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        
        
        yield return new WaitForSeconds(1);
        player.GetComponent<Animator>().SetBool("IsAttacking", false);
        enemyHUD.SetHP(enemyUnit.currentHP);

        yield return new WaitForSeconds(1);
        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);
        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(2);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            vault.addMoney(starter.getVictoryLoot());
        }
        SceneManager.UnloadSceneAsync("TBC");
        if (StartTBC.nextScenePass != "")
            SceneManager.LoadScene(StartTBC.nextScenePass, LoadSceneMode.Single);
    }

    void PlayerTurn()
    {
        EnableButtons(true);
    }

    void EnableButtons(bool value)
    {
        Buttons.SetActive(value);
    }

    IEnumerator EnemyTurn()
    {
        EnableButtons(false);
        enemy.GetComponent<Animator>().SetBool("IsAttacking", true);
        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);
        
        
        yield return new WaitForSeconds(1);
        enemy.GetComponent<Animator>().SetBool("IsAttacking", false);
        playerHUD.SetHP(playerUnit.currentHP);
        yield return new WaitForSeconds(1);
        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        EnableButtons(false);
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        EnableButtons(false);
        StartCoroutine(PlayerHeal());
    }

}
