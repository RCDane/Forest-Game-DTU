using UnityEngine;

public class CreditsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MenuManager.CreditsMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MenuManager._.CreditsMenuButtonClicked(_buttonType);
    }
}
