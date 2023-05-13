using UnityEngine;

public class PlayerBody
{
    public PlayerBody(Player p)
    {
        player = p;
    }

    public Vector3 Up => player.transform.up;
    public Vector3 Down => -Up;
    public Vector3 Right => player.transform.right;
    public Vector3 Left => -Right;

    private Player player;
}
