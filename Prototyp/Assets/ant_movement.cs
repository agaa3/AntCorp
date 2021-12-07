using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_movement : MonoBehaviour
{
    //  [Header("Movement")]
    public Rigidbody2D PlayerAnt;
    public float MovementSpeed = 10;
    public float JumpForce = 10;
    public float checkRadius;
    private bool m_FacingRight = true;
    bool wallSliding;
    public float wallSlidingSpeed;
    public Transform frontCheck;
    bool isTouchingFront;
    private void Start()
    {
        PlayerAnt = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        Move();
        Jump();
        wallWalk();


        //Cursor.lockState = CursorLockMode.Locked;  //Placeholder na celownik
        //  Cursor.visible = false;
    }

    private void Move()
    {

        var movement = Input.GetAxisRaw("Horizontal");             //poruszanie sie lewo/prawo
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        if (movement > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (movement < 0 && m_FacingRight)
        {
            Flip();
        }
    }

    
    private void wallWalk()
    {
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, checkRadius);
       

        if (isTouchingFront == true && (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.S)) /*&& (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))*/)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if(isTouchingFront)
        {
            Physics2D.gravity = new Vector2(0, 0f);
        }
         else
        {
            Physics2D.gravity = new Vector2(0, -9f);
        }

        if (wallSliding)
        {
            var wallMovement = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            // PlayerAnt.velocity = new Vector2(PlayerAnt.velocity.x, Mathf.Clamp(PlayerAnt.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
       else
        {
          //  transform.position -=new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
        
    }

        private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(PlayerAnt.velocity.y) < 0.001f)
        {
            PlayerAnt.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}