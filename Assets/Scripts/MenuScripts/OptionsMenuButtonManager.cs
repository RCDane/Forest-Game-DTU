using UnityEngine;

public class OptionsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MenuManager.OptionsMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MenuManager._.OptionsMenuButtonClicked(_buttonType);
    }
}
