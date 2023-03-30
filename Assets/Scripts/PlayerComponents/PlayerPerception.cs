using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntCorp;

public class PlayerPerception : PlayerModule
{
    public bool IsGrounded { get; private set; }
    public EnvironmentMaterial? GroundMaterial { get; private set; }

    public override void OnFixedUpdate(TimeState time){
        GroundCheck();
    }
    private void GroundCheck(){
        Transform t;
        EnvironmentMaterial m;
        bool flag = false;
        if (Physics2DEx.TryBoxCast(out t, Player.transform.position + -transform.up, new Vector2(1.0f, 0.05f), Player.transform.eulerAngles.z, -Player.transform.up, 1f)){
            m = t.GetComponent<EnvironmentMaterial>();
            if (m != null){
                if ((m.StickAxis & Player.Controller.Axis) != 0){
                    flag = true;
                    IsGrounded = flag;
                    GroundMaterial = m;
                }
            }
        }
        if (!flag){
            GroundMaterial = null;
            IsGrounded = false;
        }
    }
}
