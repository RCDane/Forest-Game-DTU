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

    public enum MainMenuButtons { play, options, credits, quit, backToMenu };

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

    public void MainMenuButtonClicked(MainMenuButtons buttonClicked)
    {
        DebugMessage("Button Clicked: " + buttonClicked.ToString());
        switch (buttonClicked)
        {
            case MainMenuButtons.play:
                PlayClicked();
                break;
            case MainMenuButtons.options:
                break;
            case MainMenuButtons.credits:
                break;
            case MainMenuButtons.backToMenu:
                break;
            case MainMenuButtons.quit:
                QuitGame();
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
