using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OrganScatterManager : MonoBehaviour
{
    // === PUBLIC VARIABLES ===
    public List<Transform> organsToMove;
    public List<Transform> scatteredPositions;
    public float animationDuration = 2.0f;

    // === PRIVATE VARIABLES ===
    private bool isAnimating = false;
    private bool hasScattered = false;
    private GameObject instructionPopup;

    void Start()
    {
        instructionPopup = GameObject.Find("InstructionPopUp");
    }

    public void ScatterOnce()
    {
        if (isAnimating || hasScattered) return;

        if (instructionPopup != null)
        {
            instructionPopup.SetActive(false);
        }

        // 1. Capture the unique STARTING positions and rotations
        List<Vector3> startPositions = new List<Vector3>();
        List<Quaternion> fixedRotations = new List<Quaternion>();

        foreach (var organ in organsToMove)
        {
            startPositions.Add(organ.position);

            // We store the rotation that makes them look "upright" now
            fixedRotations.Add(organ.rotation);

            // Disable physics so organs don't fall or bounce during movement
            if (organ.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
        }

        // 2. Start the animation
        StartCoroutine(AnimateScatter(startPositions, fixedRotations, scatteredPositions, animationDuration));
        hasScattered = true;
    }

    private IEnumerator AnimateScatter(List<Vector3> startPositions, List<Quaternion> fixedRotations, List<Transform> targetTransforms, float duration)
    {
        isAnimating = true;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            t = t * t * (3f - 2f * t); // Smooth easing

            for (int i = 0; i < organsToMove.Count; i++)
            {
                // MOVE POSITION: Glide to the target marker
                organsToMove[i].position = Vector3.Lerp(startPositions[i], targetTransforms[i].position, t);

                // LOCK ROTATION: Keep the original orientation (the -180 or -89.98 values)
                organsToMove[i].rotation = fixedRotations[i];
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Final Snap to ensure perfect position alignment
        for (int i = 0; i < organsToMove.Count; i++)
        {
            organsToMove[i].position = targetTransforms[i].position;
            organsToMove[i].rotation = fixedRotations[i];

            // OPTIONAL: If you want to grab them in VR later, 
            // you might want to turn isKinematic back to false here.
        }

        isAnimating = false;
    }
}