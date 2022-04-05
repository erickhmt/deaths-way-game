using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed, attackDistance, delayBetweenAttacks, maxHealth, damage;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 direction;
    private Animator animator;
    private bool canAttack, isDead;
    public float health;
    private Transform character;
    private CharacterStats targetStats;
    void Start()
    {   
        health = maxHealth;
        canAttack = true;
        isDead = false;
        character = transform.Find("Character");
        rb = transform.GetComponent<Rigidbody2D>();  
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = character.GetComponent<Animator>();
        targetStats = target.GetComponent<CharacterStats>();
    }

    void FixedUpdate()
    {
        if(!isDead && canAttack)
        {
            if((target.position - transform.position).magnitude < attackDistance)
                Attack();
            else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
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
        targetStats.TakeDamage(damage);

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return new WaitForSeconds(0.1f);   

        animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(delayBetweenAttacks);
        canAttack = true;
    }

    public void TakeDamage(float damage) 
    {
        health -= damage;
        if(health < 0 && !isDead)
        {
            isDead = true;
            canAttack = false;
            StartCoroutine("Death");
        }
    }
    IEnumerator Death()
    {
        rb.velocity = Vector2.zero;
        animator.SetTrigger("death");
        targetStats.RestoreMana(10f);
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(transform.gameObject);
    }

    public void FollowTarget() 
    {
        direction = (target.position - transform.position).normalized;
        direction = Vector2.ClampMagnitude(direction, 1f);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime); 

        if(target.position.x - transform.position.x > 0f)
            character.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);    
        else if(target.position.x - transform.position.x < 0f)
            character.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);  
    }
}
