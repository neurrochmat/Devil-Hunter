using UnityEngine;

public class BatFollowPlayer : MonoBehaviour
{
    public float speed = 3f; // Kecepatan gerak kelelawar
    private Transform player; // Target player
    private Vector3 targetPosition;

    void Start()
    {
        // Cari GameObject dengan tag "Player"
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Ambil posisi kelelawar sekarang
            Vector3 currentPosition = transform.position;

            // Hanya ubah arah vertikal (Y), posisi X tetap
            targetPosition = new Vector3(currentPosition.x, player.position.y, currentPosition.z);

            // Gerakkan ke arah target Y player
            transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
        }
    }
}
