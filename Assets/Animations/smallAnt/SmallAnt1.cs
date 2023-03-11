using UnityEngine;

public class SmallAnt1 : MonoBehaviour
{
    private float dirX;
    private float moveSpeed;
    private Rigidbody2D rb;
    private bool facingRight = false;
    private Vector3 localScale;
    public KeyCode combo;
    public bool comboDone = false;
    public Animator animatorAnt;

    public float mashDelay = .5f;
    public float mash;
    bool pressed;
    public float collides = 0;

    // Start is called before the first frame update
    private void Start()
    {
        localScale = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        dirX = -1f;
        moveSpeed = 0.5f;
        mash = 0f;
        animatorAnt.SetInteger("mash1", 0);

    }

    private void Update()
    {
        if (FriendsManager.instance.collides == true)
        {
            collides = 1;
			moveSpeed = 0;
			
			if(Input.GetKeyDown(combo) && !pressed) {
				mash = 6;
			}
            
        }
        else
        {
            collides = 0;
            mash = 0f;
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
        if (collision.CompareTag("wallForSmallAnts"))
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
