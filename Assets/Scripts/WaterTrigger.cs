using AntCorp;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public Vector2 PushForce => new Vector2(0, 15);
    public const float PushSpeedLimit = -0.2f;
    public float DamagePerTick = 25;
    public float TickRate = 1f;

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            InvokeRepeating(nameof(DamagePlayer), 0.0f, TickRate);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            CancelInvoke(nameof(DamagePlayer));
        }
    }
    private void DamagePlayer()
    {
        LivingMixin m = Player.Main.Mixin;
        if (m.IsAlive)
        {
            m.DealDamage(DamagePerTick);
        }
        else
        {
            CancelInvoke(nameof(DamagePlayer));
        }
    }
}
