using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    [Tooltip("Amount of damage applied to the player per second while in trap")]
    public int damagePerSecond = 10;
    
    [Tooltip("Apply damage instantly on first contact")]
    public bool applyInitialDamage = true;
    
    [Tooltip("Initial damage amount applied immediately on contact")]
    public int initialDamage = 5;
    
    private float damageTimer = 0f;
    private bool playerInTrap = false;
    private PlayerHealth playerHealth = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrap = true;
            playerHealth = collision.GetComponent<PlayerHealth>();
            
            // Apply initial damage on first contact if enabled
            if (applyInitialDamage && playerHealth != null)
            {
                playerHealth.TakeDamage(initialDamage);
                Debug.Log("Player entered trap! Initial damage: " + initialDamage);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInTrap = false;
            playerHealth = null;
            Debug.Log("Player exited trap");
        }
    }

    private void Update()
    {
        if (playerInTrap && playerHealth != null)
        {
            // Apply damage over time
            damageTimer += Time.deltaTime;
            
            // Apply damage every second
            if (damageTimer >= 1.0f)
            {
                playerHealth.TakeDamage(damagePerSecond);
                damageTimer = 0f;
                Debug.Log("Player taking trap damage: " + damagePerSecond);
            }
        }
    }
}
