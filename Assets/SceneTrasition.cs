using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    [Header("Fade Settings")]
    public CanvasGroup fadePanel;
    public float fadeSpeed = 1f;
    public string nextSceneName = "Scene02";

    [Header("Narration UI")]
    public TextMeshProUGUI narrationText;

    [Header("Setting Menu UI")]
    public GameObject settingPanel;

    private bool pressedD = false;
    private bool pressedA = false;
    private bool pressedF = false;
    private bool pressedW = false;
    private bool finished = false;
    private bool isPaused = false;

    void Start()
    {
        StartCoroutine(StartFadeIn());
        UpdateNarration("Tekan D untuk bergerak ke kanan");
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }

    void Update()
    {
        // =====================
        // ?? Tutorial Input
        // =====================
        if (!pressedD && Input.GetKeyDown(KeyCode.D))
        {
            pressedD = true;
            UpdateNarration("Bagus! Sekarang tekan A untuk bergerak ke kiri");
        }
        else if (pressedD && !pressedA && Input.GetKeyDown(KeyCode.A))
        {
            pressedA = true;
            UpdateNarration("Hebat! Tekan F untuk mengisi stamina");
        }
        else if (pressedA && !pressedF && Input.GetKeyDown(KeyCode.F))
        {
            pressedF = true;
            UpdateNarration("Keren! Tombol baru terbuka saat menuju stage baru yaitu W untuk mengarah ke atas (Pencet W untuk Melanjutkan)");
        }
        else if (pressedF && !pressedW && Input.GetKeyDown(KeyCode.W))
        {
            pressedW = true;
            UpdateNarration("Luar biasa! Maju Untuk Melanjutkan Permainan!");
            StartCoroutine(FinishTutorial());
        }

        // =====================
        // ?? Setting Menu (ESC)
        // =====================
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingMenu();
        }
    }

    IEnumerator StartFadeIn()
    {
        yield return StartCoroutine(Fade(1, 0));
    }

    IEnumerator FinishTutorial()
    {
        if (finished) yield break;
        finished = true;

        yield return new WaitForSeconds(1000000f);
        yield return StartCoroutine(Fade(0, 1));
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator Fade(float start, float end)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * fadeSpeed;
            fadePanel.alpha = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }

    // =====================
    // ?? Setting Menu Logic
    // =====================
    void ToggleSettingMenu()
    {
        isPaused = !isPaused;

        if (settingPanel != null)
            settingPanel.SetActive(isPaused);

        Time.timeScale = isPaused ? 0f : 1f;

        if (isPaused)
        {
            narrationText.text = "Game dijeda.\nTekan ESC lagi untuk melanjutkan.";
        }
        else
        {
            // Saat kembali ke game, tampilkan narasi terakhir + teks ESC
            if (!pressedD)
                UpdateNarration("Tekan D untuk bergerak ke kanan");
            else if (!pressedA)
                UpdateNarration("Bagus! Sekarang tekan A untuk bergerak ke kiri");
            else if (!pressedF)
                UpdateNarration("Hebat! Tekan F untuk mengisi stamina");
            else if (!pressedW)
                UpdateNarration("Keren! Tekan W untuk melanjutkan ke stage berikutnya");
            else
                UpdateNarration("Lanjutkan perjalananmu!");
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (settingPanel != null)
            settingPanel.SetActive(false);

        Time.timeScale = 1f;
        UpdateNarration("Permainan dilanjutkan");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    // =====================
    // ??? Helper: Update Narasi + Tambahan ESC Info
    // =====================
    void UpdateNarration(string mainText)
    {
        narrationText.text = mainText + "\n<size=70%><color=#AAAAAA>(Tekan ESC untuk membuka Setting)</color></size>";
    }
}
