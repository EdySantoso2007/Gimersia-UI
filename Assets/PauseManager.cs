using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("UI Pause Menu")]
    public GameObject pauseMenuUI;
    public Button resumeButton;
    public Button restartButton;
    public Button quitButton;

    private bool isPaused = false;

    void Start()
    {
        // Nonaktifkan UI pause di awal
        pauseMenuUI.SetActive(false);

        // Tambahkan event listener tombol
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    void Update()
    {
        // ?? Cegah pause kalau GameOver sedang aktif
        GameOverManager gameOver = FindObjectOfType<GameOverManager>();
        if (gameOver != null && gameOver.gameOverUI != null && gameOver.gameOverUI.activeSelf)
            return;

        // ?? Tekan ESC atau P untuk pause/resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ?? Fungsi PAUSE
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game dijeda");
    }

    // ?? Fungsi RESUME
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game dilanjutkan");
    }

    // ?? Fungsi RESTART
    private void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Game dimulai ulang");
    }

    // ?? Fungsi QUIT (ke SampleScene sebagai Main Menu)
    private void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // ? ubah ke nama scene main menu kamu
        Debug.Log("Kembali ke SampleScene (Main Menu)");
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}