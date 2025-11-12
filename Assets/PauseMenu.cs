using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject pauseMenuUI;
    public GameObject soundSettingsUI; // Panel untuk pengaturan sound (jika ada)
    public Slider soundSlider; // Slider volume (optional)

    private bool isPaused = false;

    void Update()
    {
        // Tekan ESC untuk toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Hentikan waktu
        isPaused = true;
    }
    

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        soundSettingsUI.SetActive(false);
        Time.timeScale = 1f; // Jalankan waktu lagi
        isPaused = false;
    }

    public void GoHome()
    {
        Time.timeScale = 1f; // Pastikan waktu normal sebelum load scene
        SceneManager.LoadScene("MainMenu"); // Ganti dengan nama scene menu utamamu
    }

    public void OpenSoundSettings()
    {
        soundSettingsUI.SetActive(true);
    }

    public void CloseSoundSettings()
    {
        soundSettingsUI.SetActive(false);
    }

    // Optional: atur volume dengan slider
    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }


}
