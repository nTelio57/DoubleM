using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, FLED }

public class BattleSystem : MonoBehaviour
{
    Vault vault;
    
    //private Fighter[] heroes;
    public Fighter[] fighters;
    public int playerIndex = 0;
    public int enemyIndex = 0;

    public GameObject Buttons;
    public Text[] buttonTexts;
    public Button[] friendlySelectButtons;
    public Button[] enemySelectButtons;

    [Header("Info panel management")]
    public GameObject infoPanel;
    public InfoPanelManager infoPanelManager;

    public Transform[] heroBattleStation;
    public Transform[] enemyBattleStation;

    public Text battleText;
    public Timer battleTextTimer;

    public BattleState state;

    GameObject[] player, enemies;

    public static Fighter enemyCurrentFighter, friendlyCurrentFighter;
    public static GameObject enemyCurrentPrefab, friendlyCurrentPrefab;

    public static Ability selectedAbility;

    // Start is called before the first frame update
    void Start()
    {
        fighters = CapturePoint.getFighters();
        InitEnemyFighters();
        //heroes = Heroes.getHeroArray();
        vault = FindObjectOfType<Vault>();
        state = BattleState.START;
        GameStatus.isMainLevelPaused = true;
        StartCoroutine(SetupBattle());
        
        setBattleText("Defeat the <color=red> enemies </color>!", 2);
        FindObjectOfType<AudioManager>().Play("Background2");
        
    }

    void InitEnemyFighters()
    {
        for (int i = 0; i < fighters.Length; i++)
        {
            fighters[i] = new Fighter(fighters[i]);
        }
    }

    IEnumerator SetupBattle()
    {
        player = new GameObject[Heroes.count];
        enemies = new GameObject[fighters.Length];
        setBattleStationIndexes();

        for(int i = 0; i < Heroes.count; i++)
        {
            player[i] = Instantiate(Heroes.getHero(i).prefab, heroBattleStation[i]);
            player[i].GetComponent<SpriteRenderer>().flipX = Heroes.getHero(i).flipSpriteOnX;
        }
           
         
        for (int i = 0; i < fighters.Length; i++)
        {
            enemies[i] = Instantiate(fighters[i].prefab, enemyBattleStation[i]);
            enemies[i].GetComponent<SpriteRenderer>().flipX = fighters[i].flipSpriteOnX;
        }

        setupHealthbars();

        EnableButtons(false);

        yield return new WaitForSeconds(2);
        
        setBattleStationIndexes();
        battleText.fontSize = 30;
        
        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void setupHealthbars()
    {
        for (int i = 0; i < Heroes.count; i++)
        {
            Heroes.getHero(i).healthbar = player[i].GetComponentInChildren<Healthbar>();
            Heroes.getHero(i).healthbar.SetHealth(Heroes.getHero(i).currentHP);
        }

        for (int i = 0; i < fighters.Length; i++)
        {
            fighters[i].healthbar = enemies[i].GetComponentInChildren<Healthbar>();
            fighters[i].healthbar.SetMaxHealth(fighters[i].maxHP);
        }

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
            Vault.addMoney(CapturePoint.getVictoryLoot());
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
            
            Vault.addChances(-1);
            if (Vault.getChances() <= 0)
                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            FindObjectOfType<CheckpointManager>().Respawn();
        }
        GameStatus.isMainLevelPaused = false;
        Heroes.ResetCooldowns();
        SceneManager.UnloadSceneAsync(CapturePoint.TBCScenePass);
    }

    void PlayerTurn()
    {
        setCurrentFighters();
        setAbilityNames();

        //friendlyCurrentFighter.DecrementCooldowns();
        friendlyCurrentFighter.StartOfTurn();
        EnableButtons(true);
    }

    void EnableButtons(bool value)
    {
        Buttons.SetActive(value);
        
    }

