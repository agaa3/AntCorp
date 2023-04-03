using UnityEngine;

public class PlayerPerception : PlayerModule
{
    public bool IsGrounded { get; private set; }
    public EnvironmentMaterial? GroundMaterial { get; private set; }

    public override void OnFixedUpdate(TimeState time){
        GroundCheck();
    }
    private void GroundCheck(){
        // Creating new array every frame is very bad solution. Too bad!
        // Consider using array pooling.
        RaycastHit2D[] hits = new RaycastHit2D[8];
        int c = Physics2D.BoxCast(Player.transform.position, new Vector2(1.0f, 0.05f), 0f, -transform.up, default, hits, 1f);
        if (c > 0)
        {
            foreach (RaycastHit2D h in hits)
            {
                Transform t = h.transform;
                EnvironmentMaterial m;
                if (t != null)
                {
                    if (t.TryGetComponent(out m))
                    {
                        if (m.CanStick(Player.Controller.Axis))
                        {
                            IsGrounded = true;
                            GroundMaterial = m;
                            return;
                        }
                    }
                }
            }
        }
        IsGrounded = false;
        GroundMaterial = null;
    }
}
