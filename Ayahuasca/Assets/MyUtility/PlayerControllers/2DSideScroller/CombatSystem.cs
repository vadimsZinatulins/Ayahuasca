using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CombatSystem : MonoBehaviour
{
    public Transform attackpoint;
    public float attackRange = 0.5f;
    public LayerMask enemylayer;
    public bool pode_melee = true;
    public Animator animat;
    public Rigidbody2D rb;
    public int melee_Damage;
    public float meelee_couldown;
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackpoint.position, attackRange);
    }
    private void Awake()
    {
        animat = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.J) && pode_melee)
        {
            pode_melee = false;
            MeleeAttack();
            animat.SetBool("Attack1", true);
        }
    }
    void MeleeAttack()
    {
        Debug.Log("PlaysAnimation");
        Attack();
        rb.velocity = rb.velocity / 2;
        rb.gravityScale = rb.gravityScale / 2;
        StartCoroutine(DurationMelee(0.5f));
    }
    IEnumerator DurationMelee(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        animat.SetBool("Attack1", false);
        //rb.velocity = rb.velocity * 7;
        rb.gravityScale = rb.gravityScale * 2;
        StartCoroutine(CouldownMelee(meelee_couldown));
    }
    IEnumerator CouldownMelee(float tempo)
    {
        Debug.Log("Entrou");
        yield return new WaitForSeconds(tempo);
        Debug.Log("Acabou");
        pode_melee = true;
    }
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackpoint.position, attackRange, enemylayer);
        foreach (var enemie in hitEnemies)
        {
            if (enemie.GetComponent<HealthSystem>())
            {
                enemie.GetComponent<HealthSystem>().TakeDamage(melee_Damage);
                Debug.Log(enemie.GetComponent<HealthSystem>().CurrentHealth);
                Debug.Log("Atacou o " + enemie.name);
            }

        }
    }
}
