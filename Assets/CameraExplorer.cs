using UnityEngine;
using System.Collections;

public class CameraExplorer : MonoBehaviour
{
    public Transform targetInside;
    public float transitionSpeed = 2.0f;
    private bool isMoving = false;
    private Vector3 adjustedTargetPos;

    public void StartExploring()
    {
        if (targetInside != null)
        {
            // STOP any existing movement before starting a new one
            isMoving = false;
            StopAllCoroutines();
            StartCoroutine(WaitBeforeMoving());
        }
    }

    IEnumerator WaitBeforeMoving()
    {
        yield return new WaitForSeconds(5.0f);

        if (targetInside != null)
        {
            adjustedTargetPos = targetInside.position;
            adjustedTargetPos.y = transform.position.y;

            // 1. Calculate the direction to the model
            Vector3 directionToModel = (targetInside.position - transform.position).normalized;
            directionToModel.y = 0; // Keep it horizontal

            // 2. Snap the XR Origin to face the model perfectly
            if (directionToModel != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(directionToModel);
            }

            isMoving = true;
        }
    }

    void Update()
    {
        if (isMoving && targetInside != null)
        {
            // FIX: Move strictly in a world-space straight line
            transform.position = Vector3.MoveTowards(transform.position, adjustedTargetPos, transitionSpeed * Time.deltaTime);

            // STOP logic
            if (Vector3.Distance(transform.position, adjustedTargetPos) < 0.001f)
            {
                isMoving = false;
                transform.position = adjustedTargetPos;
                Debug.Log("<color=magenta>[FINISH]</color> Arrived straight.");
            }
        }
    }
}