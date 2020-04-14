using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, FLED }

public class BattleSystem : MonoBehaviour
{
    public CapturePoint starter;
    Vault vault;
    
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

    public Text battleText;
    public Timer battleTextTimer;

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
        vault = FindObjectOfType<Vault>();
        state = BattleState.START;
        StartCoroutine(SetupBattle());
        
        setBattleText("Defeat the <color=red> enemies </color>!", 2);
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

        battleText.fontSize = 30;
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
        else if (state == BattleState.FLED)
        {
            for (int i = 0; i < fighters.Length; i++)
                fighters[i].currentHP = fighters[i].maxHP;
        }
        else if (state == BattleState.LOST)
        {
            for (int i = 0; i < fighters.Length; i++)
                fighters[i].currentHP = fighters[i].maxHP;
            for (int i = 0; i < Heroes.count; i++)
                Heroes.getHero(i).currentHP = (int)(Heroes.getHero(i).maxHP * 0.5);

            Vault v = FindObjectOfType<Vault>();
            v.addChances(-1);
            if (v.getChances() <= 0)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
        Heroes.ResetCooldowns();
        SceneManager.UnloadSceneAsync("TBC");
    }

    void PlayerTurn()
    {
        setAbilityNames();
        for (int i = 0; i < Heroes.count; i++)
            heroes[i].DecrementCooldowns();

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
        bool isDead = false;
        
        if (heroes[playerIndex].effects.Contains(Effect.Untargetable))
        {

        }
        else {
            enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", true);
            int damage = fighters[enemyIndex].attackDamage;
            isDead = heroes[playerIndex].TakeDamage(damage);
            setBattleText("<color=red>Enemy</color> hits <color=blue>" + heroes[playerIndex].name + "</color> for <color=red>" + damage + "</color> damage", 2);
            yield return new WaitForSeconds(1);
            enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", false);
        }

        updateHuds();
        
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
        else if (selectedAbility.requiredSelection == RequiredSelection.self)
            friendlyCurrentFighter = heroes[index];

        selectedAbility.ability.Invoke();
        selectedAbility.cooldownCurrently = selectedAbility.cooldown;
        setEnemySelectionButtonsVisible(false);
        setFriendlySelectionButtonsVisible(false);
        setSelfSelectionButtonVisible(false);
        StartCoroutine(changeTurn(2));

    }

    public void OnAbilityOneButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityOne.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + heroes[playerIndex].AbilityOne.cooldownCurrently + "</color> turns", 1);
            return;
        }
            

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityOne;

        if (heroes[playerIndex].AbilityOne.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        
    }

    

    public void OnAbilityTwoButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityTwo.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + heroes[playerIndex].AbilityTwo.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityTwo;
        
        if (heroes[playerIndex].AbilityTwo.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if(heroes[playerIndex].AbilityTwo.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
    }

    public void OnAbilityThreeButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityThree.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + heroes[playerIndex].AbilityThree.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityThree;

        if (heroes[playerIndex].AbilityThree.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if (heroes[playerIndex].AbilityThree.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
        else if (heroes[playerIndex].AbilityThree.requiredSelection == RequiredSelection.self)
            setSelfSelectionButtonVisible(true);
    }

    public void OnAbilityFourButton()
    {
        if (state != BattleState.PLAYERTURN || heroes[playerIndex].AbilityFour.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + heroes[playerIndex].AbilityFour.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = heroes[playerIndex].AbilityFour;

        if (heroes[playerIndex].AbilityFour.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if (heroes[playerIndex].AbilityFour.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
        else if (heroes[playerIndex].AbilityFour.requiredSelection == RequiredSelection.self)
            setSelfSelectionButtonVisible(true);
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

        state = BattleState.FLED;
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

    void setSelfSelectionButtonVisible(bool value)
    {
        friendlySelectButtons[playerIndex].gameObject.SetActive(value);
    }

    void setAbilityNames()
    {
        buttonTexts[0].text = heroes[playerIndex].AbilityOne.name;
        buttonTexts[1].text = heroes[playerIndex].AbilityTwo.name;
        buttonTexts[2].text = heroes[playerIndex].AbilityThree.name;
        buttonTexts[3].text = heroes[playerIndex].AbilityFour.name;
    }

    public void setBattleText(string text, int duration)
    {
        battleText.text = text;
        battleTextTimer.setTimer(duration);
    }
    
}
