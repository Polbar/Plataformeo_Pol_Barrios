using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour



{
    private Rigidbody2D characterRigidbody;

    private float horizontalInput;

    [SerializeField]private float characterSpeed = 4.5f;

    [SerializeField] private float jumpForce = 5;

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //characterRigidbody.AddForce(Vector2.up * jumpForce);
    }

    void Update()
    {
       horizontalInput = Input.GetAxis("Horizontal"); 

       if(Input.GetButtonDown("Jump"))
       {
         characterRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
       }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        characterRigidbody.velocity = new Vector2(horizontalInput  * characterSpeed, characterRigidbody.velocity.y);
    }
}