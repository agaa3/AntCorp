using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;

    public bool IsOnRightWall()
    {
        return !((Point2.position.x - Point1.position.x) >= 0);
    }
}
