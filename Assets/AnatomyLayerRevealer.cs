using UnityEngine;

public class AnatomyLayerRevealer : MonoBehaviour
{
    [Header("Nested Groups")]
    public GameObject muscleMesh;    // The actual muscle mesh
    public GameObject skeletonGroup; // The child skeleton group
    public GameObject organGroup;   // The child organ group

    void Start()
    {
        Debug.Log("<color=white>[SYSTEM]</color> AnatomyLayerRevealer is awake on " + gameObject.name);
    }

    /*private void OnTriggerEnter(Collider other)
    {
        // This will print the name of ANYTHING you hit
        Debug.Log("<color=orange>[HIT DETECTED]</color> Camera touched: " + other.name);

        if (other.name == "Trigger_Skeleton")
        {
            muscleMesh.SetActive(false);
            skeletonGroup.SetActive(true);
            organGroup.SetActive(false);
            Debug.Log("<color=yellow>SUCCESS:</color> Entered Skeleton Layer");
        }

        if (other.name == "Trigger_Organs")
        {
            muscleMesh.SetActive(false);
            skeletonGroup.SetActive(false);
            organGroup.SetActive(true);
            Debug.Log("<color=red>SUCCESS:</color> Entered Organ Layer");
        }
    }*/
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=orange>[HIT]</color> " + other.name);

        if (other.name == "Trigger_Skeleton")
        {
            muscleMesh.SetActive(false);
            skeletonGroup.SetActive(true);
            // Don't turn off organs here yet!
            Debug.Log("<color=yellow>SUCCESS:</color> Entered Skeleton Layer");
        }

        if (other.name == "Trigger_Organs")
        {
            muscleMesh.SetActive(false);
            skeletonGroup.SetActive(false); // Hide bones to see organs better
            organGroup.SetActive(true);
            Debug.Log("<color=red>SUCCESS:</color> Entered Organ Layer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Restores layers if you move backward
        if (other.name == "Trigger_Organs")
        {
            skeletonGroup.SetActive(true);
            organGroup.SetActive(false);
        }
        if (other.name == "Trigger_Skeleton")
        {
            muscleMesh.SetActive(true);
            skeletonGroup.SetActive(false);
        }
    }
}