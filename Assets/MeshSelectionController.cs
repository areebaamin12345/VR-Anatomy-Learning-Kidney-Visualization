using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class MeshSelectionController : MonoBehaviour
{
    [Header("UI Elements")]
    [Tooltip("The small label that shows during Hover.")]
    public GameObject hoverLabel;

    [Tooltip("The large label that shows during Grab.")]
    public GameObject grabDescription;

    private XRGrabInteractable grabInteractable;

    void Awake()
    {
        // Find the Grab component on this kidney mesh
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // 1. COMPLETELY HIDE the hover label so it doesn't flicker
        if (hoverLabel != null)
        {
            hoverLabel.SetActive(false);
        }

        // 2. SHOW only the Grab Description
        if (grabDescription != null)
        {
            grabDescription.SetActive(true);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // 1. HIDE the big Grab Description immediately
        if (grabDescription != null)
        {
            grabDescription.SetActive(false);
        }

        // 2. We don't force 'hoverLabel' to true here because 
        // Unity's XR system will trigger Hover naturally when your hand is near.
    }

    // This extra check ensures that if Unity tries to turn Hover back on 
    // while you are holding it, we kill it instantly.
    void Update()
    {
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            if (hoverLabel != null && hoverLabel.activeSelf)
            {
                hoverLabel.SetActive(false);
            }
        }
    }
}