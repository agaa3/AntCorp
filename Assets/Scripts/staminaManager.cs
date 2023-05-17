using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public ItemManager ItemManager;
    public PlayerController PlayerController;
    public int NumberOfCandies;

    public Sprite[] StrengthSprites = new Sprite[6];

    Image actualStrengthSprite;

    void Start()
    {
        actualStrengthSprite = GetComponent<Image>();
    }

    public void setStrenghtUI()
    {
        if(NumberOfCandies < 5)
            actualStrengthSprite.sprite = StrengthSprites[NumberOfCandies];
        else
            actualStrengthSprite.sprite = StrengthSprites[5];
    }

    public void setAntSpeed()
    {
        if (NumberOfCandies > 2)
            PlayerController.SetAntSpeedMultiplier(1.0f);
        else
            PlayerController.SetAntSpeedMultiplier(0.5f);
    }

    void Update()
    {
        NumberOfCandies = ItemManager.getNumberOfCandies();
        setStrenghtUI();
        setAntSpeed();
    }
}
