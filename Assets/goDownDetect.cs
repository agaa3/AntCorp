using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goDownDetect : MonoBehaviour
{
    public bool flag = false;

    public void Update()
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
            Debug.Log("GO DOWN: TRUE");
            return true;
        }
        else
        {
            Debug.Log("GO DOWN: FALSE");
            return false;
        }
    }

}
