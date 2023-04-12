using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Spider : MonoBehaviour
{
    [Header("State")]
    public SpiderState State = SpiderState.Rest;
    public float RestCooldown = 0f;
    public float WebCurrentLength = 0f;
    [Header("Settings")]
    [Tooltip("Local position of rope's beginning.")]
    public Vector2 WebStartPos = Vector2.zero;
    [Tooltip("Maximum length of a web rope spider can weave.")]
    public float WebMaxLength = 10f;
    public float WebTriggerWidth = 0.35f;
    [Tooltip("Speed of weaving a web rope in units per second.")]
    public float WeaveSpeed = 0.65f;
    [Tooltip("Speed of pulling a web rope by a spider in units per second.")]
    public float PullSpeed = 0.85f;
    public float RestDelay = 5f;
    public bool HuntOnAwake = false;
    [Header("Components")]
    public Animator Animator;
    public LineRenderer LineRenderer;
    public BoxCollider2D Trigger;


    private void UpdateTrigger()
    {
        throw new NotImplementedException();
    }
    private void UpdateLine()
    {
        throw new NotImplementedException();
    }
}
public enum SpiderState
{
    Rest,
    Weave,
    Hunt,
    Pull,
    Digest
}
