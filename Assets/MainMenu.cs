using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ==============================
    // 1️⃣ MULAI / PLAY GAME
    // ==============================
    public void PlayGame()
    {
        Debug.Log("Memulai game...");
        // Pindah ke scene berikutnya berdasarkan urutan Build Settings
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // Atau bisa pakai nama scene langsung:
        // SceneManager.LoadScene("NamaSceneGame");
    }

    // ==============================
    // 2️⃣ KELUAR DARI GAME
    // ==============================
    public void ExitGame()
    {
        Debug.Log("Keluar dari game...");
        Application.Quit();

#if UNITY_EDITOR
        // Supaya tombol juga berfungsi di Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // ==============================
    // 3️⃣ PINDAH KE SCENE TERTENTU
    // ==============================
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log("Memuat scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // ==============================
    // 4️⃣ PINDAH KE SCENE BERIKUTNYA
    // ==============================
    public void NextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Pindah ke scene berikutnya...");
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            Debug.LogWarning("Tidak ada scene berikutnya di Build Settings!");
        }
    }
}
