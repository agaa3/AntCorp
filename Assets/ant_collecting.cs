using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_collecting : MonoBehaviour
{
    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;

    private void Start()
    {
    }


    private void Update()
    {
     }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Friends"))
        {
            string message = "uderzaj klawisz " + other.gameObject.GetComponent<SmallAnt>().combo;
            FriendsManager.instance.PopMessage(message);

            if (other.gameObject.GetComponent<SmallAnt>().comboDone == true)
            {
                Destroy(other.gameObject);
                FriendsManager.instance.messageBox.SetActive(false);
                FriendsManager.instance.ChangeFriendsScore();
            }

        }

        if (other.gameObject.CompareTag("Candy"))
        {
            Destroy(other.gameObject);
            ItemManager.instance.ChangeCandyNumber();
        }
        
        if (other.gameObject.CompareTag("Stick"))
        {
            Destroy(other.gameObject);
            ItemManager.instance.ChangeStickNumber();
        }
    }

     
}