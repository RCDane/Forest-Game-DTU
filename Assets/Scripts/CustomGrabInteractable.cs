using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class CustomGrabInteractable : UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    public Transform leftGrabPoint;
    public Transform rightGrabPoint;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        var interactorTransform = args.interactorObject.transform;

        if (interactorTransform.parent.name.Contains("Right"))
        {
            attachTransform = rightGrabPoint;
        }
        else if (interactorTransform.parent.name.Contains("Left"))
        {
            attachTransform = leftGrabPoint;
        }

        base.OnSelectEntering(args); // Call base AFTER setting attachTransform
    }
}
