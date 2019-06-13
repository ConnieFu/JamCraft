using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public static bool s_IsPaused = false;
    [SerializeField] private UIHandler m_UIHandler;

    [Header("Gameplay Elements")]
    [SerializeField] private CharacterController m_CharacterController;

    void Start()
    {
        // show main menu first
        ShowMainMenu();
    }

    public void PlayGame(bool resumed = false)
    {
        // initialize gameplay elements
        if (!resumed)
        {
            m_CharacterController.Initialize();
        }
        s_IsPaused = false;
        m_UIHandler.ToggleMenuUI(false);
    }

    public void PauseGame()
    {
        s_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.PAUSE_MENU_SCENE_NAME);
    }

    public void ShowMainMenu()
    {
        s_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.MAIN_MENU_SCENE_NAME);
    }

    public void ShowGameOver()
    {
        s_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.GAME_OVER_MENU_SCENE_NAME);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
