using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float moved;   // 1 when D is pressed
    public float movea;   // -1 when A is pressed

    public Image stamina_bar;
    public float stamina, max_stamina;
    public float runcost; // stamina per second while moving

    [Tooltip("Stamina per second gained while holding F (only charges when not moving)")]
    public float chargeRate = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (max_stamina <= 0f) max_stamina = 1f;
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);
    }

    void Update()
    {
        // Read inputs
        moved = Input.GetKey(KeyCode.D) ? 1f : 0f;
        movea = Input.GetKey(KeyCode.A) ? -1f : 0f;
        float inputX = moved + movea;
        bool isCharging = Input.GetKey(KeyCode.F);

        float dt = Time.deltaTime;

        // Hold F to charge only when not moving
        if (isCharging && Mathf.Approximately(inputX, 0f))
        {
            stamina += chargeRate * dt;
        }
        // Moving (A or D) drains stamina
        else if (!Mathf.Approximately(inputX, 0f) && stamina > 0f)
        {
            stamina -= runcost * dt;
        }

        // Clamp stamina and update UI
        stamina = Mathf.Clamp(stamina, 0f, max_stamina);
        if (stamina_bar != null)
            stamina_bar.fillAmount = Mathf.Clamp01(stamina / max_stamina);
    }

    void FixedUpdate()
    {
        // Combine inputs, block movement when stamina is zero
        float inputX = moved + movea;
        float effectiveMove = (Mathf.Abs(inputX) > 0f && stamina > 0f) ? inputX : 0f;

        // Apply horizontal velocity (preserve vertical velocity)
        rb.linearVelocity = new Vector2(effectiveMove * speed, rb.linearVelocity.y);
    }
}