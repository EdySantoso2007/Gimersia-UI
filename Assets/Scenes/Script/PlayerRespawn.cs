using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 respawnPoint;
    private bool hasCheckpoint = false;

    private PlayerMovementScene02 playerMovement;
    private HealthManager healthManager;

    void Start()
    {
        // Simpan posisi awal player sebagai respawn default
        respawnPoint = transform.position;

        // Ambil komponen yang ada di Player sendiri
        playerMovement = GetComponent<PlayerMovementScene02>();
        healthManager = GetComponent<HealthManager>(); // ambil langsung dari Player

        // Fallback tambahan kalau sewaktu-waktu kamu pindahkan HealthManager ke objek lain
        if (healthManager == null)
            healthManager = FindObjectOfType<HealthManager>();
    }

    public void Die()
    {
        Debug.Log("Player mati!");

        // Isi stamina penuh sebelum respawn
        if (playerMovement != null)
            playerMovement.RefillStamina();

        if (hasCheckpoint)
        {
            // Reset HP agar tidak stuck 0
            if (healthManager != null)
                healthManager.ResetHealthToMax();

            // Kembalikan ke checkpoint
            transform.position = respawnPoint;
            Debug.Log("Respawn di checkpoint!");
        }
        else
        {
            // Kalau belum ada checkpoint, reload scene default
            SceneManager.LoadScene("Scene02");
            Debug.Log("Respawn di Scene02 (belum ada checkpoint)");
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
        hasCheckpoint = true;

        // Refill stamina
        if (playerMovement != null)
            playerMovement.RefillStamina();

        // Reset HP ke penuh saat buat checkpoint
        if (healthManager != null)
            healthManager.ResetHealthToMax();

        Debug.Log("Checkpoint disimpan dan stamina diisi penuh!");
    }

    // ?? Tambahkan fungsi ini di bawah SetCheckpoint()
    public void SaveCheckpointOnly(Vector3 point)
    {
        respawnPoint = point;
        hasCheckpoint = true;

        // Isi stamina penuh ketika buat checkpoint, tapi jangan reset HP
        if (playerMovement != null)
            playerMovement.RefillStamina();

        Debug.Log("Checkpoint disimpan (HP tidak direset).");
    }
}


