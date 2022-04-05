using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed, speedMultiplier;
    public Transform commands;
    [HideInInspector]
    public bool isRunning;
    [HideInInspector]
    public Animator animator;
    
    private Vector2 direction = new Vector2(); 
    private Rigidbody2D rb;
    private Camera cam;
    private Scythe scythe;
    private Transform characterTransform;

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
            {
                commands.Find("Throw Scythe").gameObject.SetActive(false);
                commands.Find("Get Scythe").gameObject.SetActive(true);
                scythe.Throw(-mouseDirection);
                animator.SetTrigger("throw");
            }
            else
            {
                scythe.RequestReturn();
                commands.Find("Get Scythe").gameObject.SetActive(false);
            }

        // Set run state
        if(stats.stamina > 0f  && !isRunning && direction != Vector2.zero)
        {
            commands.Find("Run").gameObject.SetActive(true);

            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                isRunning = true;
                speed *= speedMultiplier;
                animator.speed *= speedMultiplier;
            }   
        }
        if((Input.GetKeyUp(KeyCode.LeftShift) || stats.stamina == 0f || direction == Vector2.zero) && isRunning)
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
