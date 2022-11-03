// Przy budowaniu mapy nale�y umiescic na ka�dym rogu teleport , inacze findDestinationTeleport()/findStartTeleport() zwraca null i gierka sie wysypie
//Sterowanie:
//  E: wej�cie na �cian�
//  R: wdrapanie sie na g�r� 
//  F: oderwanie si� od �ciany
//TODO:
//switch na pocz�tku i ko�cu animacji
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ant_movement : MonoBehaviour
{
    public class teleports_pair
    {
        public Transform getTeleport1()
        {
            return teleport1;
        }
        public Transform getTeleport2()
        {
            return teleport2;
        }

        public teleports_pair(Transform t1, Transform t2)
        {
            teleport1 = t1;
            teleport2 = t2;
        }

        private Transform teleport1;
        private Transform teleport2;
    };


    public GameObject topAheadDetector;
    public GameObject downAheadDetector;
    public GameObject topBehindDetector;
    public GameObject downBehindDetector;
    public GameObject middleAheadDetector;
    public GameObject aheadCheck;
    public GameObject downCHeck;

    topAheadDetect topAheadDetectScript;
    downAheadDetect downAheadDetectScript;
    topBehindDetect topBehindDetectScript;
    downBehindDetect downBehindDetectScript;
    middleAheadDetect middleAheadDetectScript;
    aheadCheck aheadCheckScript;
    downCheck downCheckScript;

    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;
    public Animator animator;
    [SerializeField] private LayerMask layerMask, groundLayerMask, groundLayerMask1;

    private float extraHeightText = 0.009f;
    private float MovementSpeed = 3;

    private bool m_FacingRight = true;
    private bool couldAntMove = true;

    public static int NUMBER_OF_TELEPORTS_PAIRS = 6;
    public static float distanceFromStartingTeleport = 0.5f;
    public static float distanceFromDestinationTeleport = 1.65f;
    public List<teleports_pair> teleports = new List<teleports_pair>();
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

        PlayerAnt = GetComponent<Rigidbody2D>();
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        animator.SetInteger("wallClimbSide", 0);
        Physics2D.gravity = new Vector2(0, -9.81f);
        setTeleportsInArray();
        PlayerAnt.gravityScale = 1;

    }

    private void Update()
    {
        if(couldAntMove)
        {
            if (isClimbing())
                MOVEMENT = Input.GetAxis("Vertical");
            else
                MOVEMENT = Input.GetAxis("Horizontal");
        }
    }

    private void FixedUpdate()
    {

        if (couldAntMove)
            Debug.Log("COULD ANT MOVE : TRUE");
        else
            Debug.Log("COULD ANT MOVE: FALSE");
        Move();
    }

    private void Move()
    {
        if (couldAntMove)
        {
            if (isClimbing())
                climb();
            else
                walk();
        }
    }

    private void walk()
    {
            transform.position += new Vector3(MOVEMENT, 0, 0) * Time.deltaTime * MovementSpeed;
            
            //doNotFallDownFromPlatform(movement);
            //flippingWhenAntIsOnTheFloor(movement);
            rightClimbOnWall();
            //leftClimbOnWall();
            //rightGoDown();
    }

    public void antWalk()
    {
        animator.SetInteger("wallClimbSide", 6969);
    }

    private void climb()
    {
        //goDownFromRightWallToFloor();
        upToDownWallMoveRight();
        //upToDownWallMoveLeft();
        rightClimbFromWallToSurface();
        //leftClimbFromWallToSurface();
        //doNotBuggingOnRightWallWhenAntGoingDown(movement);
       // detachFromWall();   
    }

    private void antCanMove()
    {
        couldAntMove = true;
    }

    private void antCantMove()
    {
        couldAntMove = false;
    }

    void rightClimbFromWallToSurface()
    {
        if (isClimbing()&&!downAheadCheckPoint())
        {
            animator.SetInteger("wallClimbSide", 2137);
        }      
    }

    private void goDownFromRightWallToFloor()
    {
        if (isClimbing() && downAheadCheckPoint() && Input.GetKey(KeyCode.E))
        {
            animator.SetInteger("wallClimbSide", 9999);
        }
    }

    void rightGoDown()
    {
        if(!isClimbing() && !downAheadCheckPoint() && Input.GetKey(KeyCode.R))
        {
            animator.SetInteger("wallClimbSide", 1111);
        }
    }

    private bool isClimbing()
    {
        if (PlayerAnt.gravityScale == 0)
        {
            Debug.Log("is Climbing: TRUE");
            return true;
        }
        else
        {
            Debug.Log("is Climbing: FALSE");
                return false;
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void rotate_minus_90()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 0f, -90f);
    }

    private void doNotFallDownFromPlatform(float movement)
    {
        if (!downAheadCheckPoint())
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
    }

    private void flippingWhenAntIsOnTheFloor(float movement)
    {
        if (!isClimbing())
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
        if (aheadCheckScript.isTouching())
            return true;
        else
            return false;
    }


    private bool upCheck()
    {
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, extraHeightText, layerMask);

        Color rayColor;
        if (Toutch.collider != null)
        {
            Debug.Log("UP CHECK : TRUE");
            rayColor = Color.green;
        }
        else
        {
            Debug.Log("UP CHECK : FALSE");
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.left *
            (boxCollider2d.bounds.extents.y), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(0, boxCollider2d.bounds.extents.x + extraHeightText, 0), Vector2.right *
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
        if (downCheckScript.isTouching())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void rightClimbOnWall()//**************************************************************************************************************************************
    {
        if (rightCheck() && !isClimbing())
        {
            //animator.SetInteger("wallClimbSide", 2323);
            transform.Rotate(0f, 0f, 90f);
            turnOffGravity();
        }
    }

    private void upToDownWallMoveRight()
    {
        if ((isClimbing()))
        {
            turnOffGravity();
            doNotClimbTooFarWhileUpToDownWallMoveRight();
            transform.position += new Vector3(0, MOVEMENT, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotClimbTooFarWhileUpToDownWallMoveRight()
    {
        if (!downAheadCheckPoint() && Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0, MOVEMENT, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotClimbTooFarWhileUpToDownWallMoveLeft(float wallMovement)
    {
        if (topBehindCheckPoint() == false && Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
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

    public void teleportPlayerAnt()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        transform.position = new Vector2(x + 1.32f, y + 1f);
        /*
        Transform teleportationPosition = findDestinationTeleport();
        PlayerAnt.transform.position = new Vector2(teleportationPosition.position.x, teleportationPosition.position.y);
        */
    }

    public Transform findDestinationTeleport()
    {
        for(int i=0;i<NUMBER_OF_TELEPORTS_PAIRS;i++)
        {
            Vector2 positionOfTeleport1 = teleports[i].getTeleport1().transform.position;
            Vector2 positionOfTeleport2 = teleports[i].getTeleport2().transform.position;
            Vector2 antPosition = PlayerAnt.transform.position;

            if (Vector2.Distance(antPosition, positionOfTeleport1) <= distanceFromStartingTeleport)
            {
                Debug.Log("Start Teleport number: " + i);
                return teleports[i].getTeleport2().transform;
            }
            if (Vector2.Distance(antPosition, positionOfTeleport2) <= distanceFromStartingTeleport)
            {
                Debug.Log("Start Teleport number: " + i);
                return teleports[i].getTeleport1().transform;
            }
        }
        return null;
    }

    public void setTeleportsInArray()
    {
        for(int i = 1; i <= NUMBER_OF_TELEPORTS_PAIRS; i++)
        {
            string teleport1Name = "TELEPORTER_" + i.ToString() + "_1";
            string teleport2Name = "TELEPORTER_" + i.ToString() + "_2";
            Transform teleport1 = GameObject.FindGameObjectWithTag(teleport1Name).GetComponent<Transform>();
            Transform teleport2 = GameObject.FindGameObjectWithTag(teleport2Name).GetComponent<Transform>();
            teleports.Add(new teleports_pair(teleport1, teleport2));
        }
    }
    
    public void detachFromWall()
    {
        if(isClimbing())
        {
            if((rightCheck())&&Input.GetKey(KeyCode.F))
            {
                turnOnGravity();
                animator.SetInteger("wallClimbSide", 1234);
            }
        }
    }

}