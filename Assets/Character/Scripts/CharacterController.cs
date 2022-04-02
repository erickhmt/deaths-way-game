using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float speed;
    
    private Vector2 direction = new Vector2(); 
    private Rigidbody2D rb;
    private Camera cam;
    private Scythe scythe;
    private Transform characterTransform;

    private Animator animator;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        scythe = Object.FindObjectOfType<Scythe>();
        characterTransform = transform.Find("Character");
        animator = characterTransform.GetComponent<Animator>();
        cam = Camera.main;
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
            scythe.Throw(-mouseDirection);
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
