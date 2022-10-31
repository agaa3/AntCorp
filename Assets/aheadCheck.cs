using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aheadCheck : MonoBehaviour
{
    public bool flag = false;

    public GameObject aheadCheck1;
    public GameObject aheadCheck2;
    public GameObject aheadCheck3;

    simpleCheckPoint aheadCheckPoint1;
    simpleCheckPoint aheadCheckPoint2;
    simpleCheckPoint aheadCheckPoint3;
    void FixedUpdate()
    {
        isTouching();
    }

    public void isTouching()
    {
        if (aheadCheckPoint1.isTouching() || aheadCheckPoint2.isTouching() || aheadCheckPoint3.isTouching())
            flag = true;
        else
            flag = false;
    }
}
