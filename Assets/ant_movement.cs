using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_movement : MonoBehaviour
{
    //  [Header("Movement")]
    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;

    public Animator animator;
    private float MovementSpeed = 2;
    public float JumpForce = 10;
    public float checkRadius;
    private bool m_FacingRight = true;
    bool wallClimbing;
    public float wallSlidingSpeed;
    bool isTouchingFront;
    [SerializeField] private LayerMask layerMask,groundLayerMask,groundLayerMask1;


    private void Start()
    {
        PlayerAnt = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
        
    }


    private void Update()
    {
        //Cursor.lockState = CursorLockMode.Locked;  //Placeholder na celownik
        //  Cursor.visible = false;

        if (frontCheck())
        {
            PlayerAnt.gravityScale = 0;
            if (upCheck() || groundCheck())
            {
                var wallMovement = Input.GetAxisRaw("Vertical");
                var movement = Input.GetAxisRaw("Horizontal");   
                transform.position += new Vector3(movement, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            } else 
            {
                var wallMovement = Input.GetAxisRaw("Vertical");
                transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            }
        }
        else if (upCheck())
        {
            PlayerAnt.gravityScale = -1;
            Move();
        } else
        {
            PlayerAnt.gravityScale = 1;
            Move();
        }

    }
    
    private void Move()
    {
       // isGrounded();
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

    private bool frontCheck()
    {
        float extraHeightText = 0.05f;
        RaycastHit2D rightToutch=Physics2D.BoxCast(boxCollider2d.bounds.center,boxCollider2d.bounds.size,0f, Vector2.right ,extraHeightText,layerMask);
        RaycastHit2D leftToutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, extraHeightText, layerMask);
        Color rayColor;
        if ((rightToutch.collider != null)||(leftToutch.collider!=null))
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
       
        if ((rightToutch.collider != null) || (leftToutch.collider != null))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private bool upCheck()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D upToutch = Physics2D.BoxCast(boxCollider2d.bounds.center, 0.8f*boxCollider2d.bounds.size, 0f, Vector2.up, extraHeightText, layerMask);
        Color rayColor;
        if ((upToutch.collider != null) )
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

        if ((upToutch.collider != null))
        {
            return true;
        }
        else
        {
            return false;
        }

    }


    private bool groundCheck()
    {
        float extraHeightText = 0.2f;
        RaycastHit2D groundToutch = Physics2D.BoxCast(boxCollider2d.bounds.center, 0.8f*boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, layerMask);

        Color rayColor;
        if (groundToutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        if (groundToutch.collider!=null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void beSlow()
    {
        if (groundCheck())
        {
            MovementSpeed = 1;
        }
        else
            MovementSpeed = 2;
    }

    //private void Jump()
    //{
    //    if (Input.GetButtonDown("Jump") && Mathf.Abs(PlayerAnt.velocity.y) < 0.001f)
    //    {
    //        PlayerAnt.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
    //    }
    //}
    

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

   
}