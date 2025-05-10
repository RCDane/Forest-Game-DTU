using UnityEngine;

public class MainMenuBottonManager : MonoBehaviour
{
    [SerializeField] MenuManager.MainMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MenuManager._.MainMenuButtonClicked(_buttonType);
    }
}
