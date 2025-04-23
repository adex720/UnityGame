using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReset : Button
{

    public ButtonReset() : base(0.64f, 0.64f, KeyCode.R)
    {
    }

    public override void OnClick()
    {
        if (!game.CanReset()) return;

        game.Reset();
    }

    public override void UpdateSprite(GameState gameState)
    {
        anim.SetBool("Avaible", game.CanReset());
    }
}
