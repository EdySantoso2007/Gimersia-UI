using UnityEngine;

public class CheckpointInteractor : MonoBehaviour
{
    private PlayerRespawnManager playerRespawnManager;
    private bool playerInRange = false;

    void Start()
    {
        // Cari PlayerRespawnManager di scene (asumsi hanya ada satu)
        playerRespawnManager = FindObjectOfType<PlayerRespawnManager>();
        if (playerRespawnManager == null)
        {
            Debug.LogError("PlayerRespawnManager tidak ditemukan di scene!");
        }
    }

    void Update()
    {
        // Cek jika pemain di dalam area dan tombol 'F' ditekan
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            // Perbarui checkpoint pemain ke posisi RespawnPoint anak
            playerRespawnManager.UpdateCheckpoint(transform.GetChild(0).position);
            // Opsional: Nonaktifkan collider checkpoint ini setelah diaktifkan
            // GetComponent<Collider2D>().enabled = false; 
        }
    }

    // Dipanggil saat objek lain masuk ke dalam trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Pemain masuk area checkpoint. Tekan 'F' untuk mengaktifkan.");
        }
    }

    // Dipanggil saat objek lain keluar dari trigger collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Pemain keluar area checkpoint.");
        }
    }
}