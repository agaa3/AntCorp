using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenFader : MonoBehaviour
{
    public static void FadeIn(float time)
    {
        main.StartCoroutine(main.FadeInSequence(time));
    }
    public static void FadeOut(float time)
    {
        main.StartCoroutine(main.FadeOutSequence(time));
    }

    private IEnumerator FadeInSequence(float time)
    {
        float t = 0.0f;
        while (t < time)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, 1);
            Color c = Color.black;
            c.a = t / time;
            image.color = c;
            yield return null;
        }
        image.color = Color.black;
    }
    private IEnumerator FadeOutSequence(float time)
    {
        float t = 0.0f;
        while (t < time)
        {
            t = Mathf.Clamp(t + Time.deltaTime, 0, 1);
            Color c = Color.black;
            c.a = 1f - (t / time) * 2;
            image.color = c;
            yield return null;
        }
    }

    private void Awake()
    {
        main = this;
        image = GetComponent<Image>();
    }

    private Image image;
    private static ScreenFader main;
}
