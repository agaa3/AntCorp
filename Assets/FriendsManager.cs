using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsManager : MonoBehaviour
{
    public RawImage friendsCount;
    public static FriendsManager instance;
    //public Text text;
    int friendsScore = 0;
    public Texture[] friendsNumbers = new Texture[10];

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        friendsCount.texture = friendsNumbers[friendsScore];
    }

    public void ChangeFriendsScore()
    {
        if(friendsScore < friendsNumbers.Length - 1)
        {
            friendsScore++;
            //text.text = "friends: "+friendsScore.ToString();
            friendsCount.texture = friendsNumbers[friendsScore];
        }
        else
        {
            friendsCount.texture = friendsNumbers[friendsNumbers.Length - 1];
        }
        
    }
}
