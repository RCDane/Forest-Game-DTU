using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Singleton used for object reference
    public static MainMenuManager _;

    // Flag used for debug mode
    [SerializeField] private bool _debugMode;

    // Name of the scene/level which should start when clicking play
    [SerializeField] private string _sceneToLoadAfterClickingPlay;
    [SerializeField] GameObject _MainMenuContainer;
    [SerializeField] GameObject _OptionsMenuContainer;
    [SerializeField] GameObject _CreditsMenuContainer;

    // Setup buttons for menu views
    public enum MainMenuButtons { play, options, credits, quit };
    public enum OptionsMenuButtons { back };
    public enum CreditsMenuButtons { back };
    
    // Flag to check if game is paused, i.e. menu should be displayed
    private bool _isPaused = false;

    private void DebugMessage(string message)
    {
        // Used for console debugging
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    public void Awake()
    {
        // Initialize MainMenuManager singleton for references between scripts
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("More than 1 MainMenuManager detected in the scene");
        }
    }

    public void Start()
    {
        OpenMenu(_MainMenuContainer);
    }

    void Update()
    {
        // TODO: 
        // - Handle correct user input in VR setup
        // - Check if menu is open, then pause game

        // Check if Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Pause game after timescale and open main menu
        Time.timeScale = 0f;
        OpenMenu(_MainMenuContainer);
        _isPaused = true;
    }
    public void ResumeGame()
    {
        // Resume game after timescale and close all menus
        Time.timeScale = 1f;
        CloseAllMenus();
        _isPaused = false;
    }

    public void OpenMenu(GameObject menuToOpen)
    {
        // Function to open the chosen menu
        _MainMenuContainer.SetActive(menuToOpen == _MainMenuContainer);
        _CreditsMenuContainer.SetActive(menuToOpen == _CreditsMenuContainer);
        _OptionsMenuContainer.SetActive(menuToOpen == _OptionsMenuContainer);
    }
    public void OpenOptionsMenu()
    {
        OpenMenu(_OptionsMenuContainer);
    }
    public void OpenCreditsMenu()
    {
        OpenMenu(_CreditsMenuContainer);
    }
    public void OpenMainMenu()
    {
        OpenMenu(_MainMenuContainer);
    }
    private void CloseAllMenus()
    {
        _MainMenuContainer.SetActive(false);
        _OptionsMenuContainer.SetActive(false);
        _CreditsMenuContainer.SetActive(false);
    }

    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        // Handles Main Menu button logic
        DebugMessage("Main Meny Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            case MainMenuButtons.options:
                OpenOptionsMenu();
                break;
            case MainMenuButtons.credits:
                OpenCreditsMenu();
                break;
            case MainMenuButtons.quit:
                QuitGame();
                break;
            default:
                Debug.Log("Button clicked has not yet been implemented in MainMenuManager method");
                break;
        }
    }
    public void OptionsMenuButtonClicked(OptionsMenuButtons buttonClicked)
    {
        // Handles Options Menu button logic
        DebugMessage("Options Menu Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case OptionsMenuButtons.back:
                OpenMainMenu();
                break;
            default:
                Debug.Log("Button clicked has not yet been implemented in MainMenuManager method");
                break;
        }
    }
    public void CreditsMenuButtonClicked(CreditsMenuButtons buttonClicked)
    {
        // Handles Credits Menu button logic
        DebugMessage("Credits Menu Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case CreditsMenuButtons.back:
                OpenMainMenu();
                break;
            default:
                Debug.Log("Button clicked has not yet been implemented in MainMenuManager method");
                break;
        }
    }

    public void PlayClicked()
    {
        // Currently loads playable scene
        SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }

    public void QuitGame()
    {
        // Quits application
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
