using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public AudioClip pickupSound; // Opsional
    public GameObject pickupEffect; // Opsional

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsDead())
        {
            if (playerHealth.NeedsHealing())
            {
                playerHealth.HealToFull();

                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Health already full. Potion not used.");
            }
        }
    }
}
