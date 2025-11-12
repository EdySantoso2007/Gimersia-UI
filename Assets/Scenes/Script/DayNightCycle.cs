using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [Header("Pengaturan Waktu")]
    public float dayDuration = 60f; // durasi satu hari (detik)
    [Range(0, 24)] public float currentTime = 12f;
    public bool isNight;

    [Header("Cahaya Global")]
    public Light2D globalLight;
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.1f, 0.1f, 0.25f);

    void Update()
    {
        // Jalankan siklus waktu
        currentTime += (24f / dayDuration) * Time.deltaTime;
        if (currentTime >= 24f) currentTime = 0f;

        // Tentukan waktu malam
        isNight = (currentTime >= 18f || currentTime < 6f);

        // Ubah warna cahaya
        if (globalLight != null)
        {
            float t = Mathf.InverseLerp(6f, 18f, currentTime);
            globalLight.color = Color.Lerp(nightColor, dayColor, t);
        }
    }
}

