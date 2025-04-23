using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public TMPro.TextMeshProUGUI getAllCoinsText;
    public TMPro.TextMeshProUGUI reachGoalText;

    private GameHandler game;

    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("background").GetComponent<GameHandler>();

        getAllCoinsText.enabled = false;
        reachGoalText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        getAllCoinsText.enabled = game.showGetCoinsMessage;
        reachGoalText.enabled = game.showReachGoalMessage;
    }
}
