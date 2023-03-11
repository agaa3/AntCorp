using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downAheadDetect : MonoBehaviour
{
    [SerializeField] public bool flag = false;


    public void FixedUpdate()
    {
        isTouching();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("climbing_walls"))
            flag = true;
        else
            flag = false;
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
