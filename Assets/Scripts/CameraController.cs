using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class CameraController : MonoBehaviour
{
    [SerializeField]
    Camera camera;

    public GameObject photo;
    GameObject currentPhoto = null;

    Transform leftGrabPoint;
    Transform rightGrabPoint;

    [SerializeField]
    RenderTexture renderTexture;

    InputAction rightTriggerPress;
    [SerializeField]
    InputActionAsset actions;

    //[SerializeField]
    Texture2D DisplayTexture;
    //Material displayMaterial;

    [SerializeField]
    private AudioClip cameraTriggerSound;

    bool isPressed = false;
    bool wasPressed = false;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;
    
    Animator animator;

    [SerializeField]
    CreatureManager creatureManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rightTriggerPress = actions.FindAction("XRI Right Interaction/Activate");
        // right trigger is used for taking pictures, so it's disabled until we hold the camera
        // NOTE: Dunno if it disables it globally or just for this object
        rightTriggerPress.Disable();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
        leftGrabPoint = transform.Find("LeftGrabPoint");
        rightGrabPoint = transform.Find("RightGrabPoint");
        animator = transform.Find("Camera Object")?.GetComponent<Animator>();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.parent.name.Contains("Right"))
        {
            rightTriggerPress.Enable();
            //grab.attachTransform = rightGrabPoint.transform; it applies only to the next grab :(
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.parent.name.Contains("Right"))
        {
            rightTriggerPress.Disable();
        }
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
        DisplayTexture = new Texture2D(renderTexture.width, renderTexture.height, UnityEngine.Experimental.Rendering.DefaultFormat.LDR, 1, UnityEngine.Experimental.Rendering.TextureCreationFlags.None);
        Graphics.CopyTexture(renderTexture.graphicsTexture, DisplayTexture.graphicsTexture);
        currentPhoto = Instantiate(photo, transform.position, transform.rotation);
        // Find the child named "Picture" and get its Renderer
        Transform pictureChild = currentPhoto.transform.Find("Picture");
        if (pictureChild != null)
        {
            Renderer photoRenderer = pictureChild.GetComponent<Renderer>();
            if (photoRenderer != null)
            {
                photoRenderer.material = new Material(photoRenderer.material); // Avoid modifying shared material
                //AudioManagerScript.Instance.PlaySFX(cameraTriggerSound);

                

                photoRenderer.material.SetTexture("_BaseMap", DisplayTexture);
                photoRenderer.material.SetTextureScale("_BaseMap", new Vector2(-1, -1));
                photoRenderer.material.SetTextureOffset("_BaseMap", new Vector2(1, 1));
                creatureManager.TryTakePicture(camera);
            }
            else
            {
                Debug.LogWarning("Renderer not found on child 'Picture'.");
            }
        }
        else
        {
            Debug.LogWarning("Child named 'Picture' not found.");
        }
        currentPhoto.transform.SetParent(transform);
        currentPhoto.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        // Set Rigidbody to kinematic to avoid physics interactions
        Rigidbody rb = currentPhoto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool triggerPressed = GetTriggerPress();

        if (triggerPressed)
        {
            if (animator != null)
            {
                animator.SetTrigger("ChangeState");
            }
            // Prevent taking another picture if one is attached to the camera
            if (currentPhoto == null)
            {
                TakePicture();
            }
            wasPressed = true;
        }
        // Check if the trigger was released
        if (!isPressed && wasPressed)
        {
            if (animator != null)
            {
                animator.SetTrigger("ChangeState");
            }
            wasPressed = false;
        }
        // Check if the current photo is still a child of this object
        // If not, set it to null to allow for new instantiation
        if (currentPhoto != null && currentPhoto.transform.parent != transform)
        {
            currentPhoto = null;
        }
    }

    void OnActivate(InputAction action)
    {
        Instantiate(camera);
    }





}
