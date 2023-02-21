using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ant_movement : MonoBehaviour
{// naprawic ustawienia imputu do usera!!!!!!!!!!!!!!!!!!!
    public GameObject topAheadDetector;
    public GameObject downAheadDetector;
    public GameObject topBehindDetector;
    public GameObject downBehindDetector;
    public GameObject middleAheadDetector;
    public GameObject aheadCheck;
    public GameObject downCHeck;
    public GameObject goingDownCheck;

    topAheadDetect topAheadDetectScript;
    downAheadDetect downAheadDetectScript;
    topBehindDetect topBehindDetectScript;
    downBehindDetect downBehindDetectScript;
    middleAheadDetect middleAheadDetectScript;
    aheadCheck aheadCheckScript;
    downCheck downCheckScript;
    goingDownCheck goingDownDetectScript;

    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;
    public Animator animator;
    [SerializeField] private LayerMask layerMask, groundLayerMask, groundLayerMask1;

    private float extraHeightText = 0.009f;
    private float MovementSpeed = 3;

    public bool m_FacingRight = true;
    public bool couldAntMove = true;
    public bool isAntClimbing = false;
    public bool isCeilingWalk = false;
    public bool isAntClimbingOnRightWall = false;
    [SerializeField] private bool isAntReadyForNextAction = false;

    float MOVEMENT;


    private void Start()
    { 
        topAheadDetectScript = topAheadDetector.GetComponent<topAheadDetect>();
        downAheadDetectScript = downAheadDetector.GetComponent<downAheadDetect>();
        topBehindDetectScript = topBehindDetector.GetComponent<topBehindDetect>();
        downBehindDetectScript = downBehindDetector.GetComponent<downBehindDetect>();
        middleAheadDetectScript = middleAheadDetector.GetComponent<middleAheadDetect>();
        aheadCheckScript = aheadCheck.GetComponent<aheadCheck>();
        downCheckScript = downCHeck.GetComponent<downCheck>();
        goingDownDetectScript = goingDownCheck.GetComponent<goingDownCheck>();

        PlayerAnt = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
        turnOnGravityForFloorWalk();
        turnOnGravity();
    }

    private void Update()
    {
        if(couldAntMove)
        {
            if (isAntClimbing)
            {
                MOVEMENT = Input.GetAxis("Vertical");
            }
            else
            {
                MOVEMENT = Input.GetAxis("Horizontal");
            }
        }

    }

    private void FixedUpdate()
    {
        //isClimbing();
        Move();
    }

    private void Move()
    {
        if (!isAntClimbing)
        {
            walk();
        }
        else
        {
            climb();
        }
    }

    private void ceilingWalk()
    {
        
        transform.position += new Vector3(MOVEMENT, 0, 0) * Time.deltaTime * MovementSpeed;
        climbFromCeilingToWall();
        goDownFromCeilingToWall();
        flippingWhenAntIsOnTheCeiling();
        
    }

    private void goDownFromCeilingToWall()
    {
        if(rightCheck() && isAntReadyForNextAction)
        {
            notReadyForNextAction();
            antCantMove();
            animator.SetInteger("wallClimbSide", 21);
            turnOffCeilingWalk();
        }
    }
     

    private void climbFromCeilingToWall()
    {
        if (!downAheadCheckPoint() && isAntReadyForNextAction)
        {
            notReadyForNextAction();
            antCantMove();
            animator.SetInteger("wallClimbSide", 11);
        }
    }

    private void walk()
    {
            transform.position += new Vector3(MOVEMENT, 0, 0) * Time.deltaTime * MovementSpeed;
            
            flippingWhenAntIsOnTheFloor();
            climbOnWall();
            goDownFromFloorToWall();
                                // goDownFromFloorToWall();
    }

    private void readyForNextAction()
    {
        isAntReadyForNextAction = true;
    }

    private void notReadyForNextAction()
    {
        isAntReadyForNextAction = false;
    }


    private void walkAnim()
    {
        animator.SetInteger("wallClimbSide", 6969);
    }

    public void antWalk()
    {
        antCanMove();
        turnOnGravity();
        animator.SetInteger("wallClimbSide", 6969);
    }

    private void climb()
    {
        upToDownWallMoveRight();
        climbFromWallToSurface();
        goDownFromtWallToFloor();
        //climbOnCeiling();
        //climbFromWallToCeilingWhileAntIsClimbingDown();
        flippingWhenAntIsOnTheWall();
    }

    private void antCanMove()
    {
        couldAntMove = true;
    }

    private void antCantMove()
    {
        couldAntMove = false;
    }

    void climbFromWallToSurface()
    {
        if (isAntClimbing &&!downAheadCheckPoint() && !isAntGoingDown())
        {
            turnOffGravity();
            teleportPlayerAnt();
            rotate_minus_90();
            antStopClimbing();
            //animator.SetInteger("wallClimbSide", 2137);
            turnOnGravityForFloorWalk();
            turnOnGravity();
        }      
    }

    void climbFromWallToCeilingWhileAntIsClimbingDown()
    {
        if (isAntClimbing && !downAheadCheckPoint() && isAntReadyForNextAction && isAntGoingDown())
        {
            turnOnCeilingWalk();
            notReadyForNextAction();
            antCantMove();
            animator.SetInteger("wallClimbSide", 20);
        }
    }
    private void goDownFromtWallToFloor()
    {
        if (isAntClimbing && downAheadCheckPoint() && topAheadCheckPoint() && isAntGoingDown())
        {
            turnOffGravity();
            turnOnGravityForFloorWalk();
            rotate_plus_90();
            antStopClimbing();
            turnOnGravity();
            //animator.SetInteger("wallClimbSide", 3);
        }
    }

    private void isClimbing()
    {
        if (PlayerAnt.gravityScale == 0)
        {
 
            isAntClimbing = true;
        }
        else
        {
        
            isAntClimbing = false;
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void rotate_minus_90()
    {
        transform.Rotate(0f, 0f, -90f);
    }

    private void rotate_plus_90()
    {
        notReadyForNextAction();
        transform.Rotate(0f, 0f, 90f);
    }

    private void goDownFromFloorToWall()
    {
        if ( !downAheadCheckPoint() && downCheck() && !isAntClimbing)
        {
            turnOffGravity();
            teleportPlayerAntToDown();
            //antCantMove();
            //turnOffGravity();
            //notReadyForNextAction();
            //animator.SetInteger("wallClimbSide", 2);
            if (!m_FacingRight)
            {
                isAntClimbingOnRightWall = true;
                turnOnGravityForClimbingOnRightWall();
            }
            else
            {
                isAntClimbingOnRightWall = false;
                turnOnGravityForClimbingOnLefttWall();
            }
            turnOnGravity();
            rotate_minus_90();
            antStartClimbing();
            
        }
    }

    private void flippingWhenAntIsOnTheWall()

    {
        if (isAntClimbingOnRightWall)
        {
        
                if (MOVEMENT < 0 && m_FacingRight)
                {
                    Flip();
                }
                else if (MOVEMENT > 0 && !m_FacingRight)
                {
                    Flip();
                }        
        }
        else
        {
            if (MOVEMENT < 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (MOVEMENT > 0 && m_FacingRight)
            {
                Flip();
            }
        }
    }

    private void flippingWhenAntIsOnTheFloor()
    {
        if (!isAntClimbing)
        {
            if (MOVEMENT > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (MOVEMENT < 0 && m_FacingRight)
            {
                Flip();
            }
        }
    }

    private void flippingWhenAntIsOnTheCeiling()
    {

            if (MOVEMENT < 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (MOVEMENT > 0 && m_FacingRight)
            {
                Flip();
            }
    }

    private bool topAheadCheckPoint()
    {
        if (topAheadDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool middleAheadCheckPoint()
    {
        if (middleAheadDetectScript.flag)
            return true;
        else
            return false;
    }

    private bool downAheadCheckPoint()
    {
        if (downAheadDetectScript.flag)
        {
       
            return true;
        }
        else
        {
            
            return false;
        }
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
        if (aheadCheckScript.isTouching())
            return true;
        else
            return false;
    }


    private bool upCheck()
    {
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, extraHeightText, layerMask);

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
        if (downCheckScript.isTouching())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void climbOnWall()
    {
        if (rightCheck() && !isAntClimbing)
        {
            turnOffGravity();
            //notReadyForNextAction();
            //antCantMove();
            //animator.SetInteger("wallClimbSide", 1);
            //turnOffGravity();
            antStartClimbing();
            if (m_FacingRight)
            {
                isAntClimbingOnRightWall = true;
                turnOnGravityForClimbingOnRightWall();
                turnOnGravity();
            }
            else
            {
                isAntClimbingOnRightWall = false;
                turnOnGravityForClimbingOnLefttWall();
                turnOnGravity();
            }
            rotate_plus_90();
        }
    }

    private void antStartClimbing()
    {
        isAntClimbing = true;
    }

    private void antStopClimbing()
    {
        isAntClimbing = false;
    }

    private void climbOnCeiling()
    {
        if (rightCheck() && !isAntGoingDown() && isAntClimbing && isAntReadyForNextAction)
        {
            notReadyForNextAction();
            antCantMove();
            animator.SetInteger("wallClimbSide", 5);
            isCeilingWalk = true;
        }
    }

    private void idleAnimation()
    {
        animator.SetInteger("wallClimbSide", 6969);
    }

    private void upToDownWallMoveRight()
    {
        if (isAntClimbing)
        {
            //turnOffGravity();
            transform.position += new Vector3(0, MOVEMENT, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void turnOnGravity()
    {
        PlayerAnt.gravityScale = 1;
    }

    private void turnOffGravity()
    {
        PlayerAnt.gravityScale = 0 ;
    }

    private void turnOnGravityForClimbingOnRightWall()
    {
        Physics2D.gravity = new Vector2(9.8f, 0);
    }

    private void turnOnGravityForClimbingOnLefttWall()
    {
        Physics2D.gravity = new Vector2(-9.8f, 0);
    }

    private void turnOnGravityForFloorWalk()
    {
        Physics2D.gravity = new Vector2(0, -9.8f);
    }

    private void turnOnGravityForCeilingWalk()
    {
        Physics2D.gravity = new Vector2(0, 9.8f);
    }

    public void teleportPlayerAnt()
    {
        
        float x = transform.position.x;
        float y = transform.position.y;
        if(m_FacingRight)
            transform.position = new Vector2(x + 1.3f, y + 1f);
        else
            transform.position = new Vector2(x - 1.3f, y + 1f);
    }

    public void teleportPlayerAntToDown()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (m_FacingRight)
            transform.position = new Vector2(x + 1f, y - 1.3f);
        else
            transform.position = new Vector2(x - 1f, y - 1.3f);
    }

    public void teleportPlayerAntToUp()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (m_FacingRight)
            transform.position = new Vector2(x - 1f, y + 1.3f);
        else
            transform.position = new Vector2(x + 1f, y + 1.3f);
    }

    public void teleportPlayerAnWhileClimbingFromWallToCeiling()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (m_FacingRight)
            transform.position = new Vector2(x - 1f, y - 1f);
        else
            transform.position = new Vector2(x + 1f, y - 1f);
    }

    public void turnOnCeilingWalk()
    {
        isCeilingWalk = true;
    }

    public void turnOffCeilingWalk()
    {
        isCeilingWalk = false;
    }

    public bool isAntGoingDown()
    {
        return goingDownDetectScript.isAntGoingDown();
    }
}