using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerGatherer : PlayerModule
{
    public Rigidbody2D UseRigidbody => Player.UseRigidbody;
    public BoxCollider2D Collider => Player.Collider;
    public GameObject Friend;


    public override void OnUpdate(TimeState time)
    {
        // ???
        if (Friend != null && Friend.GetComponent<SmallAnt>().comboDone == true)
        {
            Destroy(Friend);
            FriendsManager.instance.messageBox.SetActive(false);
            ItemManager.Main.SmallAnts++;
            Friend = null;
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
        // TODO: redo all this shit
        switch (other.tag)
        {
            case "Friends":
                string message = "Hit " + other.gameObject.GetComponent<SmallAnt>().combo + " key \nto get mud off small ant. ";
                FriendsManager.instance.PopMessage(message);
                Friend = other.gameObject;
                break;
            case "Candy":
                Destroy(other.gameObject);
                ItemManager.Main.Candies++;
                break;
            case "Stick":
                Destroy(other.gameObject);
                ItemManager.Main.Sticks++;
                break;
        }
    }
}