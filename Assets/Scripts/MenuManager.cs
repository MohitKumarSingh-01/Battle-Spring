using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public string levelName;
    public bool reset = false;
    public int startLives = 3;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        levelName = "Level 1";
    }
    public void loadLevel(string levelToLoad)
    {
        PlayerPrefManager.ResetPlayerState(startLives, false);

        SceneManager.LoadScene(levelToLoad);
    }

    public void LoadButton()
    {
        loadLevel(levelName);
    }
    public void MainMenu()
    {
        loadLevel("MainMenu");
    }
    public void ResetButton()
    {
        PlayerPrefManager.ResetPlayerState(startLives, true);
    }

    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
        //Application.Quit();
    }
}
