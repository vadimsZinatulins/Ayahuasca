using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
/// <summary>
/// This class is deprecated. This needs a refactor, but it maybe be good for quick prototyping
/// </summary>
public class N_2DSidePlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float jumpWallForce;

    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundRadius = 0.2f;
    bool isGrounded = false;
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    bool isWallSliding = false;
    RaycastHit2D WallCheckHit;
    bool canJump = false;

    float nx = 0f;
    bool isFacingRight = true;
    public Animator animat;
    private bool candash;
    public float dash_force;
    private bool cooldown_dash = false;
    private bool dashing = false;
    public int dash_ManaCost;
    public float horJumpForce;
    private HealthSystem alive;
    private void Start()
    {
        alive = GetComponent<HealthSystem>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
    IEnumerator RestartGame(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
    }
    private void Update()
    {
        if (alive.CurrentHealth <= 0)
        {
            StartCoroutine(RestartGame(1f));
        }
        //Time.timeScale = 0.5f;
        if (Input.GetButtonDown("Jump")&& canJump && !isWallSliding)
        {
            Jump(new Vector2(rb.velocity.x, jumpForce));
            canJump = false;
        }
        if (Input.GetButtonDown("Jump") && canJump && isWallSliding)
        {
            //rb.AddForce(new Vector2(horJumpForce, jumpForce), ForceMode2D.Impulse);
            if (isFacingRight)
            {
                Jump(new Vector2(-1,0.5f)* jumpWallForce);
            }
            else
            {
                Jump(new Vector2(1, 0.5f) * jumpWallForce);                
            }
            canJump = false;
        }
        if (Input.GetButtonDown("Dash")&&candash && cooldown_dash==false /* && GetComponent<HealthSystem>()._mana -dash_ManaCost>0 */)
        {
            animat.SetBool("Dash", true);
            
            /* GetComponent<Alive>()._mana -= dash_ManaCost; */
            candash = false;
            cooldown_dash = true;
            dashing = true;
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * dash_force;
            
            float angle = Mathf.Atan2(rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);

            Debug.Log(transform.rotation.eulerAngles);
            
            rb.gravityScale = 0f;
            StartCoroutine(DurationDash(0.2f,2f));
            
        }
        animat.SetFloat("Speed", Mathf.Abs(nx));
    }
    private void FixedUpdate()
    {

        //----------------------Rotaçao--------------------------
        nx = Input.GetAxis("Horizontal");
       // Debug.Log(nx);

        if (nx<0)
        {
            isFacingRight = false;
            transform.localScale = new Vector2(-0.46184f, transform.localScale.y);
        }
        else if(nx>0)
        {
            isFacingRight = true;
            transform.localScale = new Vector2(0.46184f, transform.localScale.y);
        }
        //----------------------Rotaçao--------------------------


        //----------------------Movimento-------------------------
        if (!isWallSliding && !dashing)
            rb.velocity = new Vector2(nx * moveSpeed, rb.velocity.y);
        //----------------------Movimento-------------------------


        bool touchingGround = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        if (touchingGround)
        {
            isGrounded = true;
            isWallSliding = false;
            canJump = true;
            candash = true;
        }
        else
        {
            isGrounded = false;
            canJump = false;
            
        }

        //Wall Jump
        if (isFacingRight)
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.red);
        }
        else
        {
            WallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.red);
        }
        
        if (WallCheckHit &&!isGrounded&& nx!=0)
        {
            isWallSliding = true;
            canJump = true;
            candash = true;
        }
        else if (!WallCheckHit)
        {
            isWallSliding = false;
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
        }

    }
    IEnumerator DurationDash(float duracao,float tempo)
    {
        Debug.Log("Entrou");        
        yield return new WaitForSeconds(duracao);
        animat.SetBool("Dash", false);
        Debug.Log("Acabou");
        dashing = false;
        transform.rotation = Quaternion.FromToRotation(transform.position, Vector2.zero);
        rb.gravityScale = 1f;
        StartCoroutine(CoolDownDash(tempo));
    }
    IEnumerator CoolDownDash(float tempo)
    {     
        yield return new WaitForSeconds(tempo);
        candash = true;
        cooldown_dash = false;
    }
    void Jump(Vector2 v)
    {
        rb.velocity = v;
    }
    
}
