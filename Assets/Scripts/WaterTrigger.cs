using AntCorp;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public Vector2 PushForce => new Vector2(0, 15);
    public const float PushSpeedLimit = -0.2f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            Rigidbody2D r = Player.Main.UseRigidbody;
            if (r.velocity.y < PushSpeedLimit)
            {
                r.AddForce(PushForce);
            }
        }
    }
}
