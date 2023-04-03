using AntCorp;
using UnityEngine;

public class SmallAnt : MonoBehaviour
{
    [Header("Components")]
    public Animator Animator;
    public Animator MudAnimator;
    public Rigidbody2D UseRigidbody;
    public CameraFocusZone FocusZone;
    public uGUI_Popup Popup;
    [Header("Options")]
    public bool CanMove = true;
    public float MoveSpeed;
    public KeyCode ComboKey = KeyCode.H;
    [Header("Mash")]
    public float EndMash = 5f;
    public float MashDelay = .5f;
    public float MashDecreaseRate = 0.5f;
    [Header("State")]
    public int Direction = -1;
    public bool FacingRight = false;
    public bool SpottedPlayer = false;
    public bool ComboDone = false;
    public float CurrentMash = 0;

    private Vector3 localScale;
    bool pressed;

    // Start is called before the first frame update
    private void Start()
    {
        localScale = transform.localScale;
        UseRigidbody = GetComponent<Rigidbody2D>();
        MudAnimator.SetInteger("mash", 0);
        Animator.SetInteger("mash1", 0);

    }

    private void Update()
    {
        if (SpottedPlayer)
        {
            if(CurrentMash > 0f)
            {
                CurrentMash -= MashDecreaseRate * Time.deltaTime;
            }
            if (Input.GetKeyDown(ComboKey) && !pressed)
            {
                pressed = true;
                CurrentMash += MashDelay;
                MudAnimator.SetInteger("mash", 1);
                Animator.SetInteger("mash1", 1);
            } else if (Input.GetKeyUp(ComboKey))
            {
                pressed = false;
                MudAnimator.SetInteger("mash", 2);
            }
        }
        else
        {
            CurrentMash = 0f;
            Animator.SetInteger("mash1", 0);
            MudAnimator.SetInteger("mash", 0);
        }
        if (CurrentMash > EndMash)
        {
            End();
        }       
    }


    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 velocity = UseRigidbody.velocity;
        velocity.x = (CanMove && !SpottedPlayer) ? Direction * MoveSpeed : 0f;
        UseRigidbody.velocity = velocity;
    }
    private void End()
    {
        ItemManager.Main.SmallAnts++;
        Destroy(FocusZone);
        Destroy(gameObject);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (Direction > 0)
            FacingRight = true;
        else if (Direction < 0)
            FacingRight = false;

        if (((FacingRight) && (localScale.x < 0)) || ((!FacingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }

    #region Trigger events
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wallForSmallAnts"))
        {
            Direction *= -1;
        }
        else if (collision.CompareTag(Tag.Player))
        {
            SpottedPlayer = true;
            Popup.Show();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            SpottedPlayer = false;
            Popup.Hide();
        }
    }
    #endregion
}
