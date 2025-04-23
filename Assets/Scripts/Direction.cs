using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public static readonly Direction NORTH = new Direction(0, "North", new Vector2(0f, 1f));
    public static readonly Direction EAST = new Direction(1, "East", new Vector2(1f, 0f));
    public static readonly Direction SOUTH = new Direction(2, "South", new Vector2(0f, -1f));
    public static readonly Direction WEST = new Direction(3, "West", new Vector2(-1f, 0f));

    public static readonly bool DIRECTIONS_LOADED = InitParams();

    public readonly int id;
    public readonly string name;
    public readonly Vector2 movement;
    private Direction opposite;
    private Direction clockwise;
    private Direction counterclockwise;

    private Direction(int id, string name, Vector2 movement)
    {
        this.id = id;
        this.name = name;
        this.movement = movement;
    }


    public Direction GetOpposite()
    {
        return opposite;
    }

    public Direction GetClockwise()
    {
        return clockwise;
    }

    public Direction GetCounterclockwise()
    {
        return counterclockwise;
    }

    public static Direction GetById(int id)
    {
        return id switch
        {
            0 => NORTH,
            1 => EAST,
            2 => SOUTH,
            3 => WEST,
            _ => null
        };
    }

    public static bool InitParams()
    {
        NORTH.opposite = SOUTH;
        NORTH.clockwise = EAST;
        NORTH.counterclockwise = WEST;

        EAST.opposite = WEST;
        EAST.clockwise = SOUTH;
        EAST.counterclockwise = NORTH;

        SOUTH.opposite = NORTH;
        SOUTH.clockwise = WEST;
        SOUTH.counterclockwise = EAST;

        WEST.opposite = EAST;
        WEST.clockwise = NORTH;
        WEST.counterclockwise = SOUTH;

        return true;
    }

}