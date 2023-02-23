using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallCheck : MonoBehaviour
{
    public GameObject check1;
    public GameObject check2;

    climbingOnRightWallCheck checkPoint1;
    climbingOnRightWallCheck checkPoint2;
    void Start()
    {
        checkPoint1 = check1.GetComponent<climbingOnRightWallCheck>();
        checkPoint2 = check2.GetComponent<climbingOnRightWallCheck>();
    }
    public bool isAntOnRightWall()
    {
        float val = checkPoint2.getPositionX() - checkPoint1.getPositionX();

        if (val >= 0)
            return false;
        else
            return true;
    }
}
