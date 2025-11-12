using UnityEngine;

public class TeleportWithStamina : MonoBehaviour
{
    [Header("Target Teleport (GameObject di Scene)")]
    public Transform teleportTarget; // tujuan teleport, drag di Inspector

    [Header("Biaya Teleport (Stamina)")]
    public float staminaCost = 2f; // stamina yang dibutuhkan untuk teleport

    [Header("Tombol untuk Teleportasi")]
    public KeyCode teleportKey = KeyCode.Space; // tombol untuk teleport

    private bool playerInZone = false;
    private PlayerMovementScene02 playerMovement; // referensi ke player untuk akses stamina

    void Start()
    {
        // cari player di scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovementScene02>();
    }

    void Update()
    {
        // jika player di dalam area dan menekan tombol teleport
        if (playerInZone && Input.GetKeyDown(teleportKey))
        {
            if (teleportTarget == null)
            {
                Debug.LogWarning("Teleport target belum diatur di Inspector!");
                return;
            }

            // pastikan player dan stamina tersedia
            if (playerMovement != null && playerMovement.stamina >= staminaCost)
            {
                // kurangi stamina dan teleport
                playerMovement.stamina -= staminaCost;
                playerMovement.transform.position = teleportTarget.position;
                Debug.Log($"Teleport berhasil ke {teleportTarget.name} (Stamina -{staminaCost})");
            }
            else
            {
                Debug.Log("? Tidak cukup stamina untuk teleport!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Player masuk area teleport. Tekan " + teleportKey + " untuk teleport.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}

