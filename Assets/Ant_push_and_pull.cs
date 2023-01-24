using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant_push_and_pull : MonoBehaviour
{
    public Transform grabDetect;
    public Transform stoneHolder;
    public float rayDist;
    private bool Flipped = false;

    private void NoFlip()
    {
        if (!Flipped && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)))
        {
            transform.Rotate(0f,180f,0);
            Flipped = true;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
       RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist);

       if(grabCheck.collider != null && grabCheck.collider.CompareTag("Stone"))
       {
            if(Input.GetKey(KeyCode.F))
            {
                grabCheck.collider.gameObject.transform.parent = stoneHolder;
                grabCheck.collider.gameObject.transform.position = stoneHolder.position;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                NoFlip();
            }
            else
            {
                grabCheck.collider.gameObject.transform.parent = null;
                grabCheck.collider.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                transform.Rotate(0f,180f,0);
                Flipped = false;
            }
       }
    }
}
