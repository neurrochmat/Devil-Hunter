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

        // Inisialisasi patrol target
        patrolTarget = pointB;
    }

    private void Update()
    {
        if (isDead || playerHealth == null || playerHealth.IsDead()) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Tentukan batas patroli
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);

        bool playerInPatrolArea = player.position.x >= minX && player.position.x <= maxX;

        if (distance <= attackRange && playerInPatrolArea)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetBool("isRunning", false);

            if (cooldownTimer >= attackCooldown)
            {
                anim.SetTrigger("attack");
                cooldownTimer = 0;
            }
        }
        else if (distance <= chaseRange && playerInPatrolArea)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            bool isAtLeftBoundary = transform.position.x <= minX && direction.x < 0;
            bool isAtRightBoundary = transform.position.x >= maxX && direction.x > 0;

            if (isAtLeftBoundary || isAtRightBoundary)
            {
                // Jika musuh sudah di batas, kembali patroli
                Patrol();
            }
            else
            {
                // Jika tidak di batas, lanjutkan mengejar player
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
            // Kembali patroli jika player di luar jangkauan atau di luar area
            Patrol();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Patrol()
    {
        if (pointA == null || pointB == null) return;

        Vector2 direction = (patrolTarget.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * speed, rb.linearVelocity.y);

        if (direction.x > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction.x < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("isRunning", true);

        if (Vector2.Distance(transform.position, patrolTarget.position) < 0.2f)
        {
            patrolTarget = patrolTarget == pointA ? pointB : pointA;
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
