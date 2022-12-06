using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aheadCheck : MonoBehaviour
{
    public bool flag = true;

    public GameObject aheadCheck1;
    public GameObject aheadCheck2;
    public GameObject aheadCheck3;

    simpleCheckPoint aheadCheckPoint1;
    simpleCheckPoint aheadCheckPoint2;
    simpleCheckPoint aheadCheckPoint3;
    private void Start()
    {
        aheadCheckPoint1 = aheadCheck1.GetComponent<simpleCheckPoint>();
        aheadCheckPoint2 = aheadCheck2.GetComponent<simpleCheckPoint>();
        aheadCheckPoint3 = aheadCheck3.GetComponent<simpleCheckPoint>();
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
        if (aheadCheckPoint1.isTouching() || aheadCheckPoint2.isTouching() || aheadCheckPoint3.isTouching())
            flag = true;
        else
            flag = false;
    }
    
}
