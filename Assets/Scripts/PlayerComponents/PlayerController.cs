using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[DisallowMultipleComponent]
public class PlayerController : PlayerComponent
{
    [Header("Components")]
    public Rigidbody2D UseRigidbody;
    public BoxCollider2D Collider;
    public AntMoveSensor Sensors = new AntMoveSensor();
    [Header("Flags")]
    public HeadAxis Axis = HeadAxis.Floor;
    public bool CanMove = true;
    public bool CanFace = true;
    public bool IsMidTurn = false;
    public bool IsFacingRight = true;
    public bool IsMidTeleport = false;
    [Header("Parameters")]
    public float MoveSpeed = 2f;
    public float TurnDuration = 0.25f;

    Vector2 gravityOverride = Vector2.zero;
    float moveInput = 0.0f;




    #region Facing
    /// <summary>
    /// Sets ant's facing direction to the right
    /// </summary>
    public void FaceRight()
    {
        IsFacingRight = true;
        Vector3 scale = transform.localScale;
        scale.x = 1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Sets ant's facing direction to the left
    /// </summary>
    public void FaceLeft()
    {
        IsFacingRight = false;
        Vector3 scale = transform.localScale;
        scale.x = -1;
        transform.localScale = scale;
    }

    /// <summary>
    /// Flips ant's facing direction
    /// </summary>
    public void FaceFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x = -scale.x;
        transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        UseRigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<BoxCollider2D>();
        //Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetFloorGravity();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void FixedUpdate()
    {
        ApplyGravityOverride();
        CheckSensors();
        UpdateFacing();
        Move();
    }
    #endregion

    private void Move()
    {
        if (!CanMove || IsMidTurn)
        {
            return;
        }

        Vector3 move = Vector3.zero;
        switch (Axis)
        {
            case HeadAxis.Floor:
                move.x = moveInput;
                break;
            case HeadAxis.Ceiling:
                move.x = -moveInput;
                break;
            case HeadAxis.WallLeft:
                move.y = -moveInput;
                break;
            case HeadAxis.WallRight:
                move.y = moveInput;
                break;
        }
        transform.position += (move * MoveSpeed * Time.fixedDeltaTime);
    }
    
    private void CheckSensors()
    {
        if (Sensors.CanTurnInside())
        {
            if (IsFacingRight)
            {
                TurnLeft();
                //StartCoroutine(TurnInsideLeft());
            }
            else
            {
                TurnRight();
                //StartCoroutine(TurnInsideRight());
            }
        }
    }

    private IEnumerator TurnInsideLeft()
    {
        if (!IsMidTurn)
        {
            IsMidTurn = true;
            yield return new WaitForSeconds(TurnDuration);
            TurnLeft();
            IsMidTurn = false;
        }
    }
    private IEnumerator TurnInsideRight()
    {
        if (!IsMidTurn)
        {
            IsMidTurn = true;
            yield return new WaitForSeconds(TurnDuration);
            TurnRight();
            IsMidTurn = false;
        }
    }

    private void TurnLeft()
    {
        int num = (int)Axis;
        num--;
        if (num < 0)
        {
            num = 3;
        }
        Axis = (HeadAxis)num;
        transform.Rotate(0, 0, 90);
        UpdateGravityOverride();
    }
    private void TurnRight()
    {
        int num = (int)Axis;
        num++;
        if (num > 3)
        {
            num = 0;
        }
        Axis = (HeadAxis)num;
        transform.Rotate(0, 0, -90);
        UpdateGravityOverride();
    }
    private void UpdateFacing()
    {
        if (CanFace && !IsMidTurn)
        {
            if (moveInput > float.Epsilon && !IsFacingRight)
            {
                FaceRight();
            }
            else if (moveInput < -float.Epsilon && IsFacingRight)
            {
                FaceLeft();
            }
        }
    }
    private void UpdateGravityOverride()
    {
        switch (Axis) 
        {
            case HeadAxis.Floor:
                SetFloorGravity();
                break;
            case HeadAxis.WallLeft:
                SetWallGravity(false);
                break;
            case HeadAxis.WallRight:
                SetWallGravity(true);
                break;
            case HeadAxis.Ceiling:
                SetCeilingGravity();
                break;
        }

    }

    #region Gravity manipulations
    private void SetFloorGravity()
    {
        gravityOverride = Vector2.zero;
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
    private void SetCeilingGravity()
    {
        gravityOverride.x = 0;
        gravityOverride.y = -Physics2D.gravity.y;
    }
    private void ApplyGravityOverride()
    {
        UseRigidbody.AddForce(gravityOverride);
    }
    #endregion

    private void UpdateInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if ((Axis == HeadAxis.WallLeft || Axis == HeadAxis.WallRight) && Math.Abs(moveInput) < float.Epsilon)
        {
            moveInput = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            TurnRight();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            TurnLeft();
        }
    }
}