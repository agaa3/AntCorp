using AntCorp;
using System;
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
        if (other.CompareTag(Tag.Pickable))
        {
            ItemPickable pickable = other.GetComponent<ItemPickable>();
            switch (pickable.Type)
            {
                case ItemType.Stick:
                    ItemManager.Main.Sticks++;
                    break;
                case ItemType.Candy:
                    ItemManager.Main.Candies++;
                    break;
                default:
                    throw new NotImplementedException();
            }
            Destroy(pickable.gameObject);
        }
    }
}