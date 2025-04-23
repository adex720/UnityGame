using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public float TILE_SIZE;
    public static readonly float MARGIN = 0.24f;

    private BoxCollider2D bc;
    private Transform tr;

    private GameHandler game;

    private bool generated;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        tr = GetComponent<Transform>();

        game = GameObject.Find("background").GetComponent<GameHandler>();

        TILE_SIZE = game.TILE_SIZE;

        generated = false;

        FixHitbox();
    }

    void FixHitbox()
    {
        bc.size = new Vector2(TILE_SIZE + (2 * MARGIN) / tr.localScale[0], TILE_SIZE + 2 * MARGIN / tr.localScale[1]);
    }

    void AddWallsToGame()
    {
        int width = (int)(tr.localScale[0] + 0.01f);
        int height = (int)(tr.localScale[1] + 0.01f);

        float left = tr.position[0] - (TILE_SIZE / 2) * (width - 1);
        float bottom = tr.position[1] - (TILE_SIZE / 2) * (height - 1);

        int[] bottomLeftTileCoordinates = game.GetTileCoordinates(left, bottom);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                game.AddWallPos(bottomLeftTileCoordinates[0] + x, bottomLeftTileCoordinates[1] + y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!generated)
        {
            AddWallsToGame();
            generated = true;
        }
    }
}
