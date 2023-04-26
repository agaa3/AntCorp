using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : PlayerModule
{
    public bool CanMove = false;
    public float MoveSpeed = 1.0f;

    public override void OnFixedUpdate(TimeState time)
    {
        if (CanMove){
            throw new NotImplementedException();
        }
    }
}
