using UnityEngine;

public class ant_movement : MonoBehaviour
{
    [Header("Movement")]
    public float MovementSpeed = 10;
    public float JumpForce = 10;

    private bool m_FacingRight = true;


    private Rigidbody2D PlayerAnt;
   private void Start()
    {
        PlayerAnt = GetComponent<Rigidbody2D>();
    }

  
    private void Update()
    {
        Move();
        Jump();
        

        //Cursor.lockState = CursorLockMode.Locked;  //Placeholder na celownik
      //  Cursor.visible = false;
    }

    private void Move()
    {
        
        var movement = Input.GetAxisRaw("Horizontal");             //poruszanie sie lewo/prawo
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;
        if(movement > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if(movement < 0 && m_FacingRight)
        {
            Flip();
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && Mathf.Abs(PlayerAnt.velocity.y) < 0.001f)       //skok
        {
            PlayerAnt.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
    }
}
