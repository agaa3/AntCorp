using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsManager : MonoBehaviour
{
    public static FriendsManager instance;
    public Text text;
    int friendsNumber = 5; //na razie, do testow 
    
    public GameObject messageBox;
    public Text messageText;

    private IEnumerator popCoroutine;
    public bool collides = false;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        text.text = "X" + friendsNumber.ToString();
        messageBox.SetActive(false);    
    }

    public void ChangeFriendsScore()
    {
        friendsNumber += 1;
        text.text = "X"+friendsNumber.ToString();
    }

    public void PopMessage(string text)
    {
        if (popCoroutine != null)
        {
            StopCoroutine(popCoroutine);
        }
        popCoroutine = PopCoroutine(text);
        StartCoroutine(popCoroutine);
        messageBox.SetActive(true);
        collides = true;
    }

    public void HidePopMessage()
    {
        messageBox.SetActive(false);
    }

    public IEnumerator PopCoroutine(string text)
    {
        messageText.text = text;
        yield return new WaitForSeconds(3);
        collides = false;
        HidePopMessage();
    }


}
