using UnityEngine;
using UnityEngine.UI;

public class staminaManager : MonoBehaviour
{
    public ItemManager itemManager;
    public PlayerController playerController;
    public int numberOfCandies;

    public Sprite[] strengthSprites = new Sprite[6];

    Image actualStrengthSprite;

    void Start()
    {
        actualStrengthSprite = GetComponent<Image>();
    }

    public void setStrenghtUI()
    {
        if(numberOfCandies < 5)
            actualStrengthSprite.sprite = strengthSprites[numberOfCandies];
        else
            actualStrengthSprite.sprite = strengthSprites[5];
    }

    public void setAntSpeed()
    {
        if (numberOfCandies > 2)
            playerController.setSpeed(2.5f);
        else
            playerController.setSpeed(1.0f);
    }

    void Update()
    {
        numberOfCandies = itemManager.getNumberOfCandies();
        setStrenghtUI();
        setAntSpeed();
    }
}
