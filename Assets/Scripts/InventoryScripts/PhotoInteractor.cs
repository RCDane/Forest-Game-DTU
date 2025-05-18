using UnityEngine;

public class PhotoInteractor : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Picking up: " + gameObject.name);

        // Add this unique world object directly
        InventoryManager.Instance.AddItem(gameObject);
    }
}
