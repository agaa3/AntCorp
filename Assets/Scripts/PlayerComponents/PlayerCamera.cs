using Cinemachine;

public class PlayerCamera : PlayerModule
{
    // state
    public bool InFocusZone { get; private set; }
    public CameraFocusZone? ActiveFocusZone { get; private set; }
    // components
    public CinemachineVirtualCamera CinemaCam { get; private set; }

    public void OnFocusZoneEnter(CameraFocusZone zone)
    {
        InFocusZone = true;
        ActiveFocusZone = zone;
        CinemaCam.Follow = zone.Point;

    }
    public void OnFocusZoneExit(CameraFocusZone zone)
    {
        CinemaCam.Follow = Player.transform;
        InFocusZone = false;
        ActiveFocusZone = null;
    }

    private void Awake()
    {
        CinemaCam = GetComponent<CinemachineVirtualCamera>();
    }
}
