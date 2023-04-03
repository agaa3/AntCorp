using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void OnPlayerDie()
    {
        StartCoroutine(PlayerDeathSequence());
    }
    public void OnPlayerRevive()
    {
        StartCoroutine(PlayerRevivalSequence());
    }

    private IEnumerator PlayerDeathSequence()
    {
        ScreenFader.FadeIn(1.0f);
        yield return new WaitForSeconds(1.1f);
        yield return null;
        Player.Main.transform.position = new Vector2(-1.33f, -8.26f);
        Player.Main.Mixin.Revive();
    }
    private IEnumerator PlayerRevivalSequence()
    {
        yield return null;
        ScreenFader.FadeOut(2.0f);
    }

    private void Start()
    {
        Player.Main.Mixin.OnDie += OnPlayerDie;
        Player.Main.Mixin.OnRevive += OnPlayerRevive;
    }
}
