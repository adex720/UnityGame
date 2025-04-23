using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DirectionalTile : Tile
{

    public Direction direction;
    public int directionId = 0;


    public override void OnStart()
    {
        direction = Direction.GetById(directionId);
    }

    public override void UpdateSprite()
    {
        anim.SetInteger("Direction", direction.id);
    }

    public override int GetValueId()
    {
        return directionId;
    }

}