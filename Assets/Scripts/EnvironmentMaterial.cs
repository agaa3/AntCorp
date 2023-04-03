using UnityEngine;

public class EnvironmentMaterial : MonoBehaviour
{
    public EnvironmentMaterialDefinition Definition;

    public float GetSpeedMultiplier(HeadAxis axis)
    {
        switch (axis)
        {
            case HeadAxis.Floor:
                return Definition.FloorSpeed;
            case HeadAxis.WallLeft:
                return Definition.WallSpeed;
            case HeadAxis.WallRight:
                return Definition.WallSpeed;
            case HeadAxis.Ceiling:
                return Definition.CeilingSpeed;
            default:
                return 1.0f;
        }
    }
    public bool CanStick(HeadAxis axis)
    {
        return (Definition.StickAxis & axis) != 0;
    }
}
