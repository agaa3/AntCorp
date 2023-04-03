using AntCorp;
using UnityEngine;

public class CameraFocusZone : MonoBehaviour
{
    public Transform Point;
    public HeadAxis AllowedAxis;
    public bool AllowMidTurn;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            PlayerCamera cam = Player.Main.Camera;
            bool flag = (AllowedAxis & Player.Main.Controller.Axis) != 0;
            if (cam.ActiveFocusZone == this)
            {
                if (!flag)
                {
                    cam.OnFocusZoneExit(this);
                }
            }
            else if (flag)
            {
                cam.OnFocusZoneEnter(this);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Tag.Player))
        {
            PlayerCamera c = Player.Main.Camera;
            if (c.ActiveFocusZone == this)
            {
                c.OnFocusZoneExit(this);
            }
        }
    }
    private void OnDisable()
    {
        if (Player.Main.Camera.ActiveFocusZone == this)
        {
            Player.Main.Camera.OnFocusZoneExit(this);
        }
    }
}
