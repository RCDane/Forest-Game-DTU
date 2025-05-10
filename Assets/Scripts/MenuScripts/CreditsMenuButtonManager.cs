using UnityEngine;

public class CreditsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MainMenuManager.CreditsMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MainMenuManager._.CreditsMenuButtonClicked(_buttonType);
    }
}
