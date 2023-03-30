using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentMaterial : MonoBehaviour
{
    public float[] SpeedMultiplier = new float[3] {1, 1, 1};
    public HeadAxis StickAxis = HeadAxis.Floor;

    public float AxisToSpeed(HeadAxis axis){
        switch(axis){
            case HeadAxis.Floor:
                return SpeedMultiplier[0];
            case HeadAxis.Ceiling:
                return SpeedMultiplier[2];
            default:
                return SpeedMultiplier[1];
        }
    }
}
