using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [Header("Stats")]
    public float attackRange = 0.5f;
    public int health;
    public int currentHealth;

    float attackPointXPos;
    float attackPointXNeg;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        attackPointXPos = attackPoint.localPosition.x;
        attackPointXNeg = -attackPoint.localPosition.x;
    }

    // Update is called once per frame
    void Update()
    {
        updateAttackPointHorrizontal();
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

    public void Attack(int amount)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            if(enemy.isTrigger)
                enemy.GetComponent<Combat>().takeDamage(amount);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        
        if (currentHealth <= 0)
        {
            if (gameObject.tag == "Player")
            {
                Vault.addChances(-1);
                currentHealth = health;
            }
            if (gameObject.tag == "Enemy")
            {
                Destroy(gameObject);
            }
        }
    }
}
