using UnityEngine;
using UnityEngine.UI;

public class MusikManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public Slider volumeSlider;
    public Toggle muteToggle;

    void Start()
    {
        // Load volume dari PlayerPrefs (kalau pernah disimpan)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        bool isMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;

        bgmSource.volume = isMuted ? 0 : savedVolume;
        volumeSlider.value = savedVolume;
        muteToggle.isOn = isMuted;

        // Tambahkan listener saat slider berubah
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteToggle.onValueChanged.AddListener(SetMute);
    }

    public void SetVolume(float value)
    {
        if (!muteToggle.isOn)
            bgmSource.volume = value;

        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    public void SetMute(bool isMuted)
    {
        if (isMuted)
            bgmSource.volume = 0;
        else
            bgmSource.volume = volumeSlider.value;

        PlayerPrefs.SetInt("MusicMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
}
