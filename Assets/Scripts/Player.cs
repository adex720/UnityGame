using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 10000.0f;
    public Direction direction;
    public int startdirectionId = 0;

    bool stopped;

    private readonly Vector2[] hitboxOffsets = new Vector2[] {
            new Vector2(0f, -0.16f),
            new Vector2(-0.16f, 0f),
            new Vector2(0f, 0.16f),
            new Vector2(0.16f, 0f) };


    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private SpriteRenderer sr;
    private Transform tr;
    private Animator anim;

    private GameHandler game;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();

        game = GameObject.Find("background").GetComponent<GameHandler>();
        game.AddPlayer(this);

        direction = Direction.GetById(startdirectionId);

        stopped = false;

        OnDirectionChange();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ShouldMove()) return;

        CheckCollision();
    }

    public void OnDirectionChange()
    {
        UpdateSprite();
        UpdateVelocity();
        FixHitbox();
    }

    public void Stop()
    {
        UpdateVelocity();
    }

    void CheckCollision()
    {
        Vector2 op = tr.position;

        float halfTileWidth = game.TILE_SIZE / 2;
        float safe = 0.01f;

        float minX = game.areaX + halfTileWidth - safe;
        float maxX = game.areaX + game.areaWidth - halfTileWidth + safe;
        float minY = game.areaY + halfTileWidth - safe;
        float maxY = game.areaY + game.areaHeight - halfTileWidth + safe;

        if (op[0] >= minX && op[0] <= maxX && op[1] >= minY && op[1] <= maxY) return;

        // handle direction changes

        OnIllegalPosition();
    }

    void OnIllegalPosition()
    {
        stopped = true;
        OnDirectionChange();

        game.PlayerStuck();
    }

    public void GoalReached()
    {
        stopped = true;
        OnDirectionChange();
        game.GoalReached(this);
    }

    bool ShouldMove()
    {
        return game.IsSimulating() && !stopped;
    }

    void UpdateVelocity()
    {
        rb.velocity = ShouldMove() ? Multiply(direction.movement, speed) : Vector2.zero;
    }

    Vector2 Multiply(Vector2 v, float m)
    {
        if (m == 1.0f) return v;
        return new Vector2(v[0] * m, v[1] * m);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (game.CanMoveTiles()) return;

        if (other.gameObject.tag == "Finish")
        {
            GoalTile script = other.GetComponent<GoalTile>();
            script.OnCollision(this);
        }
        else if (other.gameObject.tag == "coin")
        {
            Coin script = other.GetComponent<Coin>();
            script.Collected();
        }
        else if (other.gameObject.tag == "wall")
        {
            // Hitbox of player is behind the middle point, so changing direction next to a wall may result in the new hitbox being inside the wall
            if (!game.IsWall(GetTileCoordinates()) && !game.IsWall(GetTileCoordinatesFront())) return;

            OnIllegalPosition();
        }
        else if (other.gameObject.tag == "direction_change")
        {
            TileDirection script = other.GetComponent<TileDirection>();
            script.OnCollision(this);
            OnDirectionChange();
        }
        else if (other.gameObject.tag == "rotation_change")
        {
            TileRotate script = other.GetComponent<TileRotate>();
            script.OnCollision(this);
            OnDirectionChange();
        }
    }

    void UpdateSprite()
    {
        anim.SetInteger("Direction", direction.id);
    }

    void FixHitbox()
    {
        bc.offset = hitboxOffsets[direction.id];
    }

    public int[] GetTileCoordinates()
    {
        return game.GetTileCoordinates(tr.position[0], tr.position[1]);
    }

    public int[] GetTileCoordinates(Direction direction, int offset)
    {
        int[] coordinates = game.GetTileCoordinates(tr.position[0], tr.position[1]);
        switch (direction.id)
        {
            case 0:
                coordinates[1] += offset;
                break;
            case 1:
                coordinates[0] += offset;
                break;
            case 2:
                coordinates[1] -= offset;
                break;
            case 3:
                coordinates[0] -= offset;
                break;
        };

        return coordinates;
    }

    public int[] GetTileCoordinatesFront()
    {
        return GetTileCoordinates(direction, 1);
    }

    public int[] GetTileCoordinatesBehind()
    {
        return GetTileCoordinates(direction.GetOpposite(), 1);
    }
}

