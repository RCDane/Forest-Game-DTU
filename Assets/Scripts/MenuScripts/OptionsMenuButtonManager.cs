using UnityEngine;

public class OptionsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.OptionsMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MainMenuManager._.OptionsMenuButtonClicked(_buttonType);
    }
}
