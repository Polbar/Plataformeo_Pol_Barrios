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

    [SerializeField] private int _maxHealth = 5;

    [SerializeField] private int _currentHealth;

    private bool isAttacking;

    [SerializeField] private Transform attackHitBox;

    [SerializeField] private float attackRadius = 3;

    private AudioSource _audioSource;

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody2D>();
        characterAnimator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentHealth = _maxHealth;

        GameManager.instance.SetHealthBar(_maxHealth);
        //characterRigidbody.AddForce(Vector2.up * jumpForce);
    }

    void Update()
    {
        Moviment();

        if(Input.GetButtonDown("Jump") && GroundSensor.isGrounded && !isAttacking)
       {
         Jump();
         SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.jumpAudio);
        }
      
       if(Input.GetButtonDown("Fire1") && GroundSensor.isGrounded && !isAttacking)
       {
         Attack();
       }

       if(Input.GetKeyDown(KeyCode.P))
       {
        GameManager.instance.Pause();
        SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.pauseAudio);
       }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAttacking)
        {
            if(horizontalInput == 0)
            {
                characterRigidbody.velocity = new Vector2(0, characterRigidbody.velocity.y);
            }
        }
        else 
        {
            characterRigidbody.velocity = new Vector2(horizontalInput  * characterSpeed, characterRigidbody.velocity.y);
        }
        
    }


    void Moviment()
    {
    
     horizontalInput = Input.GetAxis("Horizontal");
        

       if(horizontalInput < 0)
       {

        if(!isAttacking)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); //sirve para girar al personaje de una manera compleja
        }
            
            characterAnimator.SetBool("IsRunning", true);
       }
        else if(horizontalInput > 0)
       {

        if(!isAttacking)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
        characterAnimator.SetBool("IsRunning", true);
       }

       else
       {
        characterAnimator.SetBool("IsRunning", false);
       }
    }

    void Attack()
    {
        // Aquí se activan las animaciones según si el personaje se está moviendo o no
        if (horizontalInput == 0)
        {
            characterAnimator.SetTrigger("Attack");
            SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.attackAudio);
        }
        else
        {
            characterAnimator.SetTrigger("RunAttack");
            SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.attackspeedAudio);
        }
        
        // Iniciamos la lógica de ataque y daño en ambos casos
        StartCoroutine(AttackAnimation()); 
    }

    IEnumerator AttackAnimation()
    {
        isAttacking = true;

        yield return new WaitForSeconds(0.03f);

        Collider2D[] collider = Physics2D.OverlapCircleAll(attackHitBox.position, attackRadius); 
        foreach(Collider2D enemy in collider)
        {
            if(enemy.gameObject.CompareTag("Mimico"))
            {
                //Destroy(enemy.gameObject);
                Rigidbody2D enemyRigidbody = enemy.GetComponent<Rigidbody2D>();
                enemyRigidbody.AddForce(transform.right + transform.up * 2, ForceMode2D.Impulse);
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                enemyScript.TakeDamage();
            }
        }

        yield return new WaitForSeconds(0.1f);

        isAttacking = false;
    }

    void Jump()
    {
        
     characterRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse); 
     characterAnimator.SetBool("IsJumping", true);
        
    }

    void TakeDamage(int damage)
        {
            _currentHealth -= damage;

            GameManager.instance.UpdateHealthBar(_currentHealth);
                
            if(_currentHealth <= 0)
            {
                Die();
                SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.deathAudio);
            }

            else
            {
                characterAnimator.SetTrigger("IsHurt");
                SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.damageAudio);
            }
        }
    
        void Die()
        {
            characterAnimator.SetTrigger("IsDeath");
            SoundManager.instance.PlaySFX(_audioSource, SoundManager.instance.deathAudio);
            // Esperar 1 segundo para que la animación de muerte se complete y luego cambiar de escena
            StartCoroutine(WaitAndLoadGameOver());
        }

        IEnumerator WaitAndLoadGameOver()
        {
            yield return new WaitForSeconds(1f); // Esperar 1 segundo
            GameManager.instance.SceneLoader("Game Over"); // Cargar la escena de Game Over
        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 8)
        {
            //characterAnimator.SetTrigger("IsHurt");
            //Destroy(gameObject, 1f);
            TakeDamage(1);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackHitBox.position, attackRadius);
    }

    public void IncreaseHealth(int amount)
    {
    if (_currentHealth < _maxHealth)
    {
        _currentHealth += amount;
        if (_currentHealth > _maxHealth) // Asegúrate de no superar el máximo
        {
            _currentHealth = _maxHealth;
        }

        GameManager.instance.UpdateHealthBar(_currentHealth); // Actualiza la barra de salud
    }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto con el que colisiona tiene el tag "Limites"
        if (other.CompareTag("limites"))
        {
            // Cargar la escena de Game Over
            GameManager.instance.SceneLoader("Game Over");
        }
    }

}
