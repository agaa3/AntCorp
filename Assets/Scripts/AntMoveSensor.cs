using System;
using UnityEngine;

// TODO: Figure out a better name
[Serializable]
public class AntMoveSensor
{
    [Header("Sensors")]
    public TriggerSensor Ahead;
    public TriggerSensor AheadAbove;
    public TriggerSensor AheadBelow;
    public TriggerSensor Behind;
    public TriggerSensor BehindAbove;
    public TriggerSensor BehindBelow;
    public TriggerSensor Above;
    public TriggerSensor Below;
    public TriggerSensor Grounded;

    public bool CanTurnInside()
    {
        return Ahead.IsTriggering && Below.IsTriggering;
    }
    public bool CanTurnOutside()
    {
        return !Ahead.IsTriggering && (!AheadBelow.IsTriggering || (!Below.IsTriggering && BehindBelow.IsTriggering)) && Below.IsTriggering;
    }
    public bool InFreefall()
    {
        return !(Ahead.IsTriggering && AheadAbove.IsTriggering && AheadBelow.IsTriggering && Behind.IsTriggering && BehindAbove.IsTriggering && BehindBelow.IsTriggering && Above.IsTriggering && Below.IsTriggering);
    }
    public bool HasGroundBelow()
    {
        return Below.IsTriggering;
    }
    public bool HasGroundAbove()
    {
        return Above.IsTriggering || AheadAbove.IsTriggering || BehindAbove.IsTriggering;
    }
    public bool IsGrounded()
    {
        return Grounded.IsTriggering;
    }
}
