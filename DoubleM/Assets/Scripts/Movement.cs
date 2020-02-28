using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Joystick joystick;
    public Rigidbody2D rb;
    public float speed;
    Vector2 movement;
    
    void Update()
    {
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
