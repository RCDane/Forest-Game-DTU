using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Singleton used for reference
    public static MainMenuManager _;

    // Flag used for debug mode
    [SerializeField] private bool _debugMode;

    // Name of the scene/level which should start when clicking play
    [SerializeField] private string _sceneToLoadAfterClickingPlay;
    [SerializeField] GameObject _MainMenuContainer;
    [SerializeField] GameObject _CreditsMenuContainer;

    public enum MainMenuButtons { play, options, credits, quit };
    public enum CreditsMenuButtons { back };
    
    private bool _isPaused = false;

    public void Awake()
    {
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
        Time.timeScale = 0f; // Pause the game
        OpenMenu(_MainMenuContainer); // Show main menu
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Resume the game
        CloseAllMenus(); // Hide menus
        _isPaused = false;
    }

    private void CloseAllMenus()
    {
        _MainMenuContainer.SetActive(false);
        _CreditsMenuContainer.SetActive(false);
    }

    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Main Meny Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            // case MainMenuButtons.options:
            //     break;
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

    public void OpenCreditsMenu()
    {
        OpenMenu(_CreditsMenuContainer);
    }

    public void ReturnToMainMenu()
    {
        OpenMenu(_MainMenuContainer);
    }

    public void CreditsMenuButtonClicked(CreditsMenuButtons buttonClicked)
    {
        DebugMessage("Credits Menu Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case CreditsMenuButtons.back:
                ReturnToMainMenu();
                break;
            default:
                Debug.Log("Button clicked has not yet been implemented in MainMenuManager method");
                break;
        }
    }

    private void DebugMessage(string message){
        if (_debugMode)
        {
            Debug.Log(message);
        }
    }

    // Function to open a given menu
    public void OpenMenu(GameObject menuToOpen)
    {
        _MainMenuContainer.SetActive(menuToOpen == _MainMenuContainer);
        _CreditsMenuContainer.SetActive(menuToOpen == _CreditsMenuContainer);
    }

    public void PlayClicked()
    {
        SceneManager.LoadScene(_sceneToLoadAfterClickingPlay);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }
}
