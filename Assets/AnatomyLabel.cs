using UnityEngine;
using TMPro;

public class AnatomyLabel : MonoBehaviour
{
    [Header("UI Reference")]
    public GameObject labelCanvas;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    [Header("Content")]
    public string partName;
    [TextArea] public string partDescription;

    void Awake()
    {
        Debug.Log($"[INIT] AnatomyLabel active on: {gameObject.name}");
        ValidateSetup();
    }

    void Start()
    {
        // This ensures the canvas is hidden immediately when the game starts
        // even if the box is checked in the Inspector.
        ForceHideCanvas();
    }

    public void ValidateSetup()
    {
        if (GetComponent<Collider>() == null)
            Debug.LogError($"<color=orange>[MISSING COLLIDER]</color> {gameObject.name} has no Collider!");

        if (GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>() == null)
            Debug.LogError($"<color=orange>[MISSING INTERACTABLE]</color> {gameObject.name} needs an XRSimpleInteractable!");
    }

    public void ShowLabel()
    {
        Debug.Log($"<color=cyan>[PHYSICS HIT]</color> VR Ray successfully hit: {gameObject.name}");

        if (labelCanvas == null || titleText == null || descriptionText == null)
        {
            Debug.LogError($"[UI ERROR] Missing assignments on {gameObject.name}");
            return;
        }

        titleText.text = partName;
        descriptionText.text = partDescription;

        // Logic transition: Use CanvasGroup alpha to show
        CanvasGroup cg = labelCanvas.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 1;
            cg.blocksRaycasts = true;
            Debug.Log($"<color=green>[UI SUCCESS]</color> Showing Label: {partName}");
        }
    }

    public void HideLabel()
    {
        Debug.Log($"<color=white>[EXIT]</color> Laser left: {gameObject.name}");
        ForceHideCanvas();
    }

    // Helper function to handle the "Hidden but Active" state properly
    private void ForceHideCanvas()
    {
        if (labelCanvas != null)
        {
            // We keep the GameObject ACTIVE so the XR Toolkit can still find it
            labelCanvas.SetActive(true);

            // Logic transition: Use CanvasGroup alpha to hide even if checked
            CanvasGroup cg = labelCanvas.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0;
                cg.blocksRaycasts = false;
            }
        }
    }
}