using System;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class uGUI_Floater : MonoBehaviour
{
    public Vector3 InitialScale = Vector3.one;
    [Header("Affect")]
    public bool Scale = false;
    [Header("Settings")]
    public FloaterSettings ScaleConfig;

    private RectTransform _transform;


    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }
    private void Update()
    {
        UpdateSize();
    }
    private void UpdateSize()
    {
        Vector3 scale = InitialScale;

        if (Scale)
        {
            int m = ScaleConfig.Mode;
            float a = ScaleConfig.Amplitude * 0.5f;
            float s = Mathf.Cos(Time.time * ScaleConfig.Frequency) * a;
            if (m != 0)
            {
                s += a * m;
            }
            scale.x += s;
            scale.y += s;
            scale.z += s;
        }

        _transform.localScale = scale;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        ScaleConfig.Mode = Math.Sign(ScaleConfig.Mode);
        ScaleConfig.Amplitude = Mathf.Abs(ScaleConfig.Amplitude);
        if (ScaleConfig.Mode == -1 && ScaleConfig.Amplitude >= 0.5f)
        {
            ScaleConfig.Amplitude = 0.499999f;
        }
        if (ScaleConfig.Mode == 0 && ScaleConfig.Amplitude > 1.0f)
        {
            ScaleConfig.Amplitude = 1.0f;
        }
    }
}
#endif

[Serializable]
public class FloaterSettings
{
    public int Mode = 0;
    public float Amplitude = 1;
    public float Frequency = 1;
}