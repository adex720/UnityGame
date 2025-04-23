using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : Tile
{

    private bool generated;

    public override void OnStart()
    {
        shouldAddToInventory = NEVER_ADD_TO_INVENTORY;
        generated = false;
    }

    public override void OnCollision(Player player)
    {
        player.GoalReached();
    }

    public override void UpdateSprite()
    {
        if (!generated)
        {
            game.AddPosition(game.GetTileCoordinates(tr.position[0], tr.position[1]));
            generated = true;
        }
    }

    public override int GetTypeId()
    {
        return 0;
    }

    public override int GetValueId()
    {
        return 0;
    }
}
