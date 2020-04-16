﻿using System.Collections;
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
    
    //private Fighter[] heroes;
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
        //heroes = Heroes.getHeroArray();
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
        setBattleStationIndexes();

         for(int i = 0; i < Heroes.count; i++)
           player[i] = Instantiate(Heroes.getHero(i).prefab, heroBattleStation[i]);
         
        for (int i = 0; i < fighters.Length; i++)
        {
            enemy[i] = Instantiate(fighters[i].prefab, enemyBattleStation[i]);
            enemy[i].GetComponent<SpriteRenderer>().flipX = fighters[i].flipSpriteOnX;
        }
        

        playerHUD.setHUD(Heroes.getHero(0));
        enemyHUD.setHUD(fighters[enemyIndex]);

        EnableButtons(false);

        yield return new WaitForSeconds(2);

        setBattleStationIndexes();
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
        setCurrentFighters();
        setAbilityNames();
        //for (int i = 0; i < Heroes.count; i++)
        //    Heroes.getHero(i).DecrementCooldowns();
        //Heroes.getHero(playerIndex).DecrementCooldowns();
        
        for (int i = 0; i < Heroes.count; i++)
        {
            Debug.Log(Heroes.getHero(i).battleStationIndex+"");
        }


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

        int targetIndex = targetIndexForEnemy();
        
        if (targetIndex == -1)
        {
            
        }
        else {
            enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", true);
            int damage = fighters[enemyIndex].attackDamage;
            isDead = Heroes.getHero(targetIndex).TakeDamage(damage);
            setBattleText("<color=red>Enemy</color> hits <color=blue>" + Heroes.getHero(targetIndex).name + "</color> for <color=red>" + damage + "</color> damage", 2);
            yield return new WaitForSeconds(1);
            enemy[enemyIndex].GetComponent<Animator>().SetBool("IsAttacking", false);
        }

        updateHuds();

        if (enemyIndex < fighters.Length - 1)
            enemyIndex++;
        else
            enemyIndex = 0;

        removeDeadheroes();

        yield return new WaitForSeconds(1);
        if (isPlayerDefeated())
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
            friendlyCurrentFighter = Heroes.getHero(index);
        else if (selectedAbility.requiredSelection == RequiredSelection.self)
            friendlyCurrentFighter = Heroes.getHero(index);

        selectedAbility.ability.Invoke();
        selectedAbility.cooldownCurrently = selectedAbility.cooldown;
        setEnemySelectionButtonsVisible(false);
        setFriendlySelectionButtonsVisible(false);
        setSelfSelectionButtonVisible(false);
        StartCoroutine(changeTurn(2));

    }

    public void OnAbilityOneButton()
    {
        if (state != BattleState.PLAYERTURN || Heroes.getHero(playerIndex).AbilityOne.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" +Heroes.getHero(playerIndex).AbilityOne.cooldownCurrently + "</color> turns", 1);
            return;
        }
            

        EnableButtons(false);
        selectedAbility = Heroes.getHero(playerIndex).AbilityOne;

        if (Heroes.getHero(playerIndex).AbilityOne.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        
    }

    

    public void OnAbilityTwoButton()
    {
        if (state != BattleState.PLAYERTURN || Heroes.getHero(playerIndex).AbilityTwo.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityTwo.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = Heroes.getHero(playerIndex).AbilityTwo;

        if (Heroes.getHero(playerIndex).AbilityTwo.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if(Heroes.getHero(playerIndex).AbilityTwo.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
        else if (Heroes.getHero(playerIndex).AbilityTwo.requiredSelection == RequiredSelection.self)
            setSelfSelectionButtonVisible(true);
    }

    public void OnAbilityThreeButton()
    {
        if (state != BattleState.PLAYERTURN || Heroes.getHero(playerIndex).AbilityThree.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityThree.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = Heroes.getHero(playerIndex).AbilityThree;

        if (Heroes.getHero(playerIndex).AbilityThree.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if (Heroes.getHero(playerIndex).AbilityThree.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
        else if (Heroes.getHero(playerIndex).AbilityThree.requiredSelection == RequiredSelection.self)
            setSelfSelectionButtonVisible(true);
    }

    public void OnAbilityFourButton()
    {
        if (state != BattleState.PLAYERTURN || Heroes.getHero(playerIndex).AbilityFour.cooldownCurrently != 0)
        {
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityFour.cooldownCurrently + "</color> turns", 1);
            return;
        }

        EnableButtons(false);
        selectedAbility = Heroes.getHero(playerIndex).AbilityFour;

        if (Heroes.getHero(playerIndex).AbilityFour.requiredSelection == RequiredSelection.enemies)
            setEnemySelectionButtonsVisible(true);
        else if (Heroes.getHero(playerIndex).AbilityFour.requiredSelection == RequiredSelection.friendly)
            setFriendlySelectionButtonsVisible(true);
        else if (Heroes.getHero(playerIndex).AbilityFour.requiredSelection == RequiredSelection.self)
            setSelfSelectionButtonVisible(true);
    }

    void setCurrentFighters()
    {
        friendlyCurrentFighter = Heroes.getHero(playerIndex);
        enemyCurrentFighter = fighters[enemyIndex];

        friendlyCurrentPrefab = player[Heroes.getHero(playerIndex).battleStationIndex];
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

        playerIndex = nextHeroIndex(playerIndex);

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

    int nextHeroIndex(int currentIndex)
    {
        if (currentIndex < Heroes.count - 1)
            currentIndex++;
        else
            currentIndex = 0;

        if (Heroes.getHero(currentIndex).currentHP > 0)
            return currentIndex;

        return nextHeroIndex(currentIndex);
    }

    void updateHuds()
    {
        playerHUD.SetHP(Heroes.getHero(playerIndex).currentHP);
        enemyHUD.SetHP(fighters[enemyIndex].currentHP);

        printInfo();
    }

    void PlaySound(string name)
    {
        FindObjectOfType<AudioManager>().Play(name);
    }

    bool isPlayerDefeated()
    {
        int sum = 0;
        for (int i = 0; i < Heroes.count; i++)
        {
            if (Heroes.getHero(i).currentHP <= 0)
                sum++;
        }
        return sum == Heroes.count;
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
            {
                enemyBattleStation[i].GetComponent<SpriteRenderer>().enabled = value;
                enemySelectButtons[i].gameObject.SetActive(value);
            }

        }
    }

    void setFriendlySelectionButtonsVisible(bool value)
    {
        for (int i = 0; i < Heroes.count; i++)
        {
            if (value && Heroes.getHero(i).currentHP <= 0)
                continue;
            else
            {
                //friendlySelectButtons[i].gameObject.SetActive(value);
                //heroBattleStation[i].GetComponent<SpriteRenderer>().enabled = value;
                Debug.Log("Index: " + Heroes.getHero(i).battleStationIndex);
                friendlySelectButtons[Heroes.getHero(i).battleStationIndex].gameObject.SetActive(value);
                heroBattleStation[Heroes.getHero(i).battleStationIndex].GetComponent<SpriteRenderer>().enabled = value;
            }
                
        }
    }

    void setSelfSelectionButtonVisible(bool value)
    {
        // friendlySelectButtons[playerIndex].gameObject.SetActive(value);
        //heroBattleStation[playerIndex].GetComponent<SpriteRenderer>().enabled = value;
        Debug.Log(playerIndex + " : " + Heroes.getHero(playerIndex).battleStationIndex);
        friendlySelectButtons[Heroes.getHero(playerIndex).battleStationIndex].gameObject.SetActive(value);
        heroBattleStation[Heroes.getHero(playerIndex).battleStationIndex].GetComponent<SpriteRenderer>().enabled = value;
    }

    void setAbilityNames()
    {
        buttonTexts[0].text = Heroes.getHero(playerIndex).AbilityOne.name;
        buttonTexts[1].text = Heroes.getHero(playerIndex).AbilityTwo.name;
        buttonTexts[2].text = Heroes.getHero(playerIndex).AbilityThree.name;
        buttonTexts[3].text = Heroes.getHero(playerIndex).AbilityFour.name;
    }

    public void setBattleText(string text, int duration)
    {
        battleText.text = text;
        battleTextTimer.setTimer(duration);
    }


    int targetIndexForEnemy()
    {
        int index = -1;
        int leastHp = int.MaxValue;
        for (int i = 0; i < Heroes.count; i++)
        {
            if (Heroes.getHero(i).currentHP <= leastHp && Heroes.getHero(i).currentHP > 0 && !Heroes.getHero(i).effects.Contains(Effect.Untargetable))
            {
                leastHp = Heroes.getHero(i).currentHP;
                index = i;
            }
        }
        return index;
    }

    void printInfo()
    {
        Debug.Log("Heroes");
        for (int i = 0; i < Heroes.count; i++)
        {
            Debug.Log(i + " Health: " + Heroes.getHero(i).currentHP);
            Debug.Log(i + " Station: " + Heroes.getHero(i).battleStationIndex);
            Debug.Log(i + " Cooldown 1: " + Heroes.getHero(i).AbilityOne.cooldownCurrently);
            Debug.Log(i + " Cooldown 2: " + Heroes.getHero(i).AbilityTwo.cooldownCurrently);
            Debug.Log(i + " Cooldown 3: " + Heroes.getHero(i).AbilityThree.cooldownCurrently);
            Debug.Log(i + " Cooldown 4: " + Heroes.getHero(i).AbilityFour.cooldownCurrently);
        }
    }

    void removeDeadheroes()
    {
        for (int i = 0; i < Heroes.count; i++)
        {
            if (Heroes.getHero(i).currentHP <= 0)
            {
                Heroes.RemoveHero(i);
                Destroy(player[i]);
            }
                
        }
    }

    void setBattleStationIndexes()
    {
        Debug.Log("Count " + Heroes.count);
        for (int i = 0; i < Heroes.count; i++)
        {
            Heroes.getHero(i).setBattleStation(i);
        }
    }

}
