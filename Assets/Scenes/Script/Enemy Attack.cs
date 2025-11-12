using UnityEngine;

public class enemy_attack : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject prefab;
    public float minTime = 2f;
    public float maxTime = 4f;

    [Header("Drop behaviour")]
    [Tooltip("Downwards speed (units/sec)")]
    public float dropSpeed = 5f;

    [Tooltip("If true the spawned object gets a Rigidbody2D (added if missing) and velocity is applied")]
    public bool useRigidbody = true;

    [Tooltip("If true all Collider2D on the spawned object will be set to isTrigger so it will pass through tile colliders")]
    public bool makeColliderTrigger = true;

    [Header("Follow behaviour (spawned objects)")]
    [Tooltip("If true spawned objects will try to follow the player")]
    public bool spawnFollowsPlayer = true;
    public bool followOnX = true;
    public bool followOnY = false;
    public float followStrength = 5f;
    public float maxFollowSpeed = 8f;

    [Header("Layer-based ignore (optional)")]
    public string projectileLayerName = "";
    public string tileLayerName = "";
    public bool ignoreTileLayerCollision = false;

    [Tooltip("Auto destroy spawned object after seconds (0 = never)")]
    public float autoDestroyAfter = 0f;

    private void OnEnable() => Invoke(nameof(Spawn), minTime);
    private void OnDisable() => CancelInvoke();

    private void Spawn()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Spawner: prefab is null.");
            return;
        }

        GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);

        // Optionally set projectile layer
        if (!string.IsNullOrWhiteSpace(projectileLayerName))
        {
            int projLayer = LayerMask.NameToLayer(projectileLayerName);
            if (projLayer != -1) SetLayerRecursively(go, projLayer);
        }

        // Collider setup
        var cols = go.GetComponentsInChildren<Collider2D>();
        if (cols == null || cols.Length == 0)
        {
            var added = go.AddComponent<CircleCollider2D>();
            added.isTrigger = makeColliderTrigger;
        }
        else if (makeColliderTrigger)
        {
            foreach (var c in cols) c.isTrigger = true;
        }

        // Rigidbody setup
        Rigidbody2D rb = null;
        if (useRigidbody)
        {
            rb = go.GetComponent<Rigidbody2D>() ?? go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.linearVelocity = Vector2.down * Mathf.Abs(dropSpeed);
        }
        else
        {
            var mover = go.GetComponent<SimpleFall>() ?? go.AddComponent<SimpleFall>();
            mover.speed = Mathf.Abs(dropSpeed);
        }

        // Follower
        if (spawnFollowsPlayer)
        {
            var f = go.GetComponent<SpawnedFollower>() ?? go.AddComponent<SpawnedFollower>();
            f.target = null;
            f.followX = followOnX;
            f.followY = followOnY;
            f.followStrength = followStrength;
            f.maxFollowSpeed = maxFollowSpeed;
            f.dropSpeed = dropSpeed;
            f.useRigidbody = (rb != null);
        }

        // Destroy behaviour
        var destroyer = go.GetComponent<DestroyOnPlayerHit_Local>() ?? go.AddComponent<DestroyOnPlayerHit_Local>();
        destroyer.playerTag = "Player";
        destroyer.blockTag = "Block";
        destroyer.damage = 1;

        if (ignoreTileLayerCollision && !string.IsNullOrWhiteSpace(projectileLayerName) && !string.IsNullOrWhiteSpace(tileLayerName))
        {
            int projLayer = LayerMask.NameToLayer(projectileLayerName);
            int tLayer = LayerMask.NameToLayer(tileLayerName);
            if (projLayer != -1 && tLayer != -1)
                Physics2D.IgnoreLayerCollision(projLayer, tLayer, true);
        }

        if (autoDestroyAfter > 0f)
            Destroy(go, autoDestroyAfter);

        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }

    void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }

    class SimpleFall : MonoBehaviour
    {
        public float speed = 5f;
        void Update() => transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}

// -------------------------------------------
// Follower
// -------------------------------------------
public class SpawnedFollower : MonoBehaviour
{
    [HideInInspector] public Transform target;
    public bool followX = true;
    public bool followY = false;
    public float followStrength = 5f;
    public float maxFollowSpeed = 8f;
    public float dropSpeed = 5f;
    public bool useRigidbody = true;

    Rigidbody2D rb;

    void Start()
    {
        if (target == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        rb = GetComponent<Rigidbody2D>();
        if (useRigidbody && rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector2 pos = transform.position;
        Vector2 tpos = target.position;

        float vx = 0f;
        float vy = 0f;

        if (followX)
        {
            float dx = tpos.x - pos.x;
            vx = dx * followStrength;
            vx = Mathf.Clamp(vx, -Mathf.Abs(maxFollowSpeed), Mathf.Abs(maxFollowSpeed));
        }

        if (followY)
        {
            float dy = tpos.y - pos.y;
            vy = dy * followStrength;
        }
        else
        {
            vy = -Mathf.Abs(dropSpeed);
        }

        if (rb != null && useRigidbody)
        {
            rb.linearVelocity = new Vector2(vx, vy);
        }
        else
        {
            Vector2 delta = new Vector2(vx, vy) * Time.fixedDeltaTime;
            transform.position = (Vector2)transform.position + delta;
        }
    }
}

// -------------------------------------------
// Destroyer (Player + Block conditional)
// -------------------------------------------
public class DestroyOnPlayerHit_Local : MonoBehaviour
{
    public string playerTag = "Player";
    public string blockTag = "Block";
    public int damage = 0;
    public float minPlayerDistanceToBlock = 2f; // ?? Jarak minimal agar blok mulai aktif

    bool hasHit = false;
    Transform player;

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag(playerTag);
        if (p != null) player = p.transform;
    }

    void HandleHit(Collider2D other)
    {
        if (hasHit || other == null) return;

        // Hancur saat mengenai Player
        if (other.CompareTag(playerTag))
        {
            hasHit = true;
            if (damage > 0)
                HealthManager.health = Mathf.Max(0, HealthManager.health - damage);
            Destroy(gameObject);
            return;
        }

        // ?? Hancur saat mengenai Block — tetapi hanya jika player sudah cukup dekat
        if (other.CompareTag(blockTag))
        {
            if (player != null)
            {
                float dist = Vector2.Distance(player.position, other.transform.position);
                if (dist <= minPlayerDistanceToBlock)
                {
                    hasHit = true;
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) => HandleHit(other);
    void OnCollisionEnter2D(Collision2D collision) => HandleHit(collision.collider);
}
