using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    public float speed;
    
    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>(); 
        direction = transform.right;
    }
    void FixedUpdate()
    {
        rb.velocity = direction * speed * Time.fixedDeltaTime; 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // direction = collision.gameObject.transform.;
    }
}
