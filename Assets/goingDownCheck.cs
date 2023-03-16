using UnityEngine;

public class goingDownCheck : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;

    public bool IsGoingDown()
    {
        return !((Point2.position.y - Point1.position.y) >= 0);
    }
    public bool IsGoingRight()
    {
        return (Point2.position.x - Point1.position.x) >= 0;
    }

}