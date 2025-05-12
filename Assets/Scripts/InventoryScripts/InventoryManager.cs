// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.InputSystem; // New Input System

// [System.Serializable]
// public class InventoryItem
// {
//     public string itemName;
//     public int itemQuantity;
//     public GameObject prefabReference;
// }


// public class InventoryManager : MonoBehaviour
// {
//     public static InventoryManager Instance;

//     public HashSet<InventoryItem> inventory = new HashSet<InventoryItem>();

//     // Inventory UI GameObject
//     [SerializeField] private GameObject _inventoryUI;

//     // Flag to track if inventory is open
//     private bool _isInventoryOpen = false;

//     private void Awake()
//     {
//         // Singleton pattern
//         if (Instance == null)
//             Instance = this;
//         else
//             Destroy(gameObject);
//     }

//     private void Update()
//     {
//         bool inventoryPressed = 
//             (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame) ||
//             (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame);

//         if (inventoryPressed)
//         {
//             if (_isInventoryOpen)
//                 CloseInventory();
//             else
//                 OpenInventory();
//         }

//         // Press O key to spawn an item back into the world
//         if (Keyboard.current != null && Keyboard.current.oKey.wasPressedThisFrame)
//         {
//             // Spawns item at position in front of player
//             Vector3 spawnPos = Camera.main.transform.position + Camera.main.transform.forward * 2f;
//             SpawnItem("Photo", spawnPos, Quaternion.identity);
//         }
//     }


//     private void OpenInventory()
//     {
//         _inventoryUI.SetActive(true);
//         _isInventoryOpen = true;

//         // Show cursor (for PC)
//         Cursor.visible = true;
//         Cursor.lockState = CursorLockMode.None;

//         // Optional: Pause movement/input here if you want
//     }

//     private void CloseInventory()
//     {
//         _inventoryUI.SetActive(false);
//         _isInventoryOpen = false;

//         // Hide cursor (for PC)
//         Cursor.visible = false;
//         Cursor.lockState = CursorLockMode.Locked;

//         // Optional: Resume movement/input here if you paused it
//     }

//     public void AddItem(string itemName, GameObject prefab)
//     {
//         InventoryItem existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);

//         if (existingItem != null)
//         {
//             existingItem.itemQuantity++;
//         }
//         else
//         {
//             InventoryItem newItem = new InventoryItem 
//             { 
//                 itemName = itemName, 
//                 itemQuantity = 1, 
//                 prefabReference = prefab
//             };
//             inventory.Add(newItem);
//         }
//     }

//     public void SpawnItem(string itemName, Vector3 spawnPosition, Quaternion spawnRotation)
//     {
//         InventoryItem existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);

//         if (existingItem != null && existingItem.itemQuantity > 0)
//         {
//             // Spawn the item back into the world
//             GameObject spawned = Instantiate(existingItem.prefabReference, spawnPosition, spawnRotation);

//             existingItem.itemQuantity--;

//             // Remove item if quantity reaches zero
//             if (existingItem.itemQuantity <= 0)
//             {
//                 inventory.Remove(existingItem);
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Item not found in inventory or quantity is zero: " + itemName);
//         }
//     }

//     public bool HasItem(string itemName)
//     {
//         InventoryItem existingItem = inventory.FirstOrDefault(item => item.itemName == itemName);
//         return existingItem != null && existingItem.itemQuantity > 0;
//     }
// }
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public HashSet<InventoryItem> inventory = new HashSet<InventoryItem>();

    [Header("UI Settings")]
    [SerializeField] private GameObject _inventoryUI;
    [SerializeField] private Transform inventoryContentPanel;
    [SerializeField] private GameObject inventoryButtonPrefab;

    private bool _isInventoryOpen = false;
    private List<GameObject> spawnedButtons = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        bool inventoryPressed =
            (Keyboard.current != null && Keyboard.current.iKey.wasPressedThisFrame) ||
            (Gamepad.current != null && Gamepad.current.buttonNorth.wasPressedThisFrame);

        if (inventoryPressed)
        {
            if (_isInventoryOpen)
                CloseInventory();
            else
                OpenInventory();
        }
    }

    private void OpenInventory()
    {
        _inventoryUI.SetActive(true);
        _isInventoryOpen = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        RefreshInventoryUI();
    }

    private void CloseInventory()
    {
        _inventoryUI.SetActive(false);
        _isInventoryOpen = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AddItem(GameObject worldObject)
    {
        string itemName = worldObject.name;

        InventoryItem existingItem = inventory.FirstOrDefault(item => item.worldObject == worldObject);

        if (existingItem != null)
        {
            existingItem.itemQuantity++;
        }
        else
        {
            InventoryItem newItem = new InventoryItem { itemName = itemName, itemQuantity = 1, worldObject = worldObject };
            inventory.Add(newItem);
        }

        // Deactivate the unique world object
        worldObject.SetActive(false);

        if (_isInventoryOpen)
            RefreshInventoryUI();
    }

    private void RefreshInventoryUI()
    {
        foreach (var btn in spawnedButtons)
            Destroy(btn);
        spawnedButtons.Clear();

        foreach (var item in inventory)
        {
            GameObject btnObj = Instantiate(inventoryButtonPrefab, inventoryContentPanel);
            //btnObj.GetComponentInChildren<TextMeshProUGUI>().text = item.itemName;

            btnObj.GetComponent<Button>().onClick.AddListener(() => SpawnItem(item));

            spawnedButtons.Add(btnObj);
        }
    }

    private void SpawnItem(InventoryItem item)
    {
        Debug.Log($"Releasing item back to world: {item.itemName}");

        if (item.worldObject != null)
        {
            Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
            item.worldObject.transform.position = spawnPosition;
            item.worldObject.SetActive(true);

            inventory.Remove(item);
            RefreshInventoryUI();
        }
    }

    public bool HasItem(string itemName)
    {
        return inventory.Any(item => item.itemName == itemName);
    }
}
