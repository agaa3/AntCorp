using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downLeftDetect : MonoBehaviour
{
    public bool flag = false;

    public void Update()
    {
        isTouching();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        flag = true;
        //return true;
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        flag = false;
    }

    public bool isTouching()
    {
        if (flag == true)
        {
            Debug.Log("DOWNLEWYPRAWDA");
            return true;
        }
        else
        {
            Debug.Log("DOWNLEWYFALSZ");
            return false;
        }
    }
}
