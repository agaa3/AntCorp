using UnityEngine;

public class PlayerPerception : PlayerModule
{
    public bool IsGrounded { get; private set; }
    public EnvironmentMaterial? GroundMaterial { get; private set; }

    public override void OnFixedUpdate(TimeState time){
        GroundCheck();
    }
    private void GroundCheck(){
        int c = Physics2D.BoxCastNonAlloc(origin: Parent.transform.position, size: new Vector2(1.0f, 0.05f), angle: 0f, direction: -transform.up, results: groundHits, distance: 0.5f, layerMask: Physics2D.DefaultRaycastLayers);
        for (int i = 0; i < c; i++)
        {
            Transform t = groundHits[i].transform;
            EnvironmentMaterial m;
            if (t.TryGetComponent(out m))
            {
                if (m.CanStick(Parent.Controller.Axis))
                {
                    IsGrounded = true;
                    GroundMaterial = m;
                    return;
                }
            }
        }
        IsGrounded = false;
        GroundMaterial = null;
    }

    private RaycastHit2D[] groundHits = new RaycastHit2D[8];
}
