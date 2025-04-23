using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    protected Animator anim;
    protected Transform tr;


    protected GameHandler game;


    bool collected;

    bool generated;

    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();

        game = GameObject.Find("background").GetComponent<GameHandler>();

        collected = false;
        generated = false;
    }

    void AddToGame()
    {
        game.AddCoin(this);
    }

    void Update()
    {
        if (!generated)
        {
            AddToGame();
            generated = true;
        }
    }

    public void Collected()
    {
        if (collected) return;
        collected = true;

        anim.SetBool("Collected", true);

        game.coinsCollected++;
    }

    public int[] GetTileCoordinates()
    {
        return game.GetTileCoordinates(tr.position[0], tr.position[1]);
    }
}
