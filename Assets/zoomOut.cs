using UnityEngine;

public class zoomOut : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float startSize = 3f;     // zoom awal (lebih dekat)
    public float targetSize = 6f;    // zoom akhir (lebih jauh)
    public float zoomDuration = 2f;  // durasi animasi

    private Camera cam;
    private float timer;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = startSize;
    }

    void Update()
    {
        if (timer < zoomDuration)
        {
            timer += Time.deltaTime;
            float t = timer / zoomDuration;
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
        }
    }
}
