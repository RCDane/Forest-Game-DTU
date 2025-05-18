using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact();
}

public class Interactor : MonoBehaviour
{
    public Transform interactorSource;
    public float interactorRange;
    public LayerMask interactorLayer;

    // List to hold interactable objects
    private List<IInteractable> inventory = new List<IInteractable>();

    void Update()
{
    bool interactPressed = 
        (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) ||
        (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame);

    if (interactPressed)
    {
        CheckInteract();
    }
}

    private void CheckInteract()
    {
        Collider[] colliders = Physics.OverlapSphere(interactorSource.position, interactorRange, interactorLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<IInteractable>(out var interactObj))
            {
                float distance = Vector3.Distance(interactorSource.position, collider.transform.position);
                if (distance <= interactorRange && !inventory.Contains(interactObj))
                {
                    Debug.Log("Picking up object: " + collider.gameObject.name); // Logs object name

                    interactObj.Interact();
                    inventory.Add(interactObj);
                }
            }
        }
    }
}