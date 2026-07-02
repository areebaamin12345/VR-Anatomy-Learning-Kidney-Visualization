using UnityEngine;
using System.Collections.Generic;

public class KidneyReset : MonoBehaviour
{
    // This will hold all the kidney parts
    private struct PartData
    {
        public Transform partTransform;
        public Vector3 pos;
        public Quaternion rot;
    }

    private List<PartData> savedParts = new List<PartData>();

    void Awake()
    {
        // Find all children meshes and save their starting spots
        foreach (Transform child in transform)
        {
            PartData data;
            data.partTransform = child;
            data.pos = child.localPosition;
            data.rot = child.localRotation;
            savedParts.Add(data);
        }
    }

    public void ReassembleKidney()
    {
        Debug.Log("Button was clicked! Reassembling now..."); // This will show in the Console
        foreach (PartData data in savedParts)
        {
            data.partTransform.localPosition = data.pos;
            data.partTransform.localRotation = data.rot;

            // If they have physics, stop them from moving/floating away
            if (data.partTransform.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}