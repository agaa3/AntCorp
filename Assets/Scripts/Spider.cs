using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Spider : MonoBehaviour
{
    public Vector3 MagnetPoint => transform.position + (Vector3)WebStartPos + Vector3.down * WebCurrentLength;
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


    #region Unity Callbacks
    private void Awake()
    {
        if (HuntOnAwake)
        {
            State = SpiderState.Hunt;
            WebCurrentLength = WebMaxLength;
            UpdateLine();
            UpdateTrigger();
        }
    }
    private void Update()
    {
        Tick();
        UpdateLine();
        UpdateTrigger();
    }

    #endregion
    private void Tick()
    {

    }
    private void UpdateTrigger()
    {
        Trigger.size = new Vector2(WebTriggerWidth, WebCurrentLength);
        Trigger.offset = WebStartPos + new Vector2(0, -WebCurrentLength*0.5f);
    }
    private void UpdateLine()
    {
        Vector3 wp = transform.position + (Vector3)WebStartPos;
        LineRenderer.SetPosition(0, wp);
        LineRenderer.SetPosition(1, wp + Vector3.down * WebCurrentLength);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Vector3 wp = transform.position + (Vector3)WebStartPos;
        Vector3 ep = wp + Vector3.down * WebMaxLength;
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(wp, ep);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(wp, Vector3.one * 0.5f);
        Gizmos.DrawWireCube(ep, Vector3.one * 0.5f);
        if (WebCurrentLength > float.Epsilon)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(wp, wp + Vector3.down * WebCurrentLength);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube((Vector2)transform.position + Trigger.offset, Trigger.size);
        }
    }
#endif
}
public enum SpiderState
{
    Rest,
    Weave,
    Hunt,
    Pull,
    Digest
}
