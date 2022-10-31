using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downCheck : MonoBehaviour
{
    public bool flag = true;

    public GameObject aheadCheck1;
    public GameObject aheadCheck2;
    public GameObject aheadCheck3;

    simpleCheckPoint downCheckPoint1;
    simpleCheckPoint downCheckPoint2;
    simpleCheckPoint downCheckPoint3;
    private void Start()
    {
        downCheckPoint1 = aheadCheck1.GetComponent<simpleCheckPoint>();
        downCheckPoint2 = aheadCheck2.GetComponent<simpleCheckPoint>();
        downCheckPoint3 = aheadCheck3.GetComponent<simpleCheckPoint>();
    }
    void FixedUpdate()
    {
        isTouching();
    }

    public bool isTouching()
    {
        isTouchingUpdate();
        if (flag == true)
            return true;
        else
            return false;
    }

    public void isTouchingUpdate()
    {
        if (downCheckPoint1.isTouching() || downCheckPoint2.isTouching() || downCheckPoint3.isTouching())
            flag = true;
        else
            flag = false;
    }

}
