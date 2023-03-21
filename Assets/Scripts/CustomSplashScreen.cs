using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class CustomSplashScreen : MonoBehaviour
{
    [Tooltip("Video to play")]
    public VideoClip Video;
    [Header("Settings")]
    public bool PlayOnAwake;
    public LoadSceneMode LoadMode;
    public string SceneToLoad;

    public bool IsPlaying { get; private set; }

    private VideoPlayer _player;


    public void Play()
    {
        StartCoroutine(PlaySequence());
    }
    private IEnumerator PlaySequence()
    {
        _player.clip = Video;
        _player.frame = 0;
        _player.Prepare();
        _player.Play();
        // wait for the first frame
        while (!_player.isPlaying)
        {
            yield return null;
        }
        while (_player.isPlaying)
        {
            // TODO: Add support for additive load mode
            yield return null;
        }
        if (LoadMode == LoadSceneMode.Single)
        {
            SceneManager.LoadScene(SceneToLoad, LoadSceneMode.Single);
        }
    }
    #region Unity Callbacks
    private void Awake()
    {
        _player = GetComponent<VideoPlayer>();
        if (PlayOnAwake)
        {
            Play();
        }
    }
    #endregion
}
