using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Button : MonoBehaviour
{

    protected Animator anim;
    protected Transform tr;


    protected GameHandler game;
    protected TileInventory tileInventory;

    protected readonly float width;
    protected readonly float height;
    protected readonly KeyCode key;

    public Button(float w, float h, KeyCode k)
    {
        width = w;
        height = h;
        key = k;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        tr = GetComponent<Transform>();

        game = GameObject.Find("background").GetComponent<GameHandler>();
        tileInventory = GameObject.Find("tile_inventory").GetComponent<TileInventory>();

        game.AddAnimatorUpdator(gs => UpdateSprite(gs));
    }

    void Update()
    {
        CheckMouse();
        CheckKey();
    }

    void CheckMouse()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (!IsClickInside()) return;
        OnClick();
    }

    void CheckKey()
    {
        if (Input.GetKeyDown(key))
        {
            OnClick();
        }
    }

    public bool IsClickInside()
    {
        Vector2 mp = GameHandler.GetMousePos(); // mouse pos

        Vector2 op = tr.position; // own pos
        float halfWidth = width / 2;
        float halfHeight = height / 2;

        if (mp[0] < op[0] - halfWidth || mp[0] > op[0] + halfWidth || mp[1] < op[1] - halfHeight || mp[1] > op[1] + halfHeight)
        {
            return false;
        }
        return true;
    }

    public abstract void OnClick();

    public abstract void UpdateSprite(GameState gameState);

}