using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class PhotoController : MonoBehaviour
{
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    Renderer pictureRenderer;
    Color baseColor = Color.black;
    Vector3 lastPosition;
    float shakeProgress = 0f;
    float shakeSensitivity = 1.5f; // minimum velocity to trigger shake effect
    float fadeSpeed = 0.5f; // speed at which it fades when shaking

    void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // NOTE: Might be useful later, we can check which controller grabbed it
        //if (args.interactorObject.transform.parent.name.Contains("Left"))
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // Kinematic was true before the first grab as it was attached to the camera, so set it to false
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        // disable the animator cuz otherwise it keeps the end position
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform pictureChild = transform.Find("Picture");
        lastPosition = transform.position;
        if (pictureChild != null)
        {
            pictureRenderer = pictureChild.GetComponent<Renderer>();
            if (pictureRenderer != null)
            {
                // Assume the material was already overridden in instantiating script
                pictureRenderer.material.color = Color.black;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeProgress < 1.0f)
        {
            Vector3 movement = transform.position - lastPosition;
            float velocity = movement.magnitude / Time.deltaTime;
            lastPosition = transform.position;
            //print("Velocity: " + velocity);
            if (velocity > shakeSensitivity)
            {
                shakeProgress += Time.deltaTime * fadeSpeed;
                shakeProgress = Mathf.Clamp01(shakeProgress);

                // Lerp from black to white
                if (pictureRenderer != null)
                {
                    pictureRenderer.material.color = Color.Lerp(Color.black, Color.white, shakeProgress);
                    //print("Shake progress: " + shakeProgress);
                }
            }
        }
    }
}
