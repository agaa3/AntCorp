using UnityEngine;

public readonly struct TimeState
{
    public readonly float DeltaTime;
    public readonly float UnscaledDeltaTime;
    public readonly float FixedDeltaTime;
    public readonly float FixedUnscaledDeltaTime;

    public static TimeState Create()
    {
        return new TimeState(Time.deltaTime, Time.fixedDeltaTime, Time.unscaledDeltaTime, Time.fixedUnscaledDeltaTime);
    }
    internal TimeState(float dt, float fxDt, float unDt, float fxdUnDt)
    {
        DeltaTime = dt;
        FixedDeltaTime = fxdUnDt;
        UnscaledDeltaTime = unDt;
        FixedUnscaledDeltaTime = fxdUnDt;
    }
}
