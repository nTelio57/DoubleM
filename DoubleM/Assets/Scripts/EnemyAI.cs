﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float speed = 200;
    public float nextWaypointDistance = 3f;
    public float rangeFromTarget = 0;
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

        if (currentWaypoint >= path.vectorPath.Count || distanceToTarget <= rangeFromTarget)
        {
            reachedEndOfPath = true;
            Combat();
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = rb.mass * direction * speed * Time.deltaTime;
        Vector2 positionToAdd = rb.position + direction * speed * Time.deltaTime;
        
        if(!GameStatus.isMainLevelPaused)
            rb.MovePosition(positionToAdd);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void Combat()
    {
        combat.Attack();
    }
}