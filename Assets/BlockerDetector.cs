using UnityEngine;


public class BlockerDetector : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    void Start()
    {
        rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
    }

    void Update()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            // This prints the name of whatever the ray is hitting to the Console
            Debug.Log("LASER HIT SOMETHING: " + hit.collider.gameObject.name + " | Layer: " + hit.collider.gameObject.layer);
        }
    }
}
