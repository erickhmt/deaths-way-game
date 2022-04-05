using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemy : MonoBehaviour
{
    public float speed, attackDistance, delayBetweenAttacks, attackTime, maxHealth, damage, distanceToInitAttack;
    private Rigidbody2D rb;
    private Transform target;
    private Vector2 direction;
    private Animator animator;
    private bool canAttack, isDead, isAttacking;
    public float health;
    private Transform character;
    private CharacterStats targetStats;
    void Start()
    {   
        health = maxHealth;
        canAttack = true;
        isDead = isAttacking = false;
        character = transform.Find("Character");
        rb = transform.GetComponent<Rigidbody2D>();  
        target = GameObject.FindGameObjectWithTag("Player").transform;
        animator = character.GetComponent<Animator>();
        targetStats = target.GetComponent<CharacterStats>();
        attackTime = 5;
    }

    void FixedUpdate()
    {
        if(!isDead)
        {
            if(canAttack)
            {
                if((target.position - transform.position).magnitude < distanceToInitAttack)
                    Attack();
            }
            else if(isAttacking)
            {
                FollowTarget();
                if((target.position - transform.position).magnitude < attackDistance)
                {
                    targetStats.TakeDamage(damage);
                    StartCoroutine(AbortAttack());
                }
            }
        }   
    }

    public void Attack() 
    {
        canAttack = false;
        StartCoroutine("WaitAttackFinish");
    }

    IEnumerator WaitAttackFinish()
    {
        isAttacking = true;
        animator.SetTrigger("attack");
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(attackTime);
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return new WaitForSeconds(0.1f);   

        yield return new WaitForSeconds(delayBetweenAttacks);
        canAttack = true;
    }

    IEnumerator AbortAttack() {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
        

        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            yield return new WaitForSeconds(0.1f);   

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
        targetStats.RestoreMana(25f);
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
