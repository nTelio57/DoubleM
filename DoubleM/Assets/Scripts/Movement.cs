using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    public VariableJoystick joystick;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float runSpeed;
    [Header("Stamina")]
    public Healthbar staminaBar;
    public Text staminaText;
    public float maxStamina;
    public float stamina;
    public float staminaGainedOnRest;
    public float staminaLostOnRun;
    [Range(0.9f,1)]
    public float distanceFromCenterToRun = 0.95f;
    Vector2 movement;
    float memoryMovementX;
    bool isRunning;
    
    bool resetNeeded;
    float timer = 0;
    AudioManager audioM;

    void Start()
    {
        audioM = FindObjectOfType<AudioManager>();
        if (!SaveOptions.isGameSaved)
            stamina = maxStamina;

        resetNeeded = false;
        staminaBar.SetMaxHealth((int)maxStamina);
        
        if(audioM != null)
            audioM.Play("Background1");
    }

    void Update()
    {
        staminaBar.SetHealth((int)stamina);
        staminaText.text = (int)stamina + "/" + (int)maxStamina;

        timer += Time.deltaTime;
        movement.x = joystick.Horizontal;
        movement.y = joystick.Vertical;

        if (movement.x != 0)
        {
            memoryMovementX = movement.x;
        }

        if (movement.x == 0 && movement.y == 0)
        {
            resetNeeded = false;
        }

        if (joystick.Direction.sqrMagnitude >= distanceFromCenterToRun && stamina > 0 && !resetNeeded)
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else if (joystick.Direction.sqrMagnitude >= distanceFromCenterToRun && stamina == 0)
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

        animator.SetFloat("Speed", joystick.Direction.sqrMagnitude);

        
    }

    private void FixedUpdate()
    {
        if (isRunning && !resetNeeded)
        {
            rb.MovePosition(rb.position + movement * runSpeed * Time.fixedDeltaTime);
        }
        else {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    
}
