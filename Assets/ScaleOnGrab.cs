using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ScaleOnGrab : MonoBehaviour
{
    public float scaleMultiplier = 50f;
    private Vector3 smallScale;
    private Vector3 largeScale;
    private bool isCurrentlyGrabbed = false;
    private XRGrabInteractable grab;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        smallScale = transform.localScale;
        largeScale = smallScale * scaleMultiplier;
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(x => isCurrentlyGrabbed = true);
        grab.selectExited.AddListener(x => isCurrentlyGrabbed = false);
    }

    // LateUpdate runs AFTER the XR Controller has tried to reset the scale
    void LateUpdate()
    {
        if (isCurrentlyGrabbed)
        {
            transform.localScale = largeScale;
        }
        else
        {
            transform.localScale = smallScale;
        }
    }
}