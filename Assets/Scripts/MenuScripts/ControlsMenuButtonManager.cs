using UnityEngine;

public class ControlsMenuButtonManager : MonoBehaviour
{
    [SerializeField] MenuManager.ControlsMenuButtons _buttonType;

    public void ButtonClicked()
    {
        MenuManager._.ControlsMenuButtonClicked(_buttonType);
    }
}
