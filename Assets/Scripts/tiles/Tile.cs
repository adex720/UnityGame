using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{

    protected Animator anim;
    protected BoxCollider bc;
    protected Transform tr;

    protected GameHandler game;
    protected TileInventory tileInventory;

    public Vector2 startPos;
    public Vector2 lastValidPos;
    public int[] lastValidCoordinates;

    public static bool moving = false;
    private static int movingId;
    private bool fromInvetory;

    public int id;
    private static int nextId = 0;

    protected Func<Tile, bool> shouldAddToInventory;
    public static readonly Func<Tile, bool> ALWAYS_ADD_TO_INVENTORY = t => true;
    public static readonly Func<Tile, bool> NEVER_ADD_TO_INVENTORY = t => false;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        bc = GetComponent<BoxCollider>();
        tr = GetComponent<Transform>();

        game = GameObject.Find("background").GetComponent<GameHandler>();
        tileInventory = GameObject.Find("tile_inventory").GetComponent<TileInventory>();

        startPos = tr.position;
        lastValidPos = startPos;
        lastValidCoordinates = new int[] { -1, -1 };


        moving = false;
        movingId = -1;

        id = nextId;
        nextId++;

        shouldAddToInventory = ALWAYS_ADD_TO_INVENTORY;
        OnStart();

        if (shouldAddToInventory(this))
        {
            tileInventory.AddTile(this);
        }
    }

    public abstract void OnStart();

    // Update is called once per frame
    void Update()
    {
        UpdateSprite();

        CheckMouse();
    }

    public abstract void UpdateSprite();

    public abstract void OnCollision(Player player);

    public void CheckMouse()
    {
        if (!game.CanMoveTiles())
        {
            return;
        }

        if (!moving && !Input.GetMouseButton(0))
        {
            return;
        }

        if (moving && movingId != id)
        {
            return;
        }

        if (!Input.GetMouseButton(0))
        {
            moving = false;
            movingId = -1;

            if (fromInvetory && lastValidCoordinates[0] >= 0)
            {
                tileInventory.RemoveTile(this);
            }

            return;
        }

        Vector2 mousePos = GameHandler.GetMousePos();
        float mouseX = mousePos[0];
        float mouseY = mousePos[1];

        if (!moving)
        {

            float ownX = tr.position[0];
            float ownY = tr.position[1];

            float halfTileWidth = game.TILE_SIZE / 2;

            // Check if cursor is on this tile
            if (Math.Abs(ownX - mouseX) > halfTileWidth || Math.Abs(ownY - mouseY) > halfTileWidth) return;

            fromInvetory = lastValidCoordinates[0] == -1;
            moving = true;
            movingId = id;
        }

        FixPosition(mouseX, mouseY);
    }

    public void FixPosition(float x, float y)
    {
        int halfTileWidthInt = (int)(game.TILE_SIZE * 50 + 0.001f);
        float halfTileWidth = game.TILE_SIZE / 2;

        int[] coordinates = game.GetTileCoordinates(x, y);
        int tileXIndex = coordinates[0];
        int tileYIndex = coordinates[1];

        float tileXPos = game.areaX + halfTileWidth + tileXIndex * game.TILE_SIZE;
        float tileYPos = game.areaY + halfTileWidth + tileYIndex * game.TILE_SIZE;

        // Debug.Log("pos: " + intX + ", " + intY + ", alku: " + intLeft + ", " + intBottom + ", ero: " + diffX + ", " + diffY + ", tile: " + tileXIndex + ", " + tileYIndex + ", koordinaatit: " + tileXPos + ", " + tileYPos);

        if (lastValidCoordinates[0] == tileXIndex && lastValidCoordinates[1] == tileYIndex) return; // Sama kuin viimeksi

        if (tileXIndex < 0 || tileXIndex < 0 || tileXIndex >= game.boardWidth || tileYIndex >= game.boardHeight) return; // Alueen ulkopuolella
        if (game.CanNotMoveTileTo(tileXIndex, tileYIndex)) return; // Lähimmässä on jo jokin muu

        game.AddPosition(tileXIndex, tileYIndex);
        game.RemovePostion(lastValidCoordinates[0], lastValidCoordinates[1]);


        lastValidCoordinates[0] = tileXIndex;
        lastValidCoordinates[1] = tileYIndex;

        lastValidPos = new Vector2(tileXPos, tileYPos);
        tr.position = lastValidPos;

    }

    public void SetStartPos(float x, float y)
    {
        startPos = new Vector2(x, y);
        lastValidPos = startPos;
        tr.position = startPos;

        int[] coordinates = game.GetTileCoordinates(x, y);
    }

    public int GetCompareValue()
    {
        return (GetTypeId() << 8) | GetValueId();
    }

    public abstract int GetTypeId();

    public abstract int GetValueId();

}