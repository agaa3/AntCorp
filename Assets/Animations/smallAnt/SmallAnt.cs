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
    }

    private void Update()
    {
        if (FriendsManager.instance.collides == true)
        {
            if(mash > 0f)
            {
                mash -= Time.deltaTime;
            }
            if (Input.GetKeyDown(combo) && !pressed)
            {
                pressed = true;
                mash += mashDelay;
            }else if (Input.GetKeyUp(combo))
            {
                pressed = false;
            }
        }
        else
        {
            mash = 0f;
        }
        if(mash > 3)
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
