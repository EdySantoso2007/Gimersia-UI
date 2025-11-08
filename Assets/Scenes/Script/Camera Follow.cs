using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 2.0f;
    public Transform target;

    void Start()
    {
        // Auto-assign the target if not set in the Inspector (expects the player to have tag "Player")
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    // Use LateUpdate so the camera follows after all movement/physics have been applied
    void LateUpdate()
    {
        if (target == null) return;

        // Preserve camera Z position and smoothly interpolate to the target XY
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
    }
}
