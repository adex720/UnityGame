using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TileInventory : MonoBehaviour
{

    private Transform tr;

    private GameHandler game;

    public static readonly int WIDTH = 3;
    public static readonly int MIN_HEIGHT = 1;

    public static readonly float MARGIN_SIZE = 0.08f;
    public static readonly float MARGIN_RATIO = MARGIN_SIZE / 0.64f;

    private int currentHeight = MIN_HEIGHT;
    private List<Tile> tiles = new List<Tile>(); // vaihda listaksi
    private int tileCount = 0;

    private bool init = false;

    float xLeft;
    float xPos;
    float topRowY;
    float yPos;


    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();

        game = GameObject.Find("background").GetComponent<GameHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init) Init();

        tr.position = new Vector2(xPos, yPos);
        tr.localScale = new Vector2(WIDTH + (WIDTH + 1) * MARGIN_RATIO, currentHeight + (currentHeight + 1) * MARGIN_RATIO);
    }

    void Init()
    {
        xLeft = game.areaX + game.areaWidth + game.TILE_SIZE - MARGIN_SIZE;
        xPos = xLeft + (WIDTH * game.TILE_SIZE + (WIDTH + 1) * MARGIN_SIZE) / 2;
        topRowY = game.areaY + game.areaHeight - game.TILE_SIZE + MARGIN_SIZE;
        yPos = getYPos();

        UpdateTileOrder();
        UpdateTilePositions();

        init = true;
    }

    float getYPos()
    {
        return topRowY - (currentHeight) * (game.TILE_SIZE + MARGIN_SIZE) / 2 - MARGIN_SIZE / 2;
    }

    public void AddTile(Tile tile)
    {

        tiles.Add(tile);
        tileCount++;

        UpdateTileOrder();

        currentHeight = Math.Max((tileCount + WIDTH - 1) / WIDTH, MIN_HEIGHT);
        if (init)
        {
            yPos = getYPos();
            UpdateTilePositions();
        }
    }

    public void RemoveTile(Tile tile)
    {
        if (tiles.Remove(tile))
        {
            tileCount--;
            UpdateTileOrder();
            currentHeight = Math.Max((tileCount + WIDTH - 1) / WIDTH, MIN_HEIGHT);
            yPos = getYPos();
            UpdateTilePositions();
        }
    }

    public void UpdateTileOrder()
    {
        tiles.Sort((a, b) => a.GetCompareValue() - b.GetCompareValue());
    }

    public void UpdateTilePositions()
    {

        float[] xValues = new float[WIDTH];
        for (int i = 0; i < WIDTH; i++)
        {
            xValues[i] = xLeft + MARGIN_SIZE + (game.TILE_SIZE / 2) + i * (game.TILE_SIZE + MARGIN_SIZE);
        }

        float y = topRowY - (game.TILE_SIZE / 2) - MARGIN_SIZE;
        int xIndex = 0;

        for (int i = 0; i < tileCount; i++)
        {
            Tile tile = tiles[i];
            tile.SetStartPos(xValues[xIndex], y);

            xIndex++;
            if (xIndex >= WIDTH)
            {
                xIndex = 0;
                y -= game.TILE_SIZE + MARGIN_SIZE;
            }
        }

    }
}
