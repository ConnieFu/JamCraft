using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    [SerializeField] private UIHandler m_UIHandler;

    void Start()
    {
        // show main menu first
        ShowMainMenu();
    }

    public void PlayGame(bool resumed = false)
    {
        m_UIHandler.ToggleMenuUI(false);
    }

    public void PauseGame()
    {
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.PAUSE_MENU_SCENE_NAME);
    }

    public void ShowMainMenu()
    {
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.MAIN_MENU_SCENE_NAME);
    }

    public void ShowGameOver()
    {
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.GAME_OVER_MENU_SCENE_NAME);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
