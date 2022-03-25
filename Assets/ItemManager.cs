using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public Text text;
    int itemNumber = 5; //na razie, do testow 
    public RawImage numberOfCandiesImage;
    public Texture[] allNumbers = new Texture[10];


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        text.text = "X" + itemNumber.ToString();
        numberOfCandiesImage.texture = allNumbers[itemNumber];
    }

    public void ChangeItemNumber()
    {
        itemNumber += 1;
        text.text = "X" + itemNumber.ToString();
        numberOfCandiesImage.texture = allNumbers[itemNumber];
    }

}
