using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsManager : MonoBehaviour
{
    public static FriendsManager instance;
    public Text text;
    int friendsScore;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        
    }

    public void ChangeFriendsScore()
    {
        friendsScore += 1;
        text.text = "X"+friendsScore.ToString();
    }
}
