using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    public RawImage stickCount;
    private int sticks = 0; 
    public Texture[] stickNumbers = new Texture[10];

    void Start()
    {
        stickCount.texture = stickNumbers[sticks];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("stick"))
        {
            Destroy(collision.gameObject);
            if(sticks < stickNumbers.Length - 1)
            {
                sticks++;
                stickCount.texture = stickNumbers[sticks];
            }
            else
            {           
                stickCount.texture = stickNumbers[stickNumbers.Length - 1]; 
            }
        }
    }
}
