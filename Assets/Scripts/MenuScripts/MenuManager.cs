using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    // Singleton used for object reference
    public static MenuManager _;

    // Flag to toggle debug mode
    [SerializeField] private bool _debugMode;

    // Containers for Menu children components
    [SerializeField] GameObject _MenuContainer;
    [SerializeField] GameObject _MainMenuContainer;
    [SerializeField] GameObject _OptionsMenuContainer;
    [SerializeField] GameObject _CreditsMenuContainer;
    
    [SerializeField] private Transform _cameraTransform; // Assign Main Camera (from XR Rig)
    [SerializeField] private float _menuDistance = 2f;   // Distance in front of player
    private Vector3 _initialMenuPosition;
    private Quaternion _initialMenuRotation;

    private bool _hasPositionedMenuOnce = false;

    [SerializeField] private InputActionAsset actions;  // Input Actions asset
    private InputAction pauseAction;                    // Reference to a specific action

    // Reference to player movement script to enable/disable movement
    // [SerializeField] private MonoBehaviour _playerMovementScript;

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
        // Initialize MenuManager singleton
        if (_ == null)
        {
            _ = this;
        }
        else
        {
            Debug.LogError("More than 1 MenuManager detected in the scene");
        }

        // Find and assign the pause action
        pauseAction = actions.FindAction("XRI Left Interaction/Scale Toggle"); // Joystick Click       

        if (pauseAction != null)
        {
            pauseAction.performed += ctx => TogglePause(); // Attach event
            pauseAction.Enable(); // Enable the input
        }
        else
        {
            Debug.LogWarning("Pause action not found in InputActionAsset.");
        }
    }

    public void Start()
    {
        if (_cameraTransform == null && Camera.main != null)
            _cameraTransform = Camera.main.transform;

        // Store initial static menu position
        _initialMenuPosition = _MenuContainer.transform.position;
        _initialMenuRotation = _MenuContainer.transform.rotation;

        PauseGame();
    }


    private void TogglePause()
    {
        DebugMessage("TogglePause!!!");
        if (_isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void UpdateMenuPosition()
    {
        if (_cameraTransform == null) return;

        Vector3 forward = _cameraTransform.forward;
        Vector3 targetPosition = _cameraTransform.position + forward * _menuDistance;

        _MenuContainer.transform.position = targetPosition;
        _MenuContainer.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
        DebugMessage("MenuContainer updated: " + _MenuContainer.transform.position.ToString());
    }

    public void PauseGame()
    {
        DebugMessage("Pause Game triggered");

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        OpenMenu(_MainMenuContainer);
        _isPaused = true;

        // Ensure menu stays in its original place
        _MenuContainer.transform.position = _initialMenuPosition;
        _MenuContainer.transform.rotation = _initialMenuRotation;
    }


    public void ResumeGame()
    {
        DebugMessage("Resume Game triggered");

        // Hide and lock the cursor - Used for PC controller
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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

        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.button1click);
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