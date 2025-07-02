using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public AudioClip pickupSound;        // opsional
    public GameObject pickupEffect;      // assign prefab efek via Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null && !playerHealth.IsDead())
        {
            playerHealth.HealToFull();

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
