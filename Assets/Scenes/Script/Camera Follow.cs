using UnityEngine;

public class CameraFollowBounce : MonoBehaviour
{
    [Header("Target (Player)")]
    public Transform target;

    [Header("Camera Offset (Posisi di atas player)")]
    public Vector3 offset = new Vector3(0, 0, -10f);

    [Header("Bounce Settings")]
    [Tooltip("Semakin kecil nilai smoothTime, semakin cepat kamera mengikuti")]
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Posisi target + offset (kamera berada di atas player)
        Vector3 targetPosition = target.position + offset;

        // Gunakan SmoothDamp agar kamera bergerak lembut seperti bounce
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
