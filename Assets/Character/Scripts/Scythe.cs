using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed;
    
    private Rigidbody2D rb;
    private Vector2 direction;
    private Transform spriteTransform;
    private Transform playerTransform;
    private Collider2D coll;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>(); 
        direction = transform.up;
        spriteTransform = transform.Find("ScytheSprite");
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        coll = transform.GetComponent<Collider2D>(); 
    }
    void FixedUpdate()
    {
        if(!playerTransform)
        {
            spriteTransform.Rotate(new Vector3(0f, 0f, -speed * 2 * Time.fixedDeltaTime));
        }
        else
        {
            rb.velocity = Vector2.zero;
            coll.isTrigger = true;
            transform.position = playerTransform.position;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerTransform = collision.gameObject.transform;
        }
        else
        {
            ContactPoint2D contact = collision.GetContact(0);

            int randomDirectionValue = Random.Range(-10, 10);
            randomDirectionValue = randomDirectionValue > 0 ? 1 : -1;
            
            if(contact.normal.x != 0)
                direction = new Vector2(contact.normal.x, randomDirectionValue).normalized;
            else if(contact.normal.y != 0)
                direction = new Vector2(randomDirectionValue, contact.normal.y).normalized;
        }
    }

    public void Throw(Vector2 direction)
    {
        playerTransform = null;
        rb.velocity = direction * speed * Time.fixedDeltaTime; 
        coll.isTrigger = false;
    }
}
