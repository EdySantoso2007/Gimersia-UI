using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementScene02 : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    // legacy public fields kept for inspector/debug
    public float moved;   // 1 when D active (press window)
    public float movea;   // -1 when A active (press window)
    public float movew;   // 1 when W active (press window)

    public Image stamina_bar;
    public float stamina = 5f;
    public float max_stamina = 5f;
    public float runcost = 1f;
    public float chargeRate = 20f; // Stamina per second gained while holding F (only charges when not moving)

    [Header("Press-to-move settings")]
    [Tooltip("How long a single key press produces movement (seconds). To keep moving, player must press repeatedly).")]
    public float pressMoveTime = 0.15f;
    [Tooltip("Stamina cost per key press")]
    public float staminaCostPerPress = 1f;

    // disable WAS area trigger
    private bool disableWAS = false;

    // internal timers for press windows
    private float rightTimer;
    private float leftTimer;
    private float upTimer;

    // === Tambahan untuk sistem siang–malam (efek vampir) ===
    [Header("Efek Vampir (Siang–Malam)")]
    public float nightSpeedMultiplier = 1.5f;     // kecepatan ekstra saat malam
    public float dayStaminaDrainRate = 1f;        // kecepatan stamina turun saat siang
    private DayNightCycle dayNightCycle;          // referensi ke sistem siang–malam
    private float baseSpeed;                      // menyimpan kecepatan asli agar tidak berubah permanen

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (max_stamina <= 0f) max_stamina = 1f;
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);

        baseSpeed = speed;
        dayNightCycle = FindObjectOfType<DayNightCycle>(); // cari otomatis DayNightCycle di scene
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // === Efek vampir berdasarkan waktu ===
        if (dayNightCycle != null)
        {
            if (dayNightCycle.isNight)
            {
                // 🌙 malam hari → vampir makin cepat
                speed = baseSpeed * nightSpeedMultiplier;
            }
            else
            {
                // ☀️ siang hari → stamina berkurang
                speed = baseSpeed;
                stamina -= dayStaminaDrainRate * dt;
            }
        }

        // Input per tekan (bukan hold)
        if (!disableWAS)
        {
            if (Input.GetKeyDown(KeyCode.D)) TryConsumeAndStartRight();
            if (Input.GetKeyDown(KeyCode.A)) TryConsumeAndStartLeft();
            if (Input.GetKeyDown(KeyCode.W)) TryConsumeAndStartUp();
        }

        // Kurangi waktu aktif tiap arah
        rightTimer = Mathf.Max(0f, rightTimer - dt);
        leftTimer = Mathf.Max(0f, leftTimer - dt);
        upTimer = Mathf.Max(0f, upTimer - dt);

        moved = rightTimer > 0f ? 1f : 0f;
        movea = leftTimer > 0f ? -1f : 0f;
        movew = upTimer > 0f ? 1f : 0f;

        bool isCharging = Input.GetKey(KeyCode.F);
        bool isMoving = rightTimer > 0f || leftTimer > 0f || upTimer > 0f;

        // isi stamina saat diam dan menekan F
        if (isCharging && !isMoving)
            stamina += chargeRate * dt;

        // batas stamina
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);
        if (stamina_bar != null)
            stamina_bar.fillAmount = Mathf.Clamp01(stamina / max_stamina);
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float inputX = moved + movea;
        float inputY = movew;

        rb.linearVelocity = new Vector2(inputX * speed, inputY * speed);
    }

    private void TryConsumeAndStartRight()
    {
        if (stamina >= staminaCostPerPress)
        {
            stamina -= staminaCostPerPress;
            rightTimer = pressMoveTime;
            leftTimer = 0f;
        }
    }

    private void TryConsumeAndStartLeft()
    {
        if (stamina >= staminaCostPerPress)
        {
            stamina -= staminaCostPerPress;
            leftTimer = pressMoveTime;
            rightTimer = 0f;
        }
    }

    private void TryConsumeAndStartUp()
    {
        if (stamina >= staminaCostPerPress)
        {
            stamina -= staminaCostPerPress;
            upTimer = pressMoveTime;
        }
    }

    // === Sistem checkpoint & refill stamina ===
    public void RefillStamina()
    {
        stamina = max_stamina;
        if (stamina_bar != null)
            stamina_bar.fillAmount = Mathf.Clamp01(stamina / max_stamina);

        Debug.Log("Stamina diisi penuh (dari checkpoint atau respawn)!");
    }

    // === Disable area W, A, S ===
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("DisableZone"))
        {
            disableWAS = true;
            Debug.Log("W, A, S dinonaktifkan — hanya D yang aktif.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("DisableZone"))
        {
            disableWAS = false;
            Debug.Log("W, A, S diaktifkan kembali.");
        }
    }
}
