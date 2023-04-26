using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerModel : PlayerModule
{
    public Animator Animator;
    public SpriteRenderer Renderer;
    public PlayerController Controller => Parent.Controller;

    public Color DamageColor;
    public Color DeadColor;

    public bool ShownDamage;
    public float ShowDamageTime = 0.25f;


    public override void OnInitialize(TimeState time)
    {
        Animator = GetComponent<Animator>();
        Renderer = GetComponent<SpriteRenderer>();

        Controller.PerformedTurn += OnTurn;
        Parent.Mixin.OnTakeDamage += OnTakeDamage;
    }
    public override void OnUpdate(TimeState time)
    {
        Animator.SetBool("IsMoving", Controller.IsMoving);
        if (!ShownDamage)
        {
            Renderer.color = Parent.Mixin.IsAlive ? Color.white : DeadColor;
        }
    }

    private void OnTakeDamage(float old, float nw)
    {
        if (!ShownDamage)
        {
            StartCoroutine(ShowDamage());
        }
    }
    private void OnTurn(bool isInside)
    {
        Animator.SetTrigger(String.Format("BeginTurn{0}side", isInside ? "In" : "Out"));
    }

    private IEnumerator ShowDamage()
    {
        float t = 0.0f;
        ShownDamage = true;
        Renderer.color = DamageColor;
        while (t < ShowDamageTime)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Renderer.color = Color.white;
        ShownDamage = false;
    }
}
