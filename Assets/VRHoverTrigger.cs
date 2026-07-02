using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

// This ensures the object MUST have an XR Simple Interactable component
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable))]
public class VRHoverTrigger : MonoBehaviour
{
    private AnatomyLabel anatomyLabel;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable interactable;

    void Awake()
    {
        // Get the references automatically
        anatomyLabel = GetComponent<AnatomyLabel>();
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
    }

    void OnEnable()
    {
        // Subscribe to the VR Hover events
        interactable.hoverEntered.AddListener(HandleHoverEnter);
        interactable.hoverExited.AddListener(HandleHoverExit);
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or errors
        interactable.hoverEntered.RemoveListener(HandleHoverEnter);
        interactable.hoverExited.RemoveListener(HandleHoverExit);
    }

    private void HandleHoverEnter(HoverEnterEventArgs args)
    {
        if (anatomyLabel != null)
        {
            anatomyLabel.ShowLabel();
        }
    }

    private void HandleHoverExit(HoverExitEventArgs args)
    {
        if (anatomyLabel != null)
        {
            anatomyLabel.HideLabel();
        }
    }
}