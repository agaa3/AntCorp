using System;
using UnityEngine;

public class PlayerPerception : PlayerModule
{
    public bool IsGrounded { get; private set; }
    public GroundInfo GroundInfo { get; private set; } = new GroundInfo();

    [Header("Forward")]
    public float FwdCheckHeight = 0.01f;
    public float FwdCheckDistance = 0.05f;
    [Header("Bottom")]
    public float BtmCheckGap = 1f / 16f;

    public override void OnFixedUpdate(TimeState time){
        GroundCheck();
    }
    public bool FindTurnSurface()
    {
        bool flag = false;

        var fwdHit = Physics2D.Raycast(Parent.transform.position + Parent.Body.Right * 1f + -Parent.Body.Up * 0.43f + Parent.Body.Up * FwdCheckHeight, Parent.Body.Right, FwdCheckDistance);
        if (fwdHit.collider != null)
        {
            return true;
        }
        throw new NotImplementedException();
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
                    GroundInfo = new(m, groundHits[i].normal);
                    return;
                }
            }
        }
        IsGrounded = false;
        GroundInfo = default;
    }

    private RaycastHit2D[] groundHits = new RaycastHit2D[8];
}
