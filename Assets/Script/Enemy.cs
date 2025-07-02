using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public GameObject healthBarPrefab;
    public float speed = 2f;
    public float chaseRange = 5f;
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    [Header("Patrol Area")]
    public Transform pointA;
    public Transform pointB;
    private Transform patrolTarget;

    private int currentHealth;
    private EnemyHealthBar healthBar;
    private Transform player;
    private PlayerHealth playerHealth;
    private Animator anim;
    private Rigidbody2D rb;
    private bool isDead = false;
    private float cooldownTimer;
    private float patrolWaitTimer = 0f;
    private float maxPatrolTime = 5f; // Maksimum waktu sebelum mengganti target jika tidak bisa mencapai target

    private void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
        playerHealth = playerObj.GetComponent<PlayerHealth>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        GameObject barObj = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        healthBar = barObj.GetComponent<EnemyHealthBar>();
        healthBar.Initialize(transform);
        healthBar.SetHealth(currentHealth, maxHealth);

        // Inisialisasi patrol target berdasarkan posisi musuh
        if (pointA != null && pointB != null)
        {
            float distToA = Vector2.Distance(transform.position, pointA.position);
            float distToB = Vector2.Distance(transform.position, pointB.position);
            patrolTarget = distToA > distToB ? pointA : pointB;
            Debug.Log($"{gameObject.name}: Patrol target diinisialisasi ke {patrolTarget.name}");
        }
    }

    private void Update()
    {
        if (isDead || playerHealth == null || playerHealth.IsDead()) return;

        // Add null check for patrol points to prevent errors
        if (pointA == null || pointB == null)
        {
            // If patrol points are not set, the enemy will not move.
            // This prevents the NullReferenceException seen in the console.
            // Please ensure PointA and PointB are assigned in the Inspector for the Enemy.
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRunning", false);
            return;
        }

        // Tentukan batas patroli
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        bool playerInPatrolArea = player.position.x >= minX && player.position.x <= maxX;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange && playerInPatrolArea)
        {
            // Player in range and in patrol area, chase player
            Vector2 direction = (player.position - transform.position).normalized;

            // Periksa apakah musuh berada di batas area patroli
            bool isAtLeftBoundary = transform.position.x <= minX && direction.x < 0;
            bool isAtRightBoundary = transform.position.x >= maxX && direction.x > 0;

            if (isAtLeftBoundary || isAtRightBoundary)
            {
                // Jika musuh di batas, kembali patroli
                Patrol();
            }
            else if (distanceToPlayer <= attackRange)
            {
                // Attack player
                rb.linearVelocity = Vector2.zero;
                anim.SetBool("isRunning", false);
                if (cooldownTimer >= attackCooldown)
                {
                    anim.SetTrigger("attack");
                    cooldownTimer = 0;
                }
            }
            else
            {
                // Chase player
                rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

                if (direction.x > 0.01f)
                    transform.localScale = new Vector3(1, 1, 1);
                else if (direction.x < -0.01f)
                    transform.localScale = new Vector3(-1, 1, 1);

                anim.SetBool("isRunning", true);
            }
        }
        else
        {
            // Player out of range or out of patrol area, patrol
            Patrol();
        }

        // Switch patrol target when reached
        float distToTarget = Vector2.Distance(transform.position, patrolTarget.position);
        
        // Periksa jarak atau waktu untuk pergantian target patroli
        if (distToTarget < 0.5f) // Lebih toleran pada jarak
        {
            // Target tercapai, ganti target
            Transform oldTarget = patrolTarget;
            patrolTarget = patrolTarget == pointA ? pointB : pointA;
            Debug.Log($"{gameObject.name}: Target patroli diubah dari {oldTarget.name} ke {patrolTarget.name} karena sudah mencapai target");
            patrolWaitTimer = 0f; // Reset timer
        }
        else
        {
            // Jika terjebak terlalu lama mencoba mencapai target, paksa ganti target
            patrolWaitTimer += Time.deltaTime;
            if (patrolWaitTimer > maxPatrolTime)
            {
                Transform oldTarget = patrolTarget;
                patrolTarget = patrolTarget == pointA ? pointB : pointA;
                Debug.Log($"{gameObject.name}: Target patroli diubah dari {oldTarget.name} ke {patrolTarget.name} karena timeout");
                patrolWaitTimer = 0f; // Reset timer
            }
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null) return;

        // Tambahkan pemeriksaan untuk memastikan patrolTarget tidak null
        if (patrolTarget == null)
        {
            Debug.LogWarning($"{gameObject.name}: patrolTarget is null in Patrol method!");
            patrolTarget = pointA; // Default ke pointA jika null
            return;
        }

        // Periksa apakah musuh terjebak (tidak bergerak)
        Vector2 currentPos = transform.position;
        float distanceToTarget = Vector2.Distance(currentPos, patrolTarget.position);
        
        // Hitung vector arah ke target
        Vector2 direction = (patrolTarget.position - transform.position).normalized;
        
        // Tentukan batas X dari area patroli
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);
        
        // Pastikan musuh tidak keluar dari area patroli
        bool isNearLeftBoundary = transform.position.x <= minX + 0.1f;
        bool isNearRightBoundary = transform.position.x >= maxX - 0.1f;
        
        if ((isNearLeftBoundary && patrolTarget == pointA) || 
            (isNearRightBoundary && patrolTarget == pointB))
        {
            // Musuh di batas dan mengarah keluar, paksa ganti target
            patrolTarget = patrolTarget == pointA ? pointB : pointA;
            Debug.Log($"{gameObject.name}: Forcing target switch at boundary to {patrolTarget.name}");
            patrolWaitTimer = 0f;
            return;
        }
        
        // Set kecepatan berdasarkan arah target
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        // Rotate enemy based on movement direction
        if (direction.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("isRunning", true);
        
        // Debug patrol movement
        if (Time.frameCount % 60 == 0) // Log setiap 60 frame (approx. 1 detik pada 60fps)
        {
            Debug.Log($"{gameObject.name}: Patrolling towards {patrolTarget.name}. " +
                     $"Distance: {distanceToTarget}, Position: {currentPos.x}, " +
                     $"Target: {patrolTarget.position.x}");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        anim.SetTrigger("hurt");

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        StartCoroutine(Shake(0.4f, 0.08f));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        anim.SetTrigger("dead");
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 1.5f);

        if (healthBar != null)
            Destroy(healthBar.gameObject);
    }

    public void DamagePlayer()
    {
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
    // ... (di bawah fungsi DestroyEnemy())

    private void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            // Tentukan batas X
            float minX = Mathf.Min(pointA.position.x, pointB.position.x);
            float maxX = Mathf.Max(pointA.position.x, pointB.position.x);

            // Gambar garis batas di Scene View
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(minX, transform.position.y - 5, 0), new Vector3(minX, transform.position.y + 5, 0));
            Gizmos.DrawLine(new Vector3(maxX, transform.position.y - 5, 0), new Vector3(maxX, transform.position.y + 5, 0));
        }
    }
} // Pastikan ini kurung kurawal penutup terakhir untuk class
