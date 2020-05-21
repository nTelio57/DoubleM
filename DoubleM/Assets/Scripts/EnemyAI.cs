using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    //public float attackSpeed;
    //float attackTimer;
    public float speed = 10;
    public float nextWaypointDistance = 3f;
    public float rangeFromTarget = 0;
    public float detectionRange;
    public Combat combat;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        InvokeRepeating("UpdatePath", 0f, .5f);

        //attackTimer = 0;
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        if (path == null)
            return;

        float distanceToTarget = Vector2.Distance(rb.position, target.position);

        /*if (distanceToTarget <= rangeFromTarget)
        {
            Debug.Log("In range");
            reachedEndOfPath = true;
            attackTimer += Time.deltaTime;
            Combat();
            animator.SetBool("isWalking", false);
            return;
        }*/

        if (currentWaypoint >= path.vectorPath.Count || distanceToTarget <= rangeFromTarget)
        {
            reachedEndOfPath = true;
            combat.attackSpeedTimerActive = true;
            //attackTimer += Time.deltaTime;
            Combat();
            
            return;
        }
        else
        {
            combat.attackSpeedTimerActive = false;
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = rb.mass * direction * speed * Time.deltaTime;
        Vector2 positionToAdd = rb.position + direction * speed * Time.deltaTime;

        //if (direction.x > 0)
        if (target.position.x > transform.position.x)
            GetComponent<SpriteRenderer>().flipX = false;
        else
            GetComponent<SpriteRenderer>().flipX = true;

        if (!GameStatus.isMainLevelPaused && distanceToTarget <= detectionRange && !reachedEndOfPath)
        {
            rb.MovePosition(positionToAdd);
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void Combat()
    {
        if (combat.isAbleToAttack)
        {
            animator.SetTrigger("Attack");
            combat.Attack();
            animator.SetBool("isWalking", false);
        }
    }
}
