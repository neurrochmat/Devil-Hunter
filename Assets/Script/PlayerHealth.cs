using UnityEngine;
using System.Collections;

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
    private PlayerAudio playerAudio;
    private SpriteRenderer spriteRenderer;

    private int defaultLayer;

    private void Start()
    {
        spawnPosition = transform.position;
        currentHealth = maxHealth;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        movement = GetComponent<PlayerMovement>();
        playerAudio = GetComponent<PlayerAudio>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        defaultLayer = gameObject.layer;

        healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth, maxHealth);
        playerAudio?.PlayDamage();

        StartCoroutine(FlashRed());

        Debug.Log("Player took damage, current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);
            spriteRenderer.color = Color.white;
        }
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("Player has died");

        anim.SetTrigger("isDead");

        if (movement != null) movement.enabled = false;
        if (col != null) col.enabled = true;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = false;

        gameObject.layer = LayerMask.NameToLayer("DeadPlayer");

        StartCoroutine(ShowGameOverDelayed(1.2f));
        playerAudio?.PlayDeath();
    }

    private IEnumerator ShowGameOverDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.ShowGameOver();
    }

    public void Respawn()
    {
        transform.position = spawnPosition;

        currentHealth = maxHealth;
        isDead = false;
        healthBar.SetHealth(currentHealth, maxHealth);

        if (movement != null) movement.enabled = true;
        if (col != null) col.enabled = true;

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
        }

        var playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null) playerAttack.enabled = true;

        gameObject.layer = defaultLayer;

        anim.Rebind();
        anim.Update(0f);
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void HealToFull()
    {
        if (isDead) return;

        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth, maxHealth);
        Debug.Log("Player healed to full health");
    }

    public bool NeedsHealing()
    {
        return currentHealth < maxHealth;
    }
}
