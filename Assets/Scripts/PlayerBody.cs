using UnityEngine;

public class PlayerBody
{
    public PlayerBody(Player p)
    {
        player = p;
    }

    public Vector3 Up => player.transform.up;
    public Vector3 Down => -Up;
    public Vector3 Front => player.transform.right;
    public Vector3 Back => -Front;
    public Vector2 Hull => new Vector2(1f, 0.95f);
    public Vector2 HullOffset => Vector2.zero;

    private Player player;
}
