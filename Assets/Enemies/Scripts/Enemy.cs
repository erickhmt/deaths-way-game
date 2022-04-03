using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, attackDistance, delayBetweenAttacks, maxHealth;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 direction;
    private Animator animator;
    private bool canAttack, isDead;
    private float health;
    void Start()
    {   
        health = maxHealth;
        isDead = false;
        rb = transform.GetComponent<Rigidbody2D>();  
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = transform.Find("Character").GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if(!isDead)
        {
            if((target.position - transform.position).magnitude < attackDistance && canAttack)
                Attack();
            else
                FollowTarget();
        }
    }

    public void Attack() 
    {
        canAttack = false;
        StartCoroutine("WaitAttackFinish");
    }

    IEnumerator WaitAttackFinish()
    {
        animator.SetTrigger("attack");
        animator.SetBool("isAttacking", true);

        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"));
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return null;   

        yield return new WaitForSeconds(delayBetweenAttacks);
        animator.SetBool("isAttacking", false);
        canAttack = true;
    }

    public void Damage(float damage) 
    {
        health -= damage;
        if(health < 0 && !isDead)
        {
            canAttack = false;
            isDead = true;
            StartCoroutine("Death");
        }
    }
    IEnumerator Death()
    {
        animator.SetTrigger("death");
        yield return new WaitForSeconds(5f);   
        GameObject.Destroy(transform.gameObject);
    }

    public void FollowTarget() 
    {
        direction = (target.position - transform.position).normalized;
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 
    }
}
