using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private DataManager dataManager;

    void Start()
    {
        dataManager = new DataManager("../game_data.dt");
    }

    public void LoadNextLevel()
    {
        LoadLevel(dataManager.GetInt(DataObject.LEVELS_COMPLETED) + 1);
    }

    public void LoadLevel(int id)
    {
        SceneManager.LoadScene("Level" + id);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
