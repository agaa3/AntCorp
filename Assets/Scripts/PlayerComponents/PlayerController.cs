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
    public bool CanFlip = true;
    public bool IsMidTurn = false;
    public bool IsFacingRight = true;
    public bool IsMidTeleport = false;
    public bool IsMoving = false;
    [Header("Parameters")]
    public float MoveSpeed = 2f;
    public float InsideTurnDuration = 0.25f;
    public float OutsideTurnDuration = 0.4f;

    /// <summary>
    /// True if inside, false if outside
    /// </summary>
    public Action<bool> PerformedTurn;

    Vector2 gravityOverride = Vector2.zero;
    float moveInput = 0.0f;




    public void Flip()
    {
        if (CanFlip && !IsMidTurn && !Sensors.HasGroundBelow() && Sensors.HasGroundAbove() && Axis == HeadAxis.Ceiling)
        {
            SetAxis(HeadAxis.Floor, true);
            FaceFlip();
        }
    }

    #region Axis
    public void SetAxis(HeadAxis axis, bool updateGravity)
    {
        Vector3 ax = transform.eulerAngles;
        switch (axis)
        {
            case HeadAxis.Floor:
                ax.z = 0;
                break;
            case HeadAxis.Ceiling:
                ax.z = 180;
                break;
            case HeadAxis.WallLeft:
                ax.z = -90;
                break;
            case HeadAxis.WallRight:
                ax.z = 90;
                break;
        }
        transform.eulerAngles = ax;
        Axis = axis;
        if (updateGravity)
        {
            UpdateGravityOverride();
        }
    }
    #endregion

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
        EnsureGrounded();
    }
    #endregion

    private void Move()
    {
        if (CanMove && !IsMidTurn && Sensors.HasGroundBelow())
        {
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
            move *= MoveSpeed;
            move *= Time.fixedDeltaTime;
            transform.position += move;
            if (move.magnitude > float.Epsilon)
            {
                IsMoving = true;
            }
            else
            {
                IsMoving = false;
            }
        }
        else
        {
            IsMoving = false;
        }
    }

    private void CheckSensors()
    {
        if (!IsMidTurn)
        {
            if (Sensors.CanTurnInside())
            {
                IsMidTurn = true;
                if (IsFacingRight)
                {
                    StartCoroutine(TurnInside(false));
                }
                else
                {
                    StartCoroutine(TurnInside(true));
                }
            }
            else if (Sensors.CanTurnOutside())
            {
                IsMidTurn = true;
                if (IsFacingRight)
                {
                    StartCoroutine(TurnOutside(true));
                }
                else
                {
                    StartCoroutine(TurnOutside(false));
                }
            }
        }
    }

    private void EnsureGrounded()
    {
        if (!IsMidTurn && !Sensors.HasGroundBelow())
        {
            if (Axis != HeadAxis.Floor)
            {
                if (Axis == HeadAxis.Ceiling)
                {
                    SetFloorGravity();
                }
                else
                {
                    SetAxis(HeadAxis.Floor, true);
                }
            }
        }
    }

    private IEnumerator TurnInside(bool right)
    {
        Debug.Log("Turn Inside");
        Vector3 pos = transform.position;
        pos.x = Mathf.Round(pos.x * 2) / 2;
        pos.y = Mathf.Round(pos.y * 2) / 2;
        transform.position = pos;
        float timer = 0;
        bool flag = false;
        PerformedTurn?.Invoke(true);
        while (timer < InsideTurnDuration)
        {
            timer += Time.deltaTime;
            if (timer >= InsideTurnDuration * 0.77f && !flag)
            {
                if (right)
                {
                    TurnRight();
                }
                else
                {
                    TurnLeft();
                }
                flag = true;
            }
            yield return null;
        }
        IsMidTurn = false;
    }
    private IEnumerator TurnOutside(bool right)
    {
        Vector2 pos1 = transform.position;
        Vector2 pos2 = pos1;
        pos2.x = Mathf.Round(pos2.x * 2) / 2;
        pos2.y = Mathf.Round(pos2.y * 2) / 2;
        pos2 += ((Vector2)transform.right * (IsFacingRight ? 1 : -1));
        Vector2 pos3 = pos2 + -((Vector2)transform.up);
        float timer = 0;
        bool flag = false;
        PerformedTurn?.Invoke(false);
        while (timer < OutsideTurnDuration)
        {
            timer += Time.deltaTime;
            UseRigidbody.MovePosition(Vector2.Lerp(pos1, pos2, (timer / OutsideTurnDuration)));
            yield return null;
        }
        flag = true;
        if (right)
        {
            TurnRight();
        }
        else
        {
            TurnLeft();
        }
        UseRigidbody.MovePosition(pos2);
        timer = 0;
        while (timer < OutsideTurnDuration)
        {
            timer += Time.deltaTime;
            UseRigidbody.MovePosition(Vector2.Lerp(pos2, pos3, (timer / OutsideTurnDuration)));
            yield return null;
        }
        UseRigidbody.MovePosition(pos3);
        IsMidTurn = false;
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
        if (CanFace && !IsMidTurn && Sensors.HasGroundBelow())
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
        gravityOverride.y = 2 * -Physics2D.gravity.y;
    }
    private void ApplyGravityOverride()
    {
        UseRigidbody.AddForce(gravityOverride * UseRigidbody.gravityScale);
    }
    #endregion

    private void UpdateInputs()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        if ((Axis == HeadAxis.WallLeft || Axis == HeadAxis.WallRight) && Math.Abs(moveInput) < float.Epsilon)
        {
            moveInput = Input.GetAxisRaw("Vertical");
        }

        if (Input.GetButtonDown("Jump"))
        {
            Flip();
        }
    }
}