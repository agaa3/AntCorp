using UnityEngine;
using System.Threading;

public class ant_movement : MonoBehaviour
{
    public Animator animator;

    // TO-DO: Figure out how much sensors does ant actually need.
    //        Test OnCollisionEnter2D and OnCollisionExit2D.

    [Header("Sensors")]
    public TriggerSensor AheadTopSensor;
    public TriggerSensor AheadMidSensor;
    public TriggerSensor AheadDownSensor;
    public TriggerSensor BehindTopSensor;
    public TriggerSensor BehindDownSensor;
    [Header("Checks")]
    public TriggerSensor[] AheadChecks;
    public TriggerSensor[] DownChecks;
    public goingDownCheck goingDownDetectScript;
    public wallCheck wallCheckScript;


    private Rigidbody2D UseRigidbody;
    private BoxCollider2D boxCollider2d;

    private float extraHeightText = 0.009f;
    private float MovementSpeed = 2.3f;
    [SerializeField] private LayerMask layerMask, groundLayerMask, groundLayerMask1;

    public bool m_FacingRight = true;
    public bool couldAntMove = true;
    public bool isAntClimbing = false;
    public bool isCeilingWalk = false;
    public bool isTeleportingEnable = true;
    [SerializeField] private bool isAntReadyForNextAction = false;

    float MOVEMENT;
    Vector2 gravityOverride;


    private void Start()
    {
        UseRigidbody = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
        SetFloorGravity();
        turnOnGravity();
        animator.speed = 2;
    }

