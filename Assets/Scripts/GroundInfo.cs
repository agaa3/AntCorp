using UnityEngine;

public struct GroundInfo
{
    public GroundInfo(EnvironmentMaterial material, Vector3 normal)
    {
        Material = material;
        Normal = normal;
        Angle = Vector3.Angle(normal, Vector3.up);
        SignedAngle = Vector3.SignedAngle(normal, Vector3.up, Vector3.back);
    }
    public float Angle;
    public float SignedAngle;
    public Vector3 Normal;
    public EnvironmentMaterial Material;
}
