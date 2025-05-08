using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    [SerializeField]
    RenderTexture renderTexture;

    InputAction rightTriggerPress;
    [SerializeField]
    InputActionAsset actions;

    //[SerializeField]
    Texture2D DisplayTexture;
    Material displayMaterial;

    [SerializeField]
    MeshRenderer displayMeshRenderer;
    bool isPressed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rightTriggerPress = actions.FindAction("XRI Right Interaction/Activate");
        displayMaterial = displayMeshRenderer.material;
        DisplayTexture = new Texture2D(renderTexture.width, renderTexture.height, UnityEngine.Experimental.Rendering.DefaultFormat.LDR, 1, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
        displayMaterial.SetTexture("_BaseMap", DisplayTexture);
    }

    bool GetTriggerPress()
    {
        bool newPress = rightTriggerPress.inProgress && !isPressed;
        isPressed = rightTriggerPress.inProgress;



        if (newPress)
        {
            isPressed = true;
            return true;
        }
        return false;
    }

    void TakePicture()
    {
        Graphics.CopyTexture(renderTexture.graphicsTexture, DisplayTexture.graphicsTexture);
    }

    // Update is called once per frame
    void Update()
    {
        bool triggerPressed = GetTriggerPress();

        if (triggerPressed)
        {
            print("Hello world");
            TakePicture();
        }

    }

    void OnActivate(InputAction action)
    {
        Instantiate(camera);
    }
}
