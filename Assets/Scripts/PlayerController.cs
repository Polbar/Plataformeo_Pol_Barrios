using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConroller : MonoBehaviour
{
    private Rigidbody2D characterRigidbody;

    private float horizontalInput;

    private bool jumpInput;

    [SerializeField]private float characterSpeed = 4.5f;

    [SerializeField] private float jumpForce = 5;

    public static Animator characterAnimator;

    [SerializeField] private int healthPoints = 5;

    private bool isAttacking;

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //characterRigidbody.AddForce(Vector2.up * jumpForce);
    }

    void Update()
    {   
        
        Movement();

      
       if(Input.GetButtonDown("Jump") && GroundSensor.isGrounded && !isAttacking)
       {
            Jump();
       }

       if(Input.GetButtonDown("Fire1") && GroundSensor.isGrounded && !isAttacking)
       {
            Attack();
       }
    }

    void Jump()
    {
        characterRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
        characterAnimator.SetBool("IsJumping", true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        characterRigidbody.velocity = new Vector2(horizontalInput  * characterSpeed, characterRigidbody.velocity.y);
    }

    void Movement()
    {
       horizontalInput = Input.GetAxis("Horizontal"); 

       if(horizontalInput < 0)
       {
            transform.rotation = Quaternion.Euler(0, 180, 0); //sirve para girar al personaje de una manera compleja
            characterAnimator.SetBool("IsRunning", true);
       }

       else if(horizontalInput > 0)
       {
        transform.rotation = Quaternion.Euler(0, 0, 0);
        characterAnimator.SetBool("IsRunning", true);
       }

       else
       {
        characterAnimator.SetBool("IsRunning", false);
       }
    }

    void Attack()
    {
        StartCoroutine(AttackAnimation());
        characterAnimator.SetTrigger("Attack");
    }

    IEnumerator AttackAnimation()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
    }

    void TakeDamage()
        {
            healthPoints--;

            
            if(healthPoints <= 0)
            {
                Die();
            }
            else
            {
                characterAnimator.SetTrigger("IsHurt");
            }
        }
    
    void Die()
    {
        characterAnimator.SetTrigger("IsDeath");
        Destroy(gameObject, 1f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            //characterAnimator.SetTrigger("IsHurt");
            //Destroy(gameObject, 1f);
            TakeDamage();
        }
    }
}
