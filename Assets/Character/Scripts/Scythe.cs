using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed;
    public Transform handBone;
    
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
            spriteTransform.Rotate(new Vector3(0f, 0f, -speed * Time.fixedDeltaTime));
        }
        else
        {
            rb.velocity = Vector2.zero;
            transform.SetParent(handBone);
            transform.localPosition = new Vector3(0f, 0f, 0f);
            transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
            spriteTransform.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            playerTransform = collider.gameObject.transform;
        }
        else
        {
            Vector2 direction = -rb.velocity.normalized;
            float randomDirectionValue = Random.Range(-0.75f, 0.75f);
            
            if(direction.x > direction.y)
                direction = new Vector2(direction.x, direction.y + randomDirectionValue).normalized;
            else if(direction.x < direction.y)
                direction = new Vector2(direction.x + randomDirectionValue, direction.y).normalized;

            rb.velocity = direction * speed * Time.fixedDeltaTime; 
        }
    }

    public void Throw(Vector2 direction)
    {
        if(playerTransform)
        {
            transform.SetParent(null);
            playerTransform = null;
            rb.velocity = direction * speed * Time.fixedDeltaTime; 
        }
    }
}
