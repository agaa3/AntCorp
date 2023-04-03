using UnityEngine;

// TODO: Figure out a shorter name :-)
[CreateAssetMenu(fileName = "New Environment Material", menuName = "AntCorp/Environment Material")]
public class EnvironmentMaterialDefinition : ScriptableObject
{
    public HeadAxis StickAxis => _stickAxis;
    public float FloorSpeed => _floorSpeed;
    public float WallSpeed => _wallSpeed;
    public float CeilingSpeed => _ceilingSpeed;

    [SerializeField] float _floorSpeed;
    [SerializeField] float _wallSpeed;
    [SerializeField] float _ceilingSpeed;
    [SerializeField] HeadAxis _stickAxis;
}
