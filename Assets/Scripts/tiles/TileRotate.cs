using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRotate : Tile
{

    public bool isClowckwise = false;

    public override void OnStart()
    {
    }

    public override void OnCollision(Player player)
    {
        player.direction = isClowckwise ? player.direction.GetClockwise() : player.direction.GetCounterclockwise();
    }

    public override void UpdateSprite()
    {
        anim.SetBool("IsClockwise", isClowckwise);
    }

    public override int GetTypeId()
    {
        return 2;
    }

    public override int GetValueId()
    {
        return isClowckwise ? 0 : 1;
    }
}
