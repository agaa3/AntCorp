// Przy budowaniu mapy nale¿y umiescic na ka¿dym rogu teleport , inacze findDestinationTeleport()/findStartTeleport() zwraca null i gierka sie wysypie
//Sterowanie:
//  E: wejœcie na œcianê
//  R: wdrapanie sie na górê 
//  F: oderwanie siê od œciany

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ant_movement : MonoBehaviour
{
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
    private float MovementSpeed = 2;

    private bool m_FacingRight = true;

    public static int NUMBER_OF_TELEPORTS=8;
    public static float distanceFromStartingTeleport = 0.5f;
    public static float distanceFromDestinationTeleport = 1.9f;
    private Transform[] teleports=new Transform[NUMBER_OF_TELEPORTS];

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
        setTeleportsInArray(teleports);
    }


    private void Update()
    {
        if (couldAntMove())
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
            doNotBuggingOnLeftWall(movement);
            doNotBuggingOnRightWall(movement);
            doNotBuggingOnFloor(movement);
            flippingWhenAntIsOnTheFloor(movement);
        }

    }

    private void climb()
    {
        var movement = Input.GetAxisRaw("Horizontal");
        rightClimbOnWall();
        leftClimbOnWall();
        upToDownWallMoveRight();
        upToDownWallMoveLeft();
        rightClimbFromWallToSurface();
        leftClimbFromWallToSurface();
        detachFromWall();
    }
    





    //                                                 
    // bunch of some little functions bellow         \    /     \    /     \    /     \    /     \    /
    //                                                \  /       \  /       \  /       \  /       \  /
    //                                                 \/         \/         \/         \/         \/



    void rightClimbFromWallToSurface()
    {
        if (isClimbing()&&!topAheadCheckPoint()&&rightCheck()&&Input.GetKey(KeyCode.R))
        {
            teleportPlayerAnt();
            animator.SetInteger("wallClimbSide", 2137);
            turnOnGravity();
        }      
    }

    void leftClimbFromWallToSurface()
    {
        if (isClimbing() && !topBehindCheckPoint() && leftCheck() && Input.GetKey(KeyCode.R))
        {
            teleportPlayerAnt();
            Flip();
            animator.SetInteger("wallClimbSide", 2137);
            turnOnGravity();
        }
    }

    private bool isClimbing()
    {
        if (PlayerAnt.gravityScale == 0)
            return true;
        else
            return false;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        transform.Rotate(0f, 180f, 0f);
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
            return true;
        }
        else
        {
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
            rayColor = Color.green;
        }
        else
        {
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

    private void rightClimbOnWall()
    {
        if (rightCheck() == true && Input.GetKey(KeyCode.E) && !isClimbing())
        {
            animator.SetInteger("wallClimbSide", 1);
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

    private bool couldAntMove()
    {
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("rightClimb") &&
            !this.animator.GetCurrentAnimatorStateInfo(0).IsName("leftClimb"))
        {
            return true;
        }
        return false;
    }
    public void teleportPlayerAnt()
    {
            PlayerAnt.transform.position = new Vector2(findDestinationTeleport().position.x, findDestinationTeleport().position.y);
    }

    public Transform findStartTeleport()
    {
        for(int i=0;i<NUMBER_OF_TELEPORTS;i++)
        {
            if (Vector2.Distance(PlayerAnt.transform.position, teleports[i].position) <= distanceFromStartingTeleport)
            {
                Debug.Log("Start Teleport number: " + i);
                return teleports[i];
            }
        }
        return null;
    }

    public Transform findDestinationTeleport()
    {
        for (int i = 0; i < NUMBER_OF_TELEPORTS; i++)
        {
            float distanceBetweenAntAndTeleport = Vector2.Distance(PlayerAnt.transform.position, teleports[i].position);
            float distanceBetweenAndAndStartingTeleport = Vector2.Distance(PlayerAnt.transform.position, findStartTeleport().transform.position);
            if ((distanceBetweenAntAndTeleport <= distanceFromDestinationTeleport) && (distanceBetweenAntAndTeleport != distanceBetweenAndAndStartingTeleport)) 
            {
                Debug.Log("Distance: " + Vector2.Distance(PlayerAnt.transform.position, teleports[i].position));
                Debug.Log("Teleport number: " + i);
                return teleports[i];
            }
        }
        return null;
    }


    public void setTeleportsInArray(Transform[] teleports)
    {
        teleports[0] = GameObject.FindGameObjectWithTag("TELEPORTER_1_1").GetComponent<Transform>();
        teleports[1] = GameObject.FindGameObjectWithTag("TELEPORTER_1_2").GetComponent<Transform>();
        teleports[2] = GameObject.FindGameObjectWithTag("TELEPORTER_2_1").GetComponent<Transform>();
        teleports[3] = GameObject.FindGameObjectWithTag("TELEPORTER_2_2").GetComponent<Transform>();
        teleports[4] = GameObject.FindGameObjectWithTag("TELEPORTER_3_1").GetComponent<Transform>();
        teleports[5] = GameObject.FindGameObjectWithTag("TELEPORTER_3_2").GetComponent<Transform>();
        teleports[6] = GameObject.FindGameObjectWithTag("TELEPORTER_4_1").GetComponent<Transform>();
        teleports[7] = GameObject.FindGameObjectWithTag("TELEPORTER_4_2").GetComponent<Transform>();
    }

    public void detachFromWall()
    {
        if(isClimbing())
        {
            if((leftCheck()||rightCheck())&&Input.GetKey(KeyCode.F))
            {
                turnOnGravity();
                animator.SetInteger("wallClimbSide", 2137);
            }
        }
    }
}