using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Transform door;             // Reference to the door
    public float sinkSpeed = 2f;       // Speed at which the plate sinks
    public float sinkDepth = 0.3f;     // How much the plate sinks
    public float doorSwingAngle = 90f; // How much the door swings open (in degrees)
    public float doorOpenSpeed = 2f;   // Speed of the door opening

    private Vector3 originalPosition;   // Original position of the plate
    private Quaternion doorOriginalRot; // Original rotation of the door
    private bool isTriggered = false;

    void Start()
    {
        originalPosition = transform.position;
        doorOriginalRot = door.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(SinkAndSwingDoor());
        }
    }

    private System.Collections.IEnumerator SinkAndSwingDoor()
    {
        // ✅ Sink the pressure plate
        float sinkTime = 0f;
        Vector3 targetPosition = originalPosition - new Vector3(0, sinkDepth, 0);

        while (sinkTime < 1f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, sinkTime);
            sinkTime += Time.deltaTime * sinkSpeed;
            yield return null;
        }
        transform.position = targetPosition;

        // ✅ Swing the door open
        float swingTime = 0f;
        Quaternion targetRotation = Quaternion.Euler(0, doorSwingAngle, 0) * doorOriginalRot;

        while (swingTime < 1f)
        {
            door.rotation = Quaternion.Slerp(doorOriginalRot, targetRotation, swingTime);
            swingTime += Time.deltaTime * doorOpenSpeed;
            yield return null;
        }

        door.rotation = targetRotation;  // Ensure it snaps to final rotation
    }
}
