using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;
    public int candyNumber = 0; //na razie, do testow 
    public int stickNumber = 0;
    public RawImage numberOfCandiesImage;
    public RawImage numberOfSticksImage;
    public Texture[] allNumbers = new Texture[10];

    private Bridge bridgeToPlace;
    public GameObject grid;
    public CustomCursor customCursor;
    public Tile[] tiles;

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

    void Update()
    {
        if(Input.GetMouseButtonDown(0) && bridgeToPlace != null)
        {
            Tile nearestTile = null;
            float shortestDistance = float.MaxValue;
            foreach(Tile tile in tiles)
            {
                float distance = Vector2.Distance(tile.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
                if(distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestTile = tile;
                }
            }
            if(nearestTile.isOccupied == false)
            {
                Instantiate(bridgeToPlace, nearestTile.transform.position, Quaternion.identity);
                bridgeToPlace = null;
                nearestTile.isOccupied = true;
                grid.SetActive(false);
                customCursor.gameObject.SetActive(false);
                Cursor.visible = true;
            }
        }
    }

    public void IncreaseCandyNumber()
    {
        candyNumber += 1;
        numberOfCandiesImage.texture = allNumbers[candyNumber];
        
    }

    public void IncreaseStickNumber()
    {
        stickNumber += 1;
        numberOfSticksImage.texture = allNumbers[stickNumber];
    }

    public void BuildBridge(Bridge bridge)
    {
        if(stickNumber >= bridge.cost)
        {
            customCursor.gameObject.SetActive(true);
            Cursor.visible = false;
            stickNumber -= bridge.cost;
            numberOfSticksImage.texture = allNumbers[stickNumber];
            bridgeToPlace = bridge;
            grid.SetActive(true);
        }
    }

}
