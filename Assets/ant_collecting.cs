using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ant_collecting : MonoBehaviour
{
    private Rigidbody2D PlayerAnt;
    private BoxCollider2D boxCollider2d;
    private GameObject friend;

    private void Start()
    {
    }

    private void Update()
    {
        if (friend != null && friend.GetComponent<SmallAnt>().comboDone == true)
        {
            Destroy(friend);
            FriendsManager.instance.messageBox.SetActive(false);
            FriendsManager.instance.IncreaseFriendsScore();
            friend = null;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Friends"))
        {
            FriendsManager.instance.StopCollision();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Friends"))
        {
            string message = "Hit " + other.gameObject.GetComponent<SmallAnt>().combo + " key. ";
            FriendsManager.instance.PopMessage(message);
            friend = other.gameObject;

        }

        if (other.gameObject.CompareTag("Candy"))
        {
            Destroy(other.gameObject);
            ItemManager.instance.IncreaseCandyNumber();
        }
        
        if (other.gameObject.CompareTag("Stick"))
        {
            Destroy(other.gameObject);
            ItemManager.instance.IncreaseStickNumber();
        }
    }

     
}