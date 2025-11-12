using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 5;
    public static int health;

    [Header("UI (Optional)")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private bool deathTriggered = false;
    private PlayerRespawn playerRespawn;

    void Awake()
    {
        // Inisialisasi health maksimal hanya jika belum ada
        if (maxHealth <= 0) maxHealth = 5;
        health = maxHealth;
        deathTriggered = false;
    }

    void Start()
    {
        // karena script ini menempel di Player, langsung ambil komponen PlayerRespawn
        playerRespawn = GetComponent<PlayerRespawn>();

        // fallback tambahan, kalau nanti kamu pindahkan ke object lain
        if (playerRespawn == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerRespawn = player.GetComponent<PlayerRespawn>();
        }

        // pastikan UI langsung update saat mulai
        UpdateHeartsImmediate();
    }

    void Update()
    {
        // update tampilan hati di setiap frame (jika UI diisi)
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }

        // deteksi mati
        if (health <= 0 && !deathTriggered)
        {
            deathTriggered = true;
            Debug.Log("Player mati — memanggil respawn...");

            if (playerRespawn != null)
                playerRespawn.Die();
            else
                SceneManager.LoadScene("Scene02");
        }
    }

    // Dipanggil dari PlayerRespawn saat respawn atau checkpoint
    public void ResetHealthToMax()
    {
        health = maxHealth;
        deathTriggered = false;
        UpdateHeartsImmediate();
        Debug.Log("Health di-reset ke penuh");
    }

    // Jika kamu hanya ingin menghapus flag kematian tanpa restore HP
    public void ClearDeathFlag()
    {
        deathTriggered = false;
    }

    private void UpdateHeartsImmediate()
    {
        if (hearts == null || hearts.Length == 0)
            return;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].sprite = (i < health) ? fullHeart : emptyHeart;
        }
    }
}