    IEnumerator EnemyTurn()
    {
        enemyCurrentFighter.StartOfTurn();
        EnableButtons(false);
        PlaySound("Attack1");
        bool isDead = false;

        int targetIndex = targetIndexForEnemy();
        
        if (targetIndex == -1)
        {
            
        }
        else {
            enemies[enemyIndex].GetComponent<Animator>().SetTrigger("Attack");
            int damage = fighters[enemyIndex].attackDamage;
            isDead = Heroes.getHero(targetIndex).TakeDamage(damage);
            setBattleText("<color=red>Enemy</color> hits <color=blue>" + Heroes.getHero(targetIndex).name + "</color> for <color=red>" + damage + "</color> damage", 2);
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
        enemyCurrentFighter.EndOfTurn();
        enemyCurrentFighter.DecrementCooldowns();
        removeDeadheroes();
        removeDeadFighters();

        //enemyIndex = nextFighterIndex(enemyIndex);

        yield return new WaitForSeconds(1);
        
        if (isPlayerDefeated())
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else if (isEnemyDefeated())
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
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
            setBattleText("It's on cooldown for <color=blue>" +Heroes.getHero(playerIndex).AbilityOne.cooldownCurrently + "</color> turns", 1.7f);
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
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityTwo.cooldownCurrently + "</color> turns", 1.7f);
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
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityThree.cooldownCurrently + "</color> turns", 1.7f);
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
            setBattleText("It's on cooldown for <color=blue>" + Heroes.getHero(playerIndex).AbilityFour.cooldownCurrently + "</color> turns", 1.7f);
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
        enemyCurrentPrefab = enemies[fighters[enemyIndex].battleStationIndex];
    }

    public void OnFleeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        state = BattleState.FLED;
        EndBattle();
    }

    public void OnInfoButton()
    {
        infoPanelManager.setTextfields(friendlyCurrentFighter);
        infoPanel.SetActive(true);
    }

    public void OnInfoBackButton()
    {
        infoPanel.SetActive(false);
    }

    IEnumerator changeTurn(int waitTime)
    {
        yield return new WaitForSeconds(1);
        friendlyCurrentFighter.EndOfTurn();

        removeDeadheroes();
        removeDeadFighters();

        
        playerIndex = nextHeroIndex(playerIndex);

        yield return new WaitForSeconds(waitTime);
        
        if (isEnemyDefeated())
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            enemyIndex = nextFighterIndex(enemyIndex);
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

    int nextFighterIndex(int currentIndex)
    {
        if (currentIndex < fighters.Length - 1)
            currentIndex++;
        else
            currentIndex = 0;

        if (fighters[currentIndex].currentHP > 0)
            return currentIndex;

        return nextFighterIndex(currentIndex);
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
                enemyBattleStation[i].GetComponentInChildren<SpriteRenderer>().enabled = value;
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
                friendlySelectButtons[Heroes.getHero(i).battleStationIndex].gameObject.SetActive(value);
                heroBattleStation[Heroes.getHero(i).battleStationIndex].GetComponentInChildren<SpriteRenderer>().enabled = value;
            }
                
        }
    }

    void setSelfSelectionButtonVisible(bool value)
    {
        friendlySelectButtons[Heroes.getHero(playerIndex).battleStationIndex].gameObject.SetActive(value);
        heroBattleStation[Heroes.getHero(playerIndex).battleStationIndex].GetComponentInChildren<SpriteRenderer>().enabled = value;
    }

    void setAbilityNames()
    {
        buttonTexts[0].text = Heroes.getHero(playerIndex).AbilityOne.name;
        buttonTexts[1].text = Heroes.getHero(playerIndex).AbilityTwo.name;
        buttonTexts[2].text = Heroes.getHero(playerIndex).AbilityThree.name;
        buttonTexts[3].text = Heroes.getHero(playerIndex).AbilityFour.name;
    }

    public void setBattleText(string text, float duration)
    {
        battleText.text = text;
        battleTextTimer.setTimer(duration);
    }


    int targetIndexForEnemy()
    {
        int index = -1;
        int leastHp = int.MaxValue;
        Fighter h;
        for (int i = 0; i < Heroes.count; i++)
        {
            h = Heroes.getHero(i);
            if (h.currentHP <= leastHp && h.currentHP > 0 && h.GetEffect(Effect.Untargetable.name) != null && !h.GetEffect(Effect.Untargetable.name).isActive)
            {
                leastHp = Heroes.getHero(i).currentHP;
                index = i;
            }
        }
        return index;
    }

    void EndOfTurn()
    {
        enemyCurrentFighter.EndOfTurn();
        friendlyCurrentFighter.EndOfTurn();

        /*for (int i = 0; i < fighters.Length; i++)
        {
            fighters[i].EndOfTurn();
        }

        for (int i = 0; i < Heroes.count; i++)
        {
            Heroes.getHero(i).EndOfTurn();
        }*/
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

    void removeDeadFighters()
    {
        for (int i = 0; i < fighters.Length; i++)
        {
            if (fighters[i].currentHP <= 0)
            {
                Destroy(enemies[i]);
            }
        }
    }

    void setBattleStationIndexes()
    {
        for (int i = 0; i < Heroes.count; i++)
        {
            Heroes.getHero(i).setBattleStation(i);
        }

        for (int i = 0; i < fighters.Length; i++)
        {
            fighters[i].setBattleStation(i);
        }
    }

    public GameObject[] getAllEnemiesArray()
    {
        return enemies;
    }

    public Fighter[] getAllFightersArray()
    {
        return fighters;
    }
}
