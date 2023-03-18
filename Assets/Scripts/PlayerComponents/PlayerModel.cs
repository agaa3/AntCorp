using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerModel : PlayerComponent
{
    public Animator Animator;
    private PlayerController controller => Player.Controller;

    private void Update()
    {
        Animator.SetBool("IsMoving", controller.IsMoving);
    }

    private void OnEnable()
    {
        controller.PerformedTurn += TriggerTurn;
    }
    private void OnDisable()
    {
        controller.PerformedTurn -= TriggerTurn;
    }

    private void TriggerTurn(bool isInside)
    {
        Animator.SetTrigger(String.Format("BeginTurn{0}side", isInside ? "In" : "Out"));
    }
}
