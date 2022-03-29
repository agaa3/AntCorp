using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    int candyNumber = 5; //na razie, do testow 
    int stickNumber = 3;
    public RawImage numberOfCandiesImage;
    public RawImage numberOfSticksImage;
    public Texture[] allNumbers = new Texture[10];


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        numberOfCandiesImage.texture = allNumbers[candyNumber];
        numberOfSticksImage.texture = allNumbers[stickNumber];
    }

    public void ChangeCandyNumber()
    {
        candyNumber += 1;
        numberOfCandiesImage.texture = allNumbers[candyNumber];
        
    }

    public void ChangeStickNumber()
    {
        stickNumber += 1;
        numberOfSticksImage.texture = allNumbers[stickNumber];
    }

}
