using UnityEngine;


public class HoverSilencer : MonoBehaviour
{
    public GameObject hoverLabel;    // The small label
    public GameObject descriptionUI; // The big description

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    void Update()
    {
        // FORCE CHECK: If the object is currently being grabbed/selected
        if (grabInteractable != null && grabInteractable.isSelected)
        {
            // If it's being held, the hover MUST be off
            if (hoverLabel != null && hoverLabel.activeSelf)
            {
                hoverLabel.SetActive(false);
            }

            // The description MUST be on
            if (descriptionUI != null && !descriptionUI.activeSelf)
            {
                descriptionUI.SetActive(true);
            }
        }
    }
}