using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Game Over UI")]
    public GameObject gameOverUI;
    public Button restartButton;
    public Button quitButton;

    [Header("Reference ke Player Respawn")]
    public PlayerRespawn playerRespawn;

    void Start()
    {
        gameOverUI.SetActive(false);
        restartButton.onClick.AddListener(OnRestartPressed);
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitGame);
    }

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // hentikan waktu (opsional)
    }

    private void OnRestartPressed()
    {
        gameOverUI.SetActive(false);
        Time.timeScale = 1f;

        if (playerRespawn != null)
        {
            Debug.Log("Restart ditekan — respawn player...");
            playerRespawn.Die(); // fungsi asli dari script kamu
        }
        else
        {
            Debug.LogWarning("PlayerRespawn tidak ditemukan — reload scene manual.");
            SceneManager.LoadScene("Scene02");
        }
    }
    private void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // ? ubah ke nama scene main menu kamu
        Debug.Log("Kembali ke SampleScene (Main Menu)");
    }
}
