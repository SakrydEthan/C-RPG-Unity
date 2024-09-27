using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{

    public static GameStateController instance;
    public GameState gameState { get; private set; } 

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void TogglePause()
    {
        if(instance.gameState == GameState.Paused)
        {
            UnpauseGame();
        }
        else
        {
            PauseGame();
        }
    }

    public static void PauseGame()
    {
        UIPause.OpenPauseMenu();
        PlayerInstanceController.FreezePlayer();
        Time.timeScale = 0f;
    }
    public static void UnpauseGame()
    {
        UIPause.ClosePauseMenu();
        PlayerInstanceController.UnfreezePlayer();
        Time.timeScale = 1f;
    }

    public static void QuitGame()
    {
        Application.Quit();
    }
}

public enum GameState
{
    Active,
    Paused,
    PlayerDead
}
