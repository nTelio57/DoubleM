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


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Attack(int amount)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
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
