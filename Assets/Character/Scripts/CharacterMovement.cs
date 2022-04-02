using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    
    private Vector2 direction = new Vector2(); 
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Camera cam;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sprite = transform.GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    void Update() 
    {
        // Flip sprite by mouse position
        Vector3 playerScreenPos = cam.WorldToScreenPoint(transform.position);
        if((playerScreenPos - Input.mousePosition).x > 0f)
            sprite.flipX = true;
        else
            sprite.flipX = false; 
    }
    void FixedUpdate()
    {
        // Move player
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 
    }
}
