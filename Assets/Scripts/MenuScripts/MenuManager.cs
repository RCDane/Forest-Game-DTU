using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Singleton used for object reference
    public static MenuManager _;

    // Flag used for debug mode
    [SerializeField] private bool _debugMode;

    // Name of the scene/level which should start when clicking play
    [SerializeField] private string _sceneToLoadAfterClickingPlay;
    [SerializeField] GameObject _MenuContainer;
    [SerializeField] GameObject _MainMenuContainer;
    [SerializeField] GameObject _OptionsMenuContainer;
    [SerializeField] GameObject _CreditsMenuContainer;
    
    // Reference to player movement script to enable/disable movement
    [SerializeField] private MonoBehaviour _playerMovementScript;

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
        // Initialize MenuManager singleton for references between scripts
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("More than 1 MenuManager detected in the scene");
        }
    }

    public void Start()
    {
        // Pause the game and display the Main Meny upon application start
        PauseGame();
    }

    void Update()
    {
        // TODO: 
        // - Handle correct user input in VR setup

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
        DebugMessage("Pause Game triggered");
        
        // Disable player movement
        if (_playerMovementScript != null)
            _playerMovementScript.enabled = false;
        
        // Open Main Menu and set pause to true
        OpenMenu(_MainMenuContainer);
        _isPaused = true;
    }
    public void ResumeGame()
    {
        DebugMessage("Resume Game triggered");
        // SceneManager.LoadScene(_sceneToLoadAfterClickingPlay); // <--- In case of testing Main Menu scene
        
        // Enable player movement
        if (_playerMovementScript != null)
            _playerMovementScript.enabled = true;

        // Close all Menus and set pause to false
        CloseAllMenus();
        _isPaused = false;
    }

    public void OpenMenu(GameObject menuToOpen)
    {
        // Function to open the chosen menu
        _MenuContainer.SetActive(_MenuContainer);
        _MainMenuContainer.SetActive(menuToOpen == _MainMenuContainer);
        _CreditsMenuContainer.SetActive(menuToOpen == _CreditsMenuContainer);
        _OptionsMenuContainer.SetActive(menuToOpen == _OptionsMenuContainer);
    }
    public void OpenMainMenu()
    {
        OpenMenu(_MainMenuContainer);
    }
    public void OpenOptionsMenu()
    {
        OpenMenu(_OptionsMenuContainer);
    }
    public void OpenCreditsMenu()
    {
        OpenMenu(_CreditsMenuContainer);
    }
    private void CloseAllMenus()
    {
        _MenuContainer.SetActive(false);
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
                Debug.Log("Button clicked has not yet been implemented in MenuManager method");
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
                Debug.Log("Button clicked has not yet been implemented in MenuManager method");
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
                Debug.Log("Button clicked has not yet been implemented in MenuManager method");
                break;
        }
    }

    public void PlayClicked()
    {
        ResumeGame();
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