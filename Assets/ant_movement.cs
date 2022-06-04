using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ant_movement : MonoBehaviour
{
    public GameObject topAheadDetector;             //Scripts import
    topAheadDetect topAheadDetectScript;
    public GameObject downAheadDetector;
    downAheadDetect downAheadDetectScript;
    public GameObject topBehindDetector;
    topBehindDetect topBehindDetectScript;
    public GameObject downBehindDetector;
    downBehindDetect downBehindDetectScript;
   

    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;
    public Animator animator;
    public Animation anim;
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
        topAheadDetectScript = topAheadDetector.GetComponent<topAheadDetect>();
        downAheadDetectScript = downAheadDetector.GetComponent<downAheadDetect>();
        topBehindDetectScript = topBehindDetector.GetComponent<topBehindDetect>();
        downBehindDetectScript = downBehindDetector.GetComponent<downBehindDetect>();

        
        PlayerAnt = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
        Physics2D.gravity = new Vector2(0, -9.81f);
       
       
       
    }
    

    private void Update()
    {
        isOnTheWall();
        Move();
       

        climb();
        Move();
        
    
    }

    private void Move()
    {
       
        if (!isClimbing)
        {
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
            if (downCheck() == true && Input.GetKey(KeyCode.S))
            {
                transform.position -= new Vector3(0, movement, 0) * Time.deltaTime * MovementSpeed;
            }
          /*  if (Input.GetKey(KeyCode.P))
                {
                animator.Play("climb_right 0");
            }*/
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
    
    }
    
    private bool topAheadCheckPoint()
    {
        if (topAheadDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool downAheadCheckPoint()
    {
        if (downAheadDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool topBehindCheckPoint()
    {
        if (topBehindDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool downBehindCheckPoint()
    {
        if (downBehindDetectScript.flag)
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

        
        if ((rightCheck() == true && Input.GetKey(KeyCode.E)) ||(rightCheck()==true&&isClimbing==true))
        {
            if (isClimbing == false)
            {
                animator.SetInteger("wallClimbSide", 1);
            }
      
            Physics2D.gravity = new Vector2(0, 0);
            var wallMovement = Input.GetAxisRaw("Vertical");
           // animator.SetInteger("wallClimbSide", 3);
            if (topAheadCheckPoint()==false && Input.GetKey(KeyCode.W))
            {
                transform.position -= new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            }
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            isClimbing = true;
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetInteger("wallClimbSide", 3);
                //zatrzymaj();
                StartCoroutine(zatrzymaj());
               // transform.position = new Vector2(transform.position.x + 1f, transform.position.y + 1f);
                /*   if (!(this.animator.GetCurrentAnimatorStateInfo(0).IsName("right_climb 0")))
                   {
                       transform.position = new Vector2(transform.position.x + 1.2f, transform.position.y + 1f);
                       Physics2D.gravity = new Vector2(0, -9.81f);
                   }
              
                Physics2D.gravity = new Vector2(0, -9.81f);
                */
            }
        }

        if ((leftCheck() == true && Input.GetKey(KeyCode.E)) || (leftCheck() == true && isClimbing == true))
        {
            if (isClimbing == false)
            {
                Flip();
                animator.SetInteger("wallClimbSide", 2);
            }

            Physics2D.gravity = new Vector2(0, 0);
            var wallMovement = Input.GetAxisRaw("Vertical");
            
            if (topBehindCheckPoint() == false && Input.GetKey(KeyCode.W))
            {
                transform.position -= new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            }
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
            isClimbing = true;
            if (Input.GetKey(KeyCode.X))
            {
                animator.SetInteger("wallClimbSide", 3);
                //zatrzymaj();
                transform.position = new Vector2(transform.position.y + 0.1f,transform.position.x + 0.05f);

                Physics2D.gravity = new Vector2(0, -9.81f);

            }
        }

    }
 
    

    private void isOnTheWall()
    {

        if (Physics2D.gravity == new Vector2(0, 0))
            isClimbing = true;
        else
            isClimbing = false;

    }



    private void Flip()
    {
        
        
            m_FacingRight = !m_FacingRight;
            transform.Rotate(0f, 180f, 0f);
        
    }
    //sprobujmy zara wystartowac corutinea w srodku kodu - moze kurwa raczy zadzialac

    IEnumerator zatrzymaj()//jak to kurwa zrobic zeby dzialalo
    {
        Debug.Log("stop");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        transform.position = new Vector2(transform.position.x + 0.065f, transform.position.y + 0.05f);
        Physics2D.gravity = new Vector2(0, -9.81f);
    }
   // animator.GetCurrentAnimatorStateInfo(0).length + animator.GetCurrentAnimatorStateInfo(0).normalizedTime
   void setStartParameters()
    {
        animator.Play("idle");
        wallClimbing = false;
        Physics2D.gravity = new Vector2(11, 0);
        isClimbing = false;
    }
}



