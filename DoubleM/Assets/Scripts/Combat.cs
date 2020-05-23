using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [Header("Stats")]
    public int damage;
    public float attackRange = 0.5f;
    Vector2 attackRangeVector;
    public float attackSpeed = 1;
    public int maxHealth;
    public int currentHealth;
    public float regenerationSpeed = 10;

    [HideInInspector]
    public bool isAbleToAttack;
    public bool attackSpeedTimerActive;

    float attackPointXPos;
    float attackPointXNeg;
    bool isAttacking;

    float timerRegeneration;
    [HideInInspector]
    public float timerAttackSpeed;
    [HideInInspector]
    public int direction = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(!SaveOptions.isGameSaved)
            currentHealth = maxHealth;
        attackPointXPos = attackPoint.localPosition.x;
        attackPointXNeg = -attackPoint.localPosition.x;
        attackRangeVector.x = attackRange * 2;
        attackRangeVector.y = attackRange * 4;
    }

    // Update is called once per frame
    void Update()
    {
        updateAttackPointHorrizontal();
        regeneration();

        if (attackSpeedTimerActive)
        {
            timerAttackSpeed += Time.deltaTime;
        }

        if (timerAttackSpeed >= attackSpeed)
            isAbleToAttack = true;
        else
            isAbleToAttack = false;
    }

    Vector3 newPosition;
    float memoryJoystickHorizontal;
    float memoryX;

    void updateAttackPointHorrizontal()
    {
        if (transform.position.x > memoryX)
            direction = 1;
        else if (transform.position.x < memoryX)
            direction = -1;

        memoryX = transform.position.x;

        if (direction > 0)
            newPosition.x = attackPointXPos;
        else
            newPosition.x = attackPointXNeg;
        
        newPosition.y = attackPoint.localPosition.y;
        newPosition.z = attackPoint.localPosition.z;

        attackPoint.localPosition = newPosition;
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRangeVector, 1, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.isTrigger)
                enemy.GetComponent<Combat>().takeDamage(damage);
        }

        timerAttackSpeed = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireCube(attackPoint.position, new Vector2(attackRange*2, attackRange*4));
    }

    public void regeneration()
    {
        if (currentHealth < maxHealth)
        {
            timerRegeneration += Time.deltaTime;
            if (timerRegeneration >= regenerationSpeed)
            {
                currentHealth += 1;
                timerRegeneration = 0;
            }
        }
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            if (gameObject.tag == "Player")
            {
                Vault.addChances(-1);
                currentHealth = maxHealth;
                FindObjectOfType<CheckpointManager>().Respawn();
            }
            if (gameObject.tag == "Enemy")
            {
                GameTracking.enemySlainCount += 1;
                GetComponentInParent<Spawner>().entitiesCount--;
                Destroy(gameObject);
            }
        }
    }
}
