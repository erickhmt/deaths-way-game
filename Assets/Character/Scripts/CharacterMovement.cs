using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float speed;
    
    private Vector2 direction = new Vector2(); 
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        sprite = transform.GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        
        if(direction.x < -0.1f)
            sprite.flipX = true;
        else if(direction.x > 0.1f)
            sprite.flipX = false;  
    }
}
