using UnityEngine;

/// <summary>
/// Generic button hint with animation
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
[DisallowMultipleComponent]
public class ButtonHint : MonoBehaviour
{
    [Header("Assets")]
    /// <summary>
    /// These sprites will be played during an animation
    /// </summary>
    public Sprite[] Sprites;
    [Header("Timing")]
    /// <summary>
    /// Gets after how much seconds the animation will start playing 
    /// </summary>
    public float BeginAfter = 0.0f;
    /// <summary>
    /// Gets for how long each frame is shown
    /// </summary>
    public float FrameDuration = 0.1f;
    /// <summary>
    /// Gets if animation will start playing on awake.
    /// </summary>
    [Header("Flags")]
    public bool PlayOnAwake;
    /// <summary>
    /// Index of the currently shown frame
    /// </summary>
    public int CurrentFrame { get; private set; } = -1;
    /// <summary>
    /// Gets if button hint is playing animation
    /// </summary>
    public bool IsPlaying { get; private set; }

    private SpriteRenderer _renderer;


    /// <summary>
    /// Begins animation playback 
    /// </summary>
    public void BeginPlay()
    {
        if (IsPlaying) 
        {
            CancelInvoke(nameof(NextFrame));
            CurrentFrame = -1;
            NextFrame();
        }
        IsPlaying = true;
        InvokeRepeating(nameof(NextFrame), BeginAfter, FrameDuration);
    }
    /// <summary>
    /// Ends animation playback
    /// </summary>
    public void EndPlay()
    {
        IsPlaying = false;
        CurrentFrame = -1;
        CancelInvoke(nameof(NextFrame));
    }
    private void NextFrame()
    {
        CurrentFrame++;
        if (CurrentFrame >= Sprites.Length)
        {
            CurrentFrame = 0;
        }
        _renderer.sprite = Sprites[CurrentFrame];
    }

    #region Unity Callbacks
    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        CurrentFrame = -1;
        if (PlayOnAwake)
        {
            BeginPlay();
        }
    }
    private void OnEnable()
    {
        CurrentFrame = -1;
        if (PlayOnAwake)
        {
            BeginPlay();
        }
    }
    private void OnDisable()
    {
        EndPlay();
    }
    #endregion
}