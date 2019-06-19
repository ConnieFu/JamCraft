using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    private static GameFlow m_Instance;

    [SerializeField] private UIHandler m_UIHandler;
    [SerializeField] private Canvas m_Canvas;

    [Header("Gameplay Elements")]
    [SerializeField] private Player m_PlayerController;
    [SerializeField] private GridManager m_GridManager;
    [SerializeField] private CraftingManager m_CraftingManager;
    [SerializeField] private EnemySpawner m_EnemySpawner;

    [SerializeField] private Transform m_FloatingResourcesParent;

    public delegate void OnCraftingMenuClosed();
    private OnCraftingMenuClosed m_CraftingMenuClosed;


    private bool m_IsPaused = false;
    private bool m_IsCrafting = false;

    #region properties
    public static GameFlow Instance
    {
        get
        {
            return m_Instance;
        }
    }

    public bool IsPaused
    {
        get
        {
            return m_IsPaused;
        }
    }

    public GridManager GridManager
    {
        get
        {
            return m_GridManager;
        }
    }

    public Transform FloatingResourcesParent
    {
        get
        {
            return m_FloatingResourcesParent;
        }
    }

    public bool IsCrafting
    {
        get
        {
            return m_IsCrafting;
        }
    }

    public Canvas Canvas
    {
        get
        {
            return m_Canvas;
        }
    }

    public OnCraftingMenuClosed CraftingMenuClosed
    {
        get
        {
            return m_CraftingMenuClosed;
        }
        set
        {
            m_CraftingMenuClosed = value;
        }
    }

    public Player PlayerController
    {
        get
        {
            return m_PlayerController;
        }
    }
    #endregion

    void Awake()
    {
        if (m_Instance != null && m_Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            m_Instance = this;
        }

        Screen.SetResolution(1024, 768, false);

        // initialize everything
        m_GridManager.Initialize();
        m_CraftingManager.Initialize();
        m_PlayerController.Initialize();
        m_EnemySpawner.Initialize();

        // show main menu first
        m_Instance.ShowMainMenu();
    }

    public void PlayGame(bool resumed = false)
    {
        // reset gameplay elements
        if (!resumed)
        {
            m_GridManager.Reset();
            m_CraftingManager.Reset();
            m_PlayerController.Reset();
            m_EnemySpawner.Reset();
        }
        m_IsPaused = false;
        m_UIHandler.ToggleMenuUI(false);
    }

    public void PauseGame()
    {
        m_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.PAUSE_MENU_SCENE_NAME);
    }

    public void ShowMainMenu()
    {
        m_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.MAIN_MENU_SCENE_NAME);
    }

    public void ShowGameOver()
    {
        m_IsPaused = true;
        m_UIHandler.ToggleMenuUI(true);
        m_UIHandler.ShowMenu(GameConstants.GAME_OVER_MENU_SCENE_NAME);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region Crafting
    public void ShowCraftingMenu()
    {
        m_IsPaused = true;
        m_IsCrafting = true;
        m_UIHandler.ShowMenu(GameConstants.CRAFTING_MENU_NAME);
    }

    public void HideCraftingMenu()
    {
        m_IsPaused = false;
        m_IsCrafting = false;
        m_UIHandler.HideMenu(GameConstants.CRAFTING_MENU_NAME);

        if (m_CraftingMenuClosed != null)
        {
            m_CraftingMenuClosed();
        }
    }
    #endregion
}
