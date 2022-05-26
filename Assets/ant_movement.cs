using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ant_movement : MonoBehaviour
{
    public GameObject topRightDetector;             //Scripts import
    topRightDetect topRightDetectScript;
    public GameObject downRightDetector;
    downRightDetect downRightDetectScript;
    public GameObject topLeftDetector;
    topLeftDetect topLeftDetectScript;
    public GameObject downLeftDetector;
    downLeftDetect downLeftDetectScript;
   

    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;
    public Animator animator;
    [SerializeField] private LayerMask layerMask, groundLayerMask, groundLayerMask1;

    private float extraHeightText = 0.009f;
    private float MovementSpeed = 2;
    public float checkRadius;
    public float wallSlidingSpeed;

    private bool m_FacingRight = true;
    bool wallClimbing;
    bool isTouchingFront;
    private bool isClimbing = false;

    private void Start()
    {
        topRightDetectScript = topRightDetector.GetComponent<topRightDetect>();
        downRightDetectScript = downRightDetector.GetComponent<downRightDetect>();
        topLeftDetectScript = topLeftDetector.GetComponent<topLeftDetect>();
        downLeftDetectScript = downLeftDetector.GetComponent<downLeftDetect>();


        PlayerAnt = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
       // Physics2D.gravity = new Vector2(0, -10f);
    }
    

    private void Update()
    {
        
        Move();
        leftCheck();
        rightCheck();
        upCheck();
        downCheck();
        isGravity();
        climb();
        
    
    }

    private void Move()
    {
        // isGrounded();
        var movement = Input.GetAxisRaw("Horizontal");           
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        if (leftCheck() == true && Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        }
        if (rightCheck() == true && Input.GetKey(KeyCode.D))
        {
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        }
        if(downCheck()==true&&Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, movement, 0) * Time.deltaTime * MovementSpeed;
        }

        if (isClimbing == false)
        { 
            if (movement > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (movement < 0 && m_FacingRight)
            {
                Flip();
            }
         }
    
    }
    
    private bool topRightCheckPoint()
    {
        if (topRightDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool downRightCheckPoint()
    {
        if (downRightDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool topLeftCheckPoint()
    {
        if (topLeftDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool downLeftCheckPoint()
    {
        if (downLeftDetectScript.flag)
            return true;
        else
            return false;
    }
    
    private bool rightCheck()
    {
        
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0, Vector2.right, extraHeightText, layerMask);
        Color rayColor;
        if (Toutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.down *
            (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.up * 
            (boxCollider2d.bounds.extents.y), rayColor);
        if (Toutch.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool leftCheck()
    {
        
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size , 0, Vector2.left, extraHeightText, layerMask);
        Color rayColor;
        if (Toutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.down *
            (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.up *
            (boxCollider2d.bounds.extents.y), rayColor);
        if (Toutch.collider != null)
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
        
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, extraHeightText, layerMask);

        Color rayColor;
        if (Toutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0,boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.left *
            (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0,boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.right *
            (boxCollider2d.bounds.extents.y), rayColor);
        if (Toutch.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool downCheck()
    {
        
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, layerMask);

        Color rayColor;
        if (Toutch.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.left *
            (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(0, boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.right *
            (boxCollider2d.bounds.extents.y), rayColor);

        if (Toutch.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
 
    private void climb()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        if (rightCheck()==true&&topRightCheckPoint()==true)
        {
            Physics2D.gravity = new Vector2(0, 0);
            var wallMovement = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            isClimbing = true;
        }

        if (rightCheck()&&Input.GetKey(KeyCode.A)&& Input.GetKey(KeyCode.J)==false)
        {
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        }
        else
        {
            Physics2D.gravity = new Vector2(0, -10f);
            isClimbing = false;

        }
       
        if (leftCheck() && topRightCheckPoint() )
        {
            Physics2D.gravity = new Vector2(0, 0);
            var wallMovement = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;

        }
        else
        {
            Physics2D.gravity = new Vector2(0, -10f);

        }

    }
    private void wallWalk()
    {
       
         if(true)
        {
            if (m_FacingRight)
                animator.SetInteger("wallClimbSide", 1);
            else if (!m_FacingRight)
                animator.SetInteger("wallClimbSide", 1);
            
          //  PlayerAnt.gravityScale = 0;
            var wallMovement = Input.GetAxisRaw("Vertical");
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
         else
        {
          //  PlayerAnt.gravityScale = 1;
          
        }
         
        
        
    }

    private void isGravity()
    {

        if(leftCheck()||rightCheck()||upCheck()||downCheck())
            PlayerAnt.gravityScale=0.0f;
        else
            PlayerAnt.gravityScale = 1.0f;

    }



    private void Flip()
    {
        
        
            m_FacingRight = !m_FacingRight;
            transform.Rotate(0f, 180f, 0f);
        
    }

   
}