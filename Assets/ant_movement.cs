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
        Move();
        Jump();
        wallWalk();
        beSlow();
        if((groundCheck()||groundCheck())&&frontCheck()==false)
            animator.SetInteger("wallClimbSide", 0);


        //Cursor.lockState = CursorLockMode.Locked;  //Placeholder na celownik
        //  Cursor.visible = false;
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
       
        
          Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x+ extraHeightText, 0), Vector2.down * (boxCollider2d.bounds.extents.y),rayColor);       //narysowanie prostokatow
          Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x+ extraHeightText, 0), Vector2.up * (boxCollider2d.bounds.extents.y ), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x+ extraHeightText, 0), Vector2.up * (boxCollider2d.bounds.extents.y ), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x+ extraHeightText, 0), Vector2.down * (boxCollider2d.bounds.extents.y ), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.left * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.right * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.left * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);
          Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.right * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);
       

        if ((rightToutch.collider != null) || (leftToutch.collider != null))
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
        float extraHeightText = 0.1f;
        RaycastHit2D groundToutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, groundLayerMask);

        Color rayColor;
        if (groundToutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }

       
    //    Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x , 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
     //   Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x , 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
       // Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.y+ extraHeightText, 0), Vector2.left * (boxCollider2d.bounds.extents.x ), rayColor);
      //  Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.y+ extraHeightText, 0), Vector2.right * (boxCollider2d.bounds.extents.x ), rayColor);
       // Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.left * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);
       // Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.y, 0), Vector2.right * (boxCollider2d.bounds.extents.x + extraHeightText), rayColor);

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
    
    private void wallWalk()
    {
  
        
       
       
         if(frontCheck())
        {
            if (m_FacingRight)
                animator.SetInteger("wallClimbSide", 1);
            else if (!m_FacingRight)
                animator.SetInteger("wallClimbSide", 1);
            
            PlayerAnt.gravityScale = 0;
            var wallMovement = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
         else
        {
            PlayerAnt.gravityScale = 1;
          
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