using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{

    private Transform tr;

    private DataManager dataManager;


    public int playerCount = 0;
    public int playersfinished;
    private readonly HashSet<Player> players = new HashSet<Player>();

    public int coinCount = 0;
    public int coinsCollected;

    public bool[,] walls;

    public GameState gameState = GameState.MOVE_TILES;


    private readonly HashSet<int> kaytossa = new HashSet<int>();
    private static readonly int X_MULTIPLIER = 256;

    private readonly HashSet<Action<GameState>> animatorUpdators = new HashSet<Action<GameState>>();


    public readonly float TILE_SIZE = 0.64f;

    public int boardWidth = 16;
    public int boardHeight = 14;

    public bool generated = false;


    public bool showGetCoinsMessage;
    public bool showReachGoalMessage;


    public float areaX;
    public float areaY;
    public float areaWidth;
    public float areaHeight;
    void Start()
    {
        tr = GetComponent<Transform>();

        dataManager = new DataManager("../game_data.dt");

        playersfinished = 0;
        coinsCollected = 0;

        showGetCoinsMessage = false;
        showReachGoalMessage = false;

        areaWidth = TILE_SIZE * boardWidth;
        areaHeight = TILE_SIZE * boardHeight;

        walls = new bool[boardWidth, boardHeight];

        gameState = GameState.MOVE_TILES;

        CalculatePosition();
        UpdateAnimatorVariables();
        AddUnregisteredPlayerCoords();
        generated = true;
    }

    void Update()
    {
        CalculatePosition();
        ChechExit();
    }

    public int[] GetTileCoordinates(float x, float y)
    {
        int halfTileWidthInt = (int)(TILE_SIZE * 50 + 0.001f);
        float halfTileWidth = TILE_SIZE / 2;

        int intX = (int)(x * 100 - (TILE_SIZE * 50 - 0.001f));
        int intY = (int)(y * 100 - (TILE_SIZE * 50 - 0.001f));

        int intLeft = (int)(areaX * 100 + 0.001f);
        int intBottom = (int)(areaY * 100 + 0.001f);

        int diffX = intX - intLeft;
        int diffY = intY - intBottom;

        int tileXIndex = (int)((diffX + halfTileWidthInt) / (TILE_SIZE * 100));
        int tileYIndex = (int)((diffY + halfTileWidthInt) / (TILE_SIZE * 100));

        return new int[] { tileXIndex, tileYIndex };
    }

    public void AddPlayer(Player player)
    {
        playerCount++;
        players.Add(player);
        if (generated) AddPlayerCoords(player);
    }

    void AddUnregisteredPlayerCoords()
    {
        foreach (Player p in players)
        {
            AddPlayerCoords(p);
        }
    }

    void AddPlayerCoords(Player player)
    {
        int[] coords = player.GetTileCoordinates();
        AddPosition(coords[0], coords[1]);
    }

    public void GoalReached(Player player)
    {
        playersfinished++;

        if (playersfinished < playerCount) return;

        if (coinsCollected < coinCount)
        {
            NotEnoughCoins();
            return;
        }

        Completed();
    }

    public void AddCoin(Coin coin)
    {
        coinCount++;

        int[] coords = coin.GetTileCoordinates();
        AddPosition(coords[0], coords[1]);
    }

    public void NotEnoughCoins()
    {
        gameState = GameState.FAILED;
        showGetCoinsMessage = true;
        UpdateAnimatorVariables();
    }

    public void PlayerStuck()
    {
        gameState = GameState.FAILED;
        showReachGoalMessage = true;
        UpdateAnimatorVariables();
    }

    public void Completed()
    {
        gameState = GameState.FINISHED;
        UpdateAnimatorVariables();

        dataManager.IncreaseInt(DataObject.LEVELS_COMPLETED, 1);
        LoadNextLevel();
    }

    void CalculatePosition()
    {
        Vector2 position = tr.position;
        areaX = (float)Math.Round(position[0] - (areaWidth / 2), 2);
        areaY = (float)Math.Round(position[1] - (areaHeight / 2), 2);
    }

    public void AddPosition(int x, int y)
    {
        kaytossa.Add(x * X_MULTIPLIER + y);
    }

    public void AddPosition(int[] pos)
    {
        AddPosition(pos[0], pos[1]);
    }

    public void RemovePostion(int x, int y)
    {
        kaytossa.Remove(x * X_MULTIPLIER + y);
    }

    public bool CanNotMoveTileTo(int x, int y)
    {
        return kaytossa.Contains(x * X_MULTIPLIER + y) || IsWall(x, y);
    }

    public void AddWallPos(int x, int y)
    {
        walls[x, y] = true;
    }

    public void RemoveWallPos(int x, int y)
    {
        walls[x, y] = false;
    }

    public bool IsWall(int x, int y)
    {
        return walls[x, y];
    }

    public bool IsWall(int[] pos)
    {
        return walls[pos[0], pos[1]];
    }

    public void DebugWall()
    {
        string s = "";
        for (int y = boardHeight - 1; y >= 0; y--)
        {
            for (int x = 0; x < boardWidth; x++)
            {
                s += walls[x, y] ? "X" : "O";
            }
            s += "\n";
        }

        Debug.Log(s);
    }

    public void LoadNextLevel()
    {
        int id = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(id);
    }

    public bool CanMoveTiles()
    {
        return gameState == GameState.MOVE_TILES;
    }

    public bool IsSimulating()
    {
        return gameState == GameState.SIMULATE;
    }

    public bool CanStartSimulation()
    {
        return gameState == GameState.MOVE_TILES;
    }

    public void StartSimulation()
    {
        gameState = GameState.SIMULATE;

        foreach (Player player in players)
        {
            player.OnDirectionChange();
        }
        UpdateAnimatorVariables();
    }

    public bool CanPause()
    {
        return gameState == GameState.SIMULATE;
    }

    public void Pause()
    {
        gameState = GameState.PAUSED;
        foreach (Player player in players)
        {
            player.OnDirectionChange();
        }
        UpdateAnimatorVariables();
    }

    public bool CanResume()
    {
        return gameState == GameState.PAUSED;
    }

    public void Resume()
    {
        gameState = GameState.SIMULATE;
        foreach (Player player in players)
        {
            player.OnDirectionChange();
        }
        UpdateAnimatorVariables();
    }

    public bool CanReset()
    {
        return gameState == GameState.MOVE_TILES || gameState == GameState.PAUSED || gameState == GameState.FAILED;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static Vector2 GetMousePos()
    {
        Vector2 mouseScreenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }

    public void AddAnimatorUpdator(Action<GameState> updator)
    {
        animatorUpdators.Add(updator);
    }

    void UpdateAnimatorVariables()
    {
        foreach (Action<GameState> updator in animatorUpdators)
        {
            updator(gameState);
        }
    }

    void ChechExit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

}
