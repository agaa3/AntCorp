using System;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Main;
    public int Candies
    {
        get => _candies;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            CandiesChanged?.Invoke(_candies, value);
            _candies = value;
        }
    }
    public int Sticks
    {
        get => _sticks;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            SticksChanged?.Invoke(_sticks, value);
            _sticks = value;
        }
    }
    public int SmallAnts
    {
        get => _smallAnts;
        set
        {
            if (value < 0)
            {
                value = 0;
            }
            SmallAntsChanged?.Invoke(_smallAnts, value);
            _smallAnts = value;
        }
    }

    public int getNumberOfCandies()
    {
        return _candies;
    }

    public event Action<int, int> SticksChanged;
    public event Action<int, int> CandiesChanged;
    public event Action <int, int> SmallAntsChanged;

    private int _candies = 0;
    private int _sticks = 0;
    private int _smallAnts = 0;


    void Awake()
    {
        if (Main == null)
        {
            Main = this;
        }
        else if (Main != this)
        {
            Destroy(this);
        }
    }

}