    private void Update()
    {
        if (couldAntMove)
        {
            if (isAntClimbing && !isCeilingWalk)
            {
                MOVEMENT = Input.GetAxis("Vertical") / 2f;
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
        ApplyGravityOverride();
        Move();
    }

    private void Move()
    {
        if (!isAntClimbing)
        {
            walk();
            Debug.Log("walk!");
        }
        else if (isAntClimbing && !isCeilingWalk)
        {
            climb();
            Debug.Log("climb!");
        }
        else
        {
            ceilingWalk();
            Debug.Log("ceiling!");
        }
    }

    private void disableTeleports()
    {
        isTeleportingEnable = false;
        Thread.Sleep(5);
        isTeleportingEnable = true;
    }

    private void ceilingWalk()
    {
        flippingWhenAntIsOnTheCeiling();
        transform.position += new Vector3(MOVEMENT, 0, 0) * Time.deltaTime * MovementSpeed;
        climbFromCeilingToWall();
        goDownFromCeilingToWall();

    }

    private void goDownFromCeilingToWall()
    {
        if (rightCheck() && isAntReadyForNextAction)
        {
            turnOffGravity();
            notReadyForNextAction();
            rotate_plus_90();
            //antCantMove();
            //animator.SetInteger("wallClimbSide", 21);
            turnOffCeilingWalk();
            if (!isAntClimbingOnRightWall())
                SetWallGravity(false);
            else
                SetWallGravity(true);
            turnOnGravity();
            MOVEMENT = 0;
        }
    }


    private void climbFromCeilingToWall()
    {
        if (!AheadDownSensor.IsTriggering() && isAntReadyForNextAction && isTeleportingEnable)
        {
            notReadyForNextAction();
            antCantMove();
            turnOffGravity();
            teleportPlayerAntToUp();
            rotate_minus_90();
            if (isAntClimbingOnRightWall())
            {
                SetWallGravity(true);
            }
            else
            {
                SetWallGravity(false);
            }

            turnOnGravity();
            antCanMove();
            readyForNextAction();
            turnOffCeilingWalk();
            antStartClimbing();
            MOVEMENT = 0;
            //animator.SetInteger("wallClimbSide", 11);
        }
    }

    private void walk()
    {
        flippingWhenAntIsOnTheFloor();
        transform.position += new Vector3(MOVEMENT, 0, 0) * Time.deltaTime * MovementSpeed;


        climbOnWall();
        goDownFromFloorToWall();
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
        climbFromWallToSurface();
        flippingWhenAntIsOnTheWall();
        upToDownWallMoveRight();
        upToDownWallMoveRight();
        goDownFromtWallToFloor();
        climbOnCeiling();
        climbFromWallToCeilingWhileAntIsClimbingDown();
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
        if (isAntClimbing && !AheadDownSensor.IsTriggering() && !isAntGoingDown() && isTeleportingEnable && isAntReadyForNextAction)
        {
            notReadyForNextAction();
            turnOffGravity();
            antCantMove();
            antStopClimbing();
            SetFloorGravity();
            MOVEMENT = 0;
            turnOffGravity();
            teleportPlayerAnt();
            rotate_minus_90();
            turnOnGravity();
            disableTeleports();
            setFacingRight();
            antCanMove();
            MOVEMENT = 0;
        }
    }

    void climbFromWallToCeilingWhileAntIsClimbingDown()
    {
        if (isAntClimbing && !AheadDownSensor.IsTriggering() && isAntReadyForNextAction && isAntGoingDown() && isTeleportingEnable)
        {
            turnOffGravity();
            turnOnCeilingWalk();
            SetCeilingGravity();
            notReadyForNextAction();
            antCantMove();
            if (isAntClimbingOnRightWall())
            {
                m_FacingRight = true;
            }
            else
            {
                m_FacingRight = false;
            }

            rotate_minus_90();
            teleportPlayerAnWhileClimbingFromWallToCeiling();
            antCanMove();
            turnOnGravity();
            MOVEMENT = 0;
            //if(!isAntClimbingOnRightWall())
            //{
            //    Flip();
            //}
            //setFacingRight();

            //animator.SetInteger("wallClimbSide", 20);
        }
    }
    private void goDownFromtWallToFloor()
    {
        if (isAntClimbing && AheadDownSensor.IsTriggering() && AheadTopSensor.IsTriggering() && isAntGoingDown())
        {
            couldAntMove = false;
            MOVEMENT = 0f;
            turnOffGravity();
            SetFloorGravity();
            rotate_plus_90();
            antStopClimbing();
            turnOnGravity();
            ////////////////////////////////////////////
            //animator.SetInteger("wallClimbSide", 3);
            setFacingRight();
            couldAntMove = true;
            MOVEMENT = 0;
        }
    }

    private void isClimbing()
    {
        if (UseRigidbody.gravityScale == 0)
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

    private void wallFlip()
    {
        transform.Rotate(0f, 180f, 0f);
    }

    public void setFacingRight()
    {
        if (isAntGoingRight() == true)
        {
            m_FacingRight = true;
        }
        else
        {
            m_FacingRight = false;
        }
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
        if (!AheadDownSensor.IsTriggering() && downCheck() && !isAntClimbing && isTeleportingEnable && isAntReadyForNextAction)
        {
            turnOffGravity();
            teleportPlayerAntToDown();
            //antCantMove();
            //turnOffGravity();
            notReadyForNextAction();
            rotate_minus_90();
            //animator.SetInteger("wallClimbSide", 2);
            if (!isAntClimbingOnRightWall())
            {
                SetWallGravity(false);
            }
            else
            {
                SetWallGravity(true);
            }
            turnOnGravity();
            antStartClimbing();
            disableTeleports();
            MOVEMENT = 0;
        }
    }

    private void flippingWhenAntIsOnTheWall()
    {

        if (MOVEMENT > 0 && isAntGoingDown())
        {
            Flip();
        }
        else if (MOVEMENT < 0 && !isAntGoingDown())
        {
            Flip();
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

        if (MOVEMENT < 0 && m_FacingRight)
        {
            Flip();
        }
        else if (MOVEMENT > 0 && !m_FacingRight)
        {
            Flip();
        }
    }

    private bool rightCheck()
    {
        bool flag = false;
        for (int i = 0; i < AheadChecks.Length && flag != true; i++)
        {
            flag = AheadChecks[i].IsTriggering();
        }
        return flag;
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
        bool flag = false;
        for (int i = 0; i < DownChecks.Length && flag != true; i++)
        {
            flag = DownChecks[i].IsTriggering();
        }
        return flag;
    }

    private void climbOnWall()
    {
        if (rightCheck() && !isAntClimbing && isAntReadyForNextAction)
        {
            turnOffGravity();
            //notReadyForNextAction();
            //antCantMove();
            //animator.SetInteger("wallClimbSide", 1);
            //turnOffGravity();
            antStartClimbing();
            if (m_FacingRight)
            {

                SetWallGravity(true);
                turnOnGravity();
            }
            else
            {
                SetWallGravity(false);
                turnOnGravity();
            }
            rotate_plus_90();
            MOVEMENT = 0f;
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
            turnOffGravity();
            SetCeilingGravity();
            notReadyForNextAction();
            rotate_plus_90();
            //animator.SetInteger("wallClimbSide", 5);
            //isCeilingWalk = true;
            turnOnCeilingWalk();
            turnOnGravity();
            setFacingRight();
            MOVEMENT = 0f;
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
        UseRigidbody.gravityScale = 1;
    }

    private void turnOffGravity()
    {
       
        UseRigidbody.gravityScale = 0;
    }

    public void teleportPlayerAnt()
    {

        float x = transform.position.x;
        float y = transform.position.y;
        if (isAntClimbingOnRightWall())
            transform.position = new Vector2(x + 1.1f, y + 1f);
        else
            transform.position = new Vector2(x - 1.1f, y + 1f);
    }

    public void teleportPlayerAntToDown()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (m_FacingRight)
            transform.position = new Vector2(x + 1f, y - 1.4f);
        else
            transform.position = new Vector2(x - 1f, y - 1.4f);
    }

    public void teleportPlayerAntToUp()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (isAntGoingRight())
            transform.position = new Vector2(x + 1f, y + 1.4f);
        else
            transform.position = new Vector2(x - 1f, y + 1.4f);
    }

    public void teleportPlayerAnWhileClimbingFromWallToCeiling()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (!isAntGoingRight())
            transform.position = new Vector2(x - 1.4f, y - 1f);
        else
            transform.position = new Vector2(x + 1.4f, y - 1f);
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
        return goingDownDetectScript.IsGoingDown();
    }

    public bool isAntClimbingOnRightWall()
    {
        return !wallCheckScript.IsOnRightWall();
    }

    public bool isAntGoingRight()
    {
        return goingDownDetectScript.IsGoingRight();
    }


    private void SetWallGravity(bool right)
    {
        float h = -Physics2D.gravity.y;
        gravityOverride.y = h;
        if (!right)
        {
            h = -h;
        }
        gravityOverride.x = h;
    }
    private void SetFloorGravity()
    {
        gravityOverride = Vector2.zero;
    }
    private void SetCeilingGravity()
    {
        gravityOverride.x = 0;
        gravityOverride.y = -Physics2D.gravity.y;
    }
    private void ApplyGravityOverride()
    {
        UseRigidbody.AddForce(gravityOverride);
    }
}