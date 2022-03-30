using UnityEngine;

public class SmallAnt : MonoBehaviour
{
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    public KeyCode combo;
    public bool comboDone = false;
    public Animator animatorMud;
    public Animator animatorAnt;

    public float mashDelay = .5f;
    float mash;
    bool pressed;

    // Start is called before the first frame update
    private void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 0.5f;
        mash = 0f;
        animatorMud.SetInteger("mash", 0);
        animatorAnt.SetInteger("mash1", 0);

    }

    private void Update()
    {
        if (FriendsManager.instance.collides == true)
        {
            if(mash > 0f)
            {
                mash -= 0.5f * Time.deltaTime;
            }
            if (Input.GetKeyDown(combo) && !pressed)
            {
                pressed = true;
                mash += mashDelay;
                animatorMud.SetInteger("mash", 1);
                animatorAnt.SetInteger("mash1", 1);

                moveSpeed = 0;  
            }else if (Input.GetKeyUp(combo))
            {
                pressed = false;
                animatorMud.SetInteger("mash", 2);
            }
        }
        else
        {
            mash = 0f;
            animatorMud.SetInteger("mash", 0);
            animatorAnt.SetInteger("mash1", 0);

            moveSpeed = 0.5f;
        }
        if (mash > 5)
        {
            comboDone = true;
        }       
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Wall>())
        {
            dirX *= -1f;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
    }

    void LateUpdate()
    {
        CheckWhereToFace();
    }

    void CheckWhereToFace()
    {
        if (dirX > 0)
            facingRight = true;
        else if (dirX < 0)
            facingRight = false;

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            localScale.x *= -1;

        transform.localScale = localScale;
    }
}
