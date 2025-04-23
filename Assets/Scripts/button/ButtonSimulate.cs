using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSimulate : Button
{

    public ButtonSimulate() : base(1.92f, 0.64f, KeyCode.Space)
    {
    }

    public override void OnClick()
    {
        if (game.CanStartSimulation()) game.StartSimulation();
        else if (game.CanPause()) game.Pause();
        else if (game.CanResume()) game.Resume();
    }

    public override void UpdateSprite(GameState gameState)
    {
        anim.SetBool("Playing", gameState == GameState.SIMULATE);
    }
}
