using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsManager : MonoBehaviour
{
    public static FriendsManager instance;
    int friendsNumber = 0; //na razie, do testow 
    
    public GameObject messageBox;
    public Text messageText;

    public RawImage numberOfFriendsImage;
    public Texture[] allNumbers = new Texture[10];

    private IEnumerator popCoroutine;
    private IEnumerator stopCoroutine;

    public bool collides = false;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        messageBox.SetActive(false);    
        numberOfFriendsImage.texture = allNumbers[friendsNumber];
    }

    public void IncreaseFriendsScore()
    {
        friendsNumber += 1;
        numberOfFriendsImage.texture = allNumbers[friendsNumber];
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
        HidePopMessage();
    }

    public void StopCollision()
    {
        if (stopCoroutine != null)
        {
            StopCoroutine(stopCoroutine);
        }
        stopCoroutine = StopCollisionCoroutine();
        StartCoroutine(stopCoroutine);
    }

    public IEnumerator StopCollisionCoroutine()
    {
        yield return new WaitForSeconds(3);
        collides = false;
    }


}
