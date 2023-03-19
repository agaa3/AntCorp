using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerModel : PlayerModule
{
    public Animator Animator;
    public PlayerController Controller => Player.Controller;


    public override void OnInitialize(TimeState time)
    {
        Animator = GetComponent<Animator>();
        Controller.PerformedTurn += TriggerTurn;
    }
    public override void OnUpdate(TimeState time)
    {
        Animator.SetBool("IsMoving", Controller.IsMoving);
    }

    private void TriggerTurn(bool isInside)
    {
        Animator.SetTrigger(String.Format("BeginTurn{0}side", isInside ? "In" : "Out"));
    }
}
