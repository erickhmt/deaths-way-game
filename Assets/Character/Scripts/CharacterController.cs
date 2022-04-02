using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed, speedMultiplier;
    [HideInInspector]
    public bool isRunning;
    
    private Vector2 direction = new Vector2(); 
    private Rigidbody2D rb;
    private Camera cam;
    private Scythe scythe;
    private Transform characterTransform;

    private Animator animator;
    private CharacterStats stats;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        scythe = Object.FindObjectOfType<Scythe>();
        characterTransform = transform.Find("Character");
        animator = characterTransform.GetComponent<Animator>();
        stats = transform.GetComponent<CharacterStats>();
        cam = Camera.main;
        isRunning = false;
    }

    void Update() 
    {
        Vector3 playerScreenPos = cam.WorldToScreenPoint(transform.position);
        Vector2 mouseDirection = (playerScreenPos - Input.mousePosition).normalized;
 
        // Flip sprite by mouse position
        if(mouseDirection.x > 0f)
            characterTransform.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
        else
            characterTransform.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

        // Throw scythe
        if(Input.GetKeyDown(KeyCode.Mouse0))
            if(!scythe.isThrowing)
                scythe.Throw(-mouseDirection);
            else
                scythe.RequestReturn();

        // Set run state
        if(Input.GetKeyDown(KeyCode.LeftShift) && stats.stamina > 0f  && !isRunning)
        {
            isRunning = true;
            speed *= speedMultiplier;
            animator.speed *= speedMultiplier;
        }
        if((Input.GetKeyUp(KeyCode.LeftShift) || stats.stamina == 0f) && isRunning)
        {
            isRunning = false;
            speed /= speedMultiplier;
            animator.speed /= speedMultiplier;
        }

        if(isRunning) stats.ConsumeStamina();
            
    }
    void FixedUpdate()
    {
        // Move player
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 
        animator.SetBool("isRunning", direction != Vector2.zero);
    }
}
