using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoSceneTeleporter : MonoBehaviour
{
    [Header("Nama Scene Tujuan")]
    public string sceneTarget = "Scene02"; // Ganti di Inspector

    [Header("Delay sebelum pindah (optional)")]
    public float delayBeforeTeleport = 0.5f; // jeda setengah detik sebelum pindah (bisa 0)

    private bool hasTeleported = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTeleported) return; // cegah duplikat trigger

        if (other.CompareTag("Player"))
        {
            hasTeleported = true;
            Debug.Log("Player menyentuh portal, berpindah ke " + sceneTarget);
            StartCoroutine(TeleportAfterDelay());
        }
    }

    private System.Collections.IEnumerator TeleportAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeTeleport);
        SceneManager.LoadScene(sceneTarget);
    }
}

