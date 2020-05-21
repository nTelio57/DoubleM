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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        attackPointXPos = attackPoint.localPosition.x;
        attackPointXNeg = -attackPoint.localPosition.x;
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

    void updateAttackPointHorrizontal()
    {
        if(JoystickManager.currentJoystick.Horizontal != 0)
            memoryJoystickHorizontal = JoystickManager.currentJoystick.Horizontal;

        if (memoryJoystickHorizontal > 0)
            newPosition.x = attackPointXPos;
        else
            newPosition.x = attackPointXNeg;
        
        newPosition.y = attackPoint.localPosition.y;
        newPosition.z = attackPoint.localPosition.z;

        attackPoint.localPosition = newPosition;
    }

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

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
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
        Debug.Log(amount + ":" + currentHealth);
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
