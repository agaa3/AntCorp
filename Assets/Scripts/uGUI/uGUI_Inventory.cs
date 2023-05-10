using UnityEngine;
using UnityEngine.UI;

public class uGUI_Inventory : MonoBehaviour
{
    [Header("Displays")]
    public Image SticksDisplay;
    public Image CandiesDisplay;
    public Image SmallAntsDisplay;
    [Header("Assets")]
    public Sprite[] Digits = new Sprite[10];

    private void Start()
    {
        ItemManager.Main.SticksChanged += UpdateSticks;
        ItemManager.Main.CandiesChanged += UpdateCandies;
        ItemManager.Main.SmallAntsChanged += UpdateSmallAnts;
    }
    private void OnDisable()
    {
        ItemManager.Main.SticksChanged -= UpdateSticks;
        ItemManager.Main.CandiesChanged -= UpdateCandies;
        ItemManager.Main.SmallAntsChanged -= UpdateSmallAnts;
    }
    private void UpdateSticks(int old, int nw)
    {
        if (nw > 9)
        {
            nw = 9;
        }
        SticksDisplay.sprite = Digits[nw];
    }
    private void UpdateCandies(int old, int nw)
    {
        if (nw > 9)
        {
            nw = 9;
        }
    }
    private void UpdateSmallAnts(int old, int nw)
    {
        if (nw > 9)
        {
            nw = 9;
        }
        SmallAntsDisplay.sprite = Digits[nw];
    }
}
