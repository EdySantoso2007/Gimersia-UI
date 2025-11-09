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
    public float runcost = 1f; // unused in press-based movement but kept
    public float chargeRate = 20f; // Stamina per second gained while holding F (only charges when not moving)

    // press-based movement settings
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (max_stamina <= 0f) max_stamina = 1f;
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // Input: only start movement on GetKeyDown (single press).
        // If zone disables WAS, those inputs are ignored.
        if (!disableWAS)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                TryConsumeAndStartRight();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                TryConsumeAndStartLeft();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                TryConsumeAndStartUp();
            }
        }

        // Decrease timers
        rightTimer = Mathf.Max(0f, rightTimer - dt);
        leftTimer = Mathf.Max(0f, leftTimer - dt);
        upTimer = Mathf.Max(0f, upTimer - dt);

        // Update the public flags (for compatibility)
        moved = rightTimer > 0f ? 1f : 0f;
        movea = leftTimer > 0f ? -1f : 0f;
        movew = upTimer > 0f ? 1f : 0f;

        float inputX = moved + movea;
        float inputY = movew;
        bool isCharging = Input.GetKey(KeyCode.F);

        // Consider "moving" if any timer is active
        bool isMoving = rightTimer > 0f || leftTimer > 0f || upTimer > 0f;

        // Hold F to charge only when not moving
        if (isCharging && !isMoving)
        {
            stamina += chargeRate * dt;
        }

        // (No continuous drain while moving — we cost per press)
        // Clamp stamina and update UI
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);
        if (stamina_bar != null)
            stamina_bar.fillAmount = Mathf.Clamp01(stamina / max_stamina);
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        float inputX = moved + movea;
        float inputY = movew;

        // movement only active during the press window; no continuous holding
        float effectiveX = inputX;
        float effectiveY = inputY;

        rb.linearVelocity = new Vector2(effectiveX * speed, effectiveY * speed);
    }

    private void TryConsumeAndStartRight()
    {
        if (stamina >= staminaCostPerPress)
        {
            stamina -= staminaCostPerPress;
            rightTimer = pressMoveTime;
            // cancel opposite if needed
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

    // Trigger area to disable WAS input (keeps D active)
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