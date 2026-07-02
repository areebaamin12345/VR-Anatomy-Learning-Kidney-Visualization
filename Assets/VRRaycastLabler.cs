using UnityEngine;

public class VRRaycastLabeler : MonoBehaviour
{
    public float rayDistance = 10f;
    private AnatomyLabel lastLabel;

    void Update()
    {
        // Shoot a ray forward from the controller
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // Check if what we hit has your AnatomyLabel script
            AnatomyLabel currentLabel = hit.collider.GetComponent<AnatomyLabel>();

            if (currentLabel != null && currentLabel != lastLabel)
            {
                if (lastLabel != null) lastLabel.HideLabel();
                currentLabel.ShowLabel();
                lastLabel = currentLabel;
            }
        }
        else if (lastLabel != null)
        {
            lastLabel.HideLabel();
            lastLabel = null;
        }
    }
}