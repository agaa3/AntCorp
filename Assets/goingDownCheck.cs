using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goingDownCheck : MonoBehaviour
{
    public GameObject check1;
    public GameObject check2;

    goingDownCheck1 checkPoint1;
    goingDownCheck2 checkPoint2;
    void Start()
    {
        checkPoint1 = check1.GetComponent<goingDownCheck1>();
        checkPoint2 = check2.GetComponent<goingDownCheck2>();
    }
    public bool isAntGoingDown()
    {
        float val = checkPoint2.getPositionY() - checkPoint1.getPositionY();

        if ( val >= 0)
            return false;
        else
            return true;
    }
}
