using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileDirection : DirectionalTile
{

    public override void OnCollision(Player player)
    {
        player.direction = direction;
    }

    public override int GetTypeId()
    {
        return 1;
    }

}