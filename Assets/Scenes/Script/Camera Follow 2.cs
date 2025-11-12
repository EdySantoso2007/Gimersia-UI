using UnityEngine;

public class CameraStepFollow : MonoBehaviour
{
    public Transform player;           // referensi ke player
    public float stepHeight = 5f;      // jarak vertikal antar "lantai" (sesuaikan)
    public float smoothSpeed = 3f;     // kecepatan transisi kamera (0 = instan)
    private float targetY;             // posisi Y kamera yang diinginkan

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        // set posisi awal kamera
        targetY = transform.position.y;
    }

    void Update()
    {
        if (player == null) return;

        // jika player melewati batas atas kamera (naik cukup jauh)
        if (player.position.y > targetY + stepHeight / 2f)
        {
            targetY += stepHeight;
        }
        // jika player turun cukup jauh (misal jatuh ke area bawah)
        else if (player.position.y < targetY - stepHeight / 2f)
        {
            targetY -= stepHeight;
        }

        // transisi halus ke posisi target
       Vector3 targetPos = new Vector3(transform.position.x, targetY + 1f, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);
    }
}

