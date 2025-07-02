using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator anim;
    private Vector3 spawnPosition;
    private bool isDead = false;

    public PlayerHealthBar healthBar; // Assign via Inspector

    private Rigidbody2D rb;
    private Collider2D col;
    private PlayerMovement movement;

    private int defaultLayer;

    private void Start()
    {
        spawnPosition = transform.position;
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        movement = GetComponent<PlayerMovement>();

        defaultLayer = gameObject.layer;

        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth, maxHealth);

        Debug.Log("Player took damage, current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player has died");

        anim.SetTrigger("isDead");

        // Disable movement
        if (movement != null) movement.enabled = false;

        // Tetap aktifkan collider untuk jatuh di tanah
        if (col != null) col.enabled = true;

        // Aktifkan physics agar jatuh ke tanah
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        // Ubah layer agar tidak diserang musuh
        gameObject.layer = LayerMask.NameToLayer("DeadPlayer"); // Buat layer baru di Project Settings

        // Tampilkan Game Over panel dengan delay
        StartCoroutine(ShowGameOverDelayed(1.2f));
    }

    private System.Collections.IEnumerator ShowGameOverDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.ShowGameOver(); // GameOver muncul fade-in di GameManager
    }

    public void Respawn()
    {
        transform.position = spawnPosition;

        currentHealth = maxHealth;
        isDead = false;
        healthBar.SetHealth(currentHealth, maxHealth);

        // Aktifkan ulang movement
        if (movement != null) movement.enabled = true;

        // Aktifkan collider
        if (col != null) col.enabled = true;

        // Aktifkan physics kembali
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        // Kembalikan layer ke default
        gameObject.layer = defaultLayer;

        // Reset animasi
        anim.Rebind();
        anim.Update(0f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
