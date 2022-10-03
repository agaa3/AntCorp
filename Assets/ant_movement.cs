// Przy budowaniu mapy nale¿y umiescic na ka¿dym rogu teleport , inacze findDestinationTeleport()/findStartTeleport() zwraca null i gierka sie wysypie
//Sterowanie:
//  E: wejœcie na œcianê
//  R: wdrapanie sie na górê 
//  F: oderwanie siê od œciany
//TODO:
//switch na pocz¹tku i koñcu animacji
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

    topAheadDetect topAheadDetectScript;
    downAheadDetect downAheadDetectScript;
    topBehindDetect topBehindDetectScript;
    downBehindDetect downBehindDetectScript;

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
        setTeleportsInArray();
    }

    private void Update()
    {
        if (couldAntMove)
        {
            Move();
            climb();
        }
    }



    private void Move()
    {

        if (!isClimbing())
        {
            var movement = Input.GetAxisRaw("Horizontal");
            transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
            doNotBuggingWhileWalking(movement);
            doNotFallDownFromPlatform(movement);
            flippingWhenAntIsOnTheFloor(movement);
        }

    }

    private void climb()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        var verticalMovement = Input.GetAxisRaw("Vertical");
        Debug.Log("aaaaaaaaa");
        goDownFromRightWallToFloor();
        rightClimbOnWall();
        leftClimbOnWall();
        upToDownWallMoveRight();
        upToDownWallMoveLeft();
        rightClimbFromWallToSurface();
        leftClimbFromWallToSurface();
        doNotBuggingOnRightWallWhenAntGoingDown(movement);
        detachFromWall();
        rightGoDown();
        
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
        if (isClimbing()&&!topAheadCheckPoint()&&rightCheck()&&Input.GetKey(KeyCode.R))
        {
            //teleportPlayerAnt();
            animator.SetInteger("wallClimbSide", 2137);
            //turnOnGravity();
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
    void leftClimbFromWallToSurface()
    {
        if (isClimbing() && !topBehindCheckPoint() && leftCheck() && Input.GetKey(KeyCode.R))
        {
            //teleportPlayerAnt();
            //Flip();
            animator.SetInteger("wallClimbSide", 2137);
            //turnOnGravity();
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
    
    private void doNotBuggingOnRightWallWhenAntGoingDown(float movement)
    {
        if(isClimbing() && downAheadCheckPoint() && Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, movement, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotBuggingWhileWalking(float movement)
    {
        doNotBuggingOnLeftWall(movement);
        doNotBuggingOnRightWall(movement);
        doNotBuggingOnFloor(movement);
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
            Debug.Log("RIGHT CHECK: TRUE");
            return true;
        }
        else
        {
            Debug.Log("RIGHT CHECK: FALSE");
            return false;
        }
    }

    private bool leftCheck()
    {
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0, Vector2.left, extraHeightText, layerMask);
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
        RaycastHit2D Toutch = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, layerMask);

        Color rayColor;
        if (Toutch.collider != null)
        {
            Debug.Log("DOWN CHECK : TRUE");
            rayColor = Color.green;
        }
        else
        {
            Debug.Log("DOWN CHECK : FALSE");
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

    private void rightClimbOnWall()//**************************************************************************************************************************************
    {
        if (rightCheck() && Input.GetKey(KeyCode.E) && !isClimbing())
        {
            animator.SetInteger("wallClimbSide", 2323);
            turnOffGravity();
        }
    }

    private void leftClimbOnWall()
    {
        if (leftCheck() && Input.GetKey(KeyCode.E))
        {
            if (!isClimbing())
            {
                Flip();
                animator.SetInteger("wallClimbSide", 2);
                turnOffGravity();
            }
        }
    }

    private void upToDownWallMoveRight()
    {
        if ((rightCheck() && isClimbing()))
        {
            turnOffGravity();
            var wallMovement = Input.GetAxisRaw("Vertical");
            doNotClimbTooFarWhileUpToDownWallMoveRight(wallMovement);
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotClimbTooFarWhileUpToDownWallMoveRight(float wallMovement)
    {
        if (!topAheadCheckPoint() && Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotClimbTooFarWhileUpToDownWallMoveLeft(float wallMovement)
    {
        if (topBehindCheckPoint() == false && Input.GetKey(KeyCode.W))
        {
            transform.position -= new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
        }
    }
    private void upToDownWallMoveLeft()
    {
        if ((leftCheck() && isClimbing()))
        {
            turnOffGravity();
            var wallMovement = Input.GetAxisRaw("Vertical");
            doNotClimbTooFarWhileUpToDownWallMoveLeft(wallMovement);
            transform.position += new Vector3(0, wallMovement, 0) * Time.deltaTime * MovementSpeed;
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

    /*private bool couldAntMove()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("rightClimb") &&
            !this.animator.GetCurrentAnimatorStateInfo(0).IsName("leftClimb"))
        {
            return true;
        }
        return false;
    }*/
    public void teleportPlayerAnt()
    {
        Transform teleportationPosition = findDestinationTeleport();
        PlayerAnt.transform.position = new Vector2(teleportationPosition.position.x, teleportationPosition.position.y);
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
            if((leftCheck()||rightCheck())&&Input.GetKey(KeyCode.F))
            {
                turnOnGravity();
                animator.SetInteger("wallClimbSide", 1234);
            }
        }
    }

    private void doNotBuggingOnLeftWall(float movement)
    {
        if (leftCheck() == true && Input.GetKey(KeyCode.A))
        {
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotBuggingOnRightWall(float movement)
    {
        if (rightCheck() == true && Input.GetKey(KeyCode.D))
        {
            transform.position -= new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        }
    }

    private void doNotBuggingOnFloor(float movement)
    {
        if (downCheck() == true && Input.GetKey(KeyCode.S))
        {
            transform.position -= new Vector3(0, movement, 0) * Time.deltaTime * MovementSpeed;
        }
    }
}