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
            spriteTransform.Rotate(new Vector3(0f, 0f, -speed * 30 * Time.fixedDeltaTime));
            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        }
        else
        {
            // rb.velocity = Vector2.zero;
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
            Vector2 direction_temp = -direction;
            float randomDirectionValue = Random.Range(-.5f, .5f);
            
            if(Mathf.Abs(direction_temp.x) > Mathf.Abs(direction_temp.y))
                direction_temp = new Vector2(direction_temp.x, direction_temp.y + randomDirectionValue).normalized;
            else if(Mathf.Abs(direction_temp.x) < Mathf.Abs(direction_temp.y))
                direction_temp = new Vector2(direction_temp.x + randomDirectionValue, direction_temp.y).normalized;

            direction = direction_temp; 
        }
    }

    public void Throw(Vector2 direction)
    {
        if(playerTransform)
        {
            transform.SetParent(null);
            playerTransform = null;
            this.direction = direction; 
        }
    }
}
