using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public CapturePoint starter;
    public Vault vault;

    public GameObject[] playerPrefab;
    public GameObject enemyPrefab;
    private Fighter[] heroes;
    public Fighter[] fighters;
    public int playerIndex = 0;
    public int enemyIndex = 0;

    public GameObject Buttons;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    GameObject player, enemy;

    // Start is called before the first frame update
    void Start()
    {
        fighters = starter.getFighters();
        heroes = Heroes.getHeroArray();
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        FindObjectOfType<AudioManager>().Play("Background2");
    }

    IEnumerator SetupBattle()
    {
        player = Instantiate(heroes[playerIndex].prefab, playerBattleStation);
        
        enemy = Instantiate(fighters[enemyIndex].prefab, enemyBattleStation);
        enemy.GetComponent<SpriteRenderer>().flipX = fighters[enemyIndex].flipSpriteOnX;

        playerHUD.setHUD(heroes[playerIndex]);
        enemyHUD.setHUD(fighters[enemyIndex]);

        EnableButtons(false);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        float critChanceRoll = Random.value * 100;
        float damage = heroes[playerIndex].attackDamage;
        if (critChanceRoll <= heroes[playerIndex].critChance)
            damage *= 2;

        player.GetComponent<Animator>().SetBool("IsAttacking", true);
        bool isDead = fighters[enemyIndex].TakeDamage((int)damage);
        
        yield return new WaitForSeconds(1);
        player.GetComponent<Animator>().SetBool("IsAttacking", false);
        enemyHUD.SetHP(fighters[enemyIndex].currentHP);

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
        heroes[playerIndex].Heal(5);
        playerHUD.SetHP(heroes[playerIndex].currentHP);

        yield return new WaitForSeconds(2);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            if (CapturePoint.currentCapturePoint.reward == BattleReward.Attack)
                for (int i = 0; i < Heroes.count; i++)
                    Heroes.getHero(i).attackDamage += 7;

            if (CapturePoint.currentCapturePoint.reward == BattleReward.Health)
                for (int i = 0; i < Heroes.count; i++)
                {
                    Heroes.getHero(i).maxHP += 10;
                    Heroes.getHero(i).currentHP += 10;
                }

            vault.addMoney(starter.getVictoryLoot());
            CapturePoint.currentCapturePoint.Victory();
        }
        SceneManager.UnloadSceneAsync("TBC");
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
        PlaySound("Attack1");
        enemy.GetComponent<Animator>().SetBool("IsAttacking", true);
        bool isDead = heroes[playerIndex].TakeDamage(fighters[enemyIndex].attackDamage);
        
        
        yield return new WaitForSeconds(1);
        enemy.GetComponent<Animator>().SetBool("IsAttacking", false);
        playerHUD.SetHP(heroes[playerIndex].currentHP);
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
        PlaySound("Attack1");
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

    public void OnFleeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.LOST;
        EndBattle();
    }

    public void OnDefendButton()
    {
        
    }

    void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

}
