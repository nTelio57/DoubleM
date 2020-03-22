using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Joystick joystick;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    Vector2 movement;
    float memoryMovementX;

    void Start()
    {
        joystick = JoystickManager.currentJoystick;
    }

    void Update()
    {
        movement.x = JoystickManager.currentJoystick.Horizontal;
        movement.y = JoystickManager.currentJoystick.Vertical;

        if (movement.x != 0)
            memoryMovementX = movement.x;

        animator.SetFloat("Speed", JoystickManager.currentJoystick.Direction.sqrMagnitude);
        animator.SetFloat("Horizontal", memoryMovementX);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
