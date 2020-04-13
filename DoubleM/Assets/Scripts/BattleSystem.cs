using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    public CapturePoint starter;
    public Vault vault;
    
    private Fighter[] heroes;
    public Fighter[] fighters;
    public int playerIndex = 0;
    public int enemyIndex = 0;

    public GameObject Buttons;
    public Text[] buttonTexts;
    public Button[] friendlySelectButtons;
    public Button[] enemySelectButtons;

    public Transform[] heroBattleStation;
    public Transform[] enemyBattleStation;

    public Text dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public BattleState state;

    GameObject[] player, enemy;

    public static Fighter enemyCurrentFighter, friendlyCurrentFighter;
    public static GameObject enemyCurrentPrefab, friendlyCurrentPrefab;

    public static Ability selectedAbility;

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
        player = new GameObject[Heroes.count];
        enemy = new GameObject[fighters.Length];
        
         for(int i = 0; i < Heroes.count; i++)
           player[i] = Instantiate(heroes[i].prefab, heroBattleStation[i]);
         
        for (int i = 0; i < fighters.Length; i++)
        {
            enemy[i] = Instantiate(fighters[i].prefab, enemyBattleStation[i]);
            enemy[i].GetComponent<SpriteRenderer>().flipX = fighters[i].flipSpriteOnX;
        }
        

        playerHUD.setHUD(heroes[playerIndex]);
        enemyHUD.setHUD(fighters[enemyIndex]);

        EnableButtons(false);

        yield return new WaitForSeconds(2);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
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
        setAbilityNames();
        for (int i = 0; i < Heroes.count; i++)
        {
            Fighter f = heroes[i];
            if(f.AbilityOne.cooldownCurrently > 0)
                f.AbilityOne.cooldownCurrently--;

            if (f.AbilityTwo.cooldownCurrently > 0)
                f.AbilityTwo.cooldownCurrently--;

            if (f.AbilityThree.cooldownCurrently > 0)
                f.AbilityThree.cooldownCurrently--;

            if (f.AbilityFour.cooldownCurrently > 0)
                f.AbilityFour.cooldownCurrently--;
        }

        setCurrentFighters();
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
        enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", true);
        bool isDead = heroes[playerIndex].TakeDamage(fighters[enemyIndex].attackDamage);

        updateHuds();

        yield return new WaitForSeconds(1);
        enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", false);
        
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

    public void invokeAbility(int index)
    {
        if (selectedAbility.requiredSelection == RequiredSelection.enemies)
            enemyCurrentFighter = fighters[index];
        else if (selectedAbility.requiredSelection == RequiredSelection.friendly)
            friendlyCurrentFighter = heroes[index];

        selectedAbility.ability.Invoke();
        setEnemySelectionButtonsVisible(false);
        setFriendlySelectionButtonsVisible(false);
        StartCoroutine(changeTurn(2));

    }

    public void OnAbilityOneButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityOne.cooldownCurrently != 0)
            return;

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityOne;
        if (heroes[playerIndex].AbilityOne.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        
    }

    

    public void OnAbilityTwoButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityTwo.cooldownCurrently != 0)
            return;

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityTwo;
        Debug.Log(heroes[playerIndex].AbilityTwo.requiredSelection + "  " + RequiredSelection.enemies);
        Debug.Log(heroes[playerIndex].AbilityTwo.requiredSelection == RequiredSelection.enemies);
        if (heroes[playerIndex].AbilityTwo.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if(heroes[playerIndex].AbilityTwo.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
    }

    void setCurrentFighters()
    {
        friendlyCurrentFighter = heroes[playerIndex];
        enemyCurrentFighter = fighters[enemyIndex];

        friendlyCurrentPrefab = player[playerIndex];
        enemyCurrentPrefab = enemy[enemyIndex];
    }

    public void OnFleeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.LOST;
        EndBattle();
    }

    IEnumerator changeTurn(int waitTime)
    {
        updateHuds();
        yield return new WaitForSeconds(waitTime);
        
        if (isEnemyDefeated())
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    void updateHuds()
    {
        playerHUD.SetHP(heroes[playerIndex].currentHP);
        enemyHUD.SetHP(fighters[enemyIndex].currentHP);
    }

    void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    bool isEnemyDefeated()
    {
        int sum = 0;
        for (int i = 0; i < fighters.Length; i++)
        {
            if (fighters[i].currentHP <= 0)
                sum++;
        }
        return sum == fighters.Length;
    }

    void setEnemySelectionButtonsVisible(bool value)
    {
        for (int i = 0; i < fighters.Length; i++)
        {
            if (value && fighters[i].currentHP <= 0)
                continue;
            else
                enemySelectButtons[i].gameObject.SetActive(value);
        }
    }

    void setFriendlySelectionButtonsVisible(bool value)
    {
        for (int i = 0; i < Heroes.count; i++)
        {
            if (value && heroes[i].currentHP <= 0)
                continue;
            else
                friendlySelectButtons[i].gameObject.SetActive(value);
        }
    }

    void setAbilityNames()
    {
        buttonTexts[0].text = heroes[playerIndex].AbilityOne.name;
        buttonTexts[1].text = heroes[playerIndex].AbilityTwo.name;
        buttonTexts[2].text = heroes[playerIndex].AbilityThree.name;
        buttonTexts[3].text = heroes[playerIndex].AbilityFour.name;
    }

}
