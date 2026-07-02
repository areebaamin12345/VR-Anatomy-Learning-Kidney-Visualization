using UnityEngine;
using System.Collections.Generic;

public class ModelResetter : MonoBehaviour
{
    // A way to store the original position and rotation of every child mesh
    private struct Pose
    {
        public Vector3 position;
        public Quaternion rotation;
    }

    private Dictionary<Transform, Pose> originalPoses = new Dictionary<Transform, Pose>();

    void Awake()
    {
        // Save the positions of every piece of the kidney at the very start
        foreach (Transform child in GetComponentsInChildren<Transform>())
        {
            originalPoses[child] = new Pose
            {
                position = child.localPosition,
                rotation = child.localRotation
            };
        }
    }

    public void ResetToOriginal()
    {
        foreach (var entry in originalPoses)
        {
            Transform t = entry.Key;
            t.localPosition = entry.Value.position;
            t.localRotation = entry.Value.rotation;

            // NEW: Stop the parts from moving/spinning after they reset
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero; // Stops movement
                rb.angularVelocity = Vector3.zero; // Stops spinning
            }
        }
        Debug.Log("<color=cyan>[RESET]</color> Meshes and physics reset.");
    }
}