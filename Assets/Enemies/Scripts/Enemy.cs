using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 direction;
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();  
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        FollowTarget();
    }

    public void ThrowBack(Vector2 direction)
    {
        rb.AddForce(direction * 100f * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void FollowTarget() 
    {
        direction = (target.position - transform.position).normalized;
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 
    }
}
