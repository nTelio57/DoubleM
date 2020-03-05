using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Joystick joystick;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    Vector2 movement;
    float memoryMovementX;
    
    void Update()
    {
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;

        if (movement.x != 0)
            memoryMovementX = movement.x;

        animator.SetFloat("Speed", joystick.Direction.sqrMagnitude);
        animator.SetFloat("Horizontal", memoryMovementX);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
