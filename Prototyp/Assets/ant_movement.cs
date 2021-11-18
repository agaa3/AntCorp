
using UnityEngine;

public class ant_movement : MonoBehaviour
{
    public float MovementSpeed = 1;
    public float JumpForce = 1;

    private Rigidbody2D _rigibody;
   private void Start()
    {
        _rigibody = GetComponent<Rigidbody2D>();
    }

  
    private void Update()
    {
        var movement = Input.GetAxis("Horizontal");
        transform.position += new Vector3(movement, 0, 0) * Time.deltaTime * MovementSpeed;

        if(Input.GetButtonDown("Jump")&& Mathf.Abs(_rigibody.velocity.y)<0.001f)
        {
            _rigibody.AddForce(new Vector2(0, JumpForce),ForceMode2D.Impulse);
        }
    }
}
