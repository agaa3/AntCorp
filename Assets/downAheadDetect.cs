using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downAheadDetect : MonoBehaviour
{
    public bool flag = false;


    public void FixedUpdate()
    {
        isTouching();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("climbing_walls"))
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
            Debug.Log("DOWN AHEAD : TRUE");
            return true;
        }
        else
        {
            Debug.Log("DOWN AHEAD : FALSE");
            return false;
        }
    }
}
