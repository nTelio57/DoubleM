using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Joystick joystick;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float runSpeed;
    public float maxStamina;
    public float staminaGainedOnRest;
    public float staminaLostOnRun;
    [Range(0.9f,1)]
    public float distanceFromCenterToRun = 0.95f;
    Vector2 movement;
    float memoryMovementX;
    bool isRunning;
    float stamina;
    bool resetNeeded;
    float timer = 0;

    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Background1");
        joystick = JoystickManager.currentJoystick;
        stamina = maxStamina;
        resetNeeded = false;
    }

    void Update()
    {
        timer += Time.deltaTime;
        movement.x = JoystickManager.currentJoystick.Horizontal;
        movement.y = JoystickManager.currentJoystick.Vertical;

        if (movement.x != 0)
        {
            memoryMovementX = movement.x;
        }

        if (movement.x == 0 && movement.y == 0)
        {
            resetNeeded = false;
        }

        if (JoystickManager.currentJoystick.Direction.sqrMagnitude >= distanceFromCenterToRun && stamina > 0 && !resetNeeded)
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else if (JoystickManager.currentJoystick.Direction.sqrMagnitude >= distanceFromCenterToRun && stamina == 0)
        {
            resetNeeded = true;
            isRunning = false;
            animator.SetBool("isRunning", false);
        }
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }

        if (memoryMovementX < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else
           GetComponent<SpriteRenderer>().flipX = false;

        if (timer >= 1)
        {
            if (isRunning)
                stamina -= staminaLostOnRun;
            if (!isRunning && stamina < maxStamina)
                stamina += staminaGainedOnRest;
            timer = 0;
        }

        animator.SetFloat("Speed", JoystickManager.currentJoystick.Direction.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        Debug.Log("Ejimas");
        if (isRunning && !resetNeeded)
        {
            rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
        }
        else {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
            

    }
}
