using UnityEngine;

public class BossMusicTrigger : MonoBehaviour
{
    public float triggerRange = 10f;
    public AudioSource bossMusicSource;
    private AudioSource mainCameraAudio;
    private Transform player;
    private bool musicSwitched = false;

    private Enemy enemyBase; // untuk cek isDead jika perlu

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        mainCameraAudio = Camera.main.GetComponent<AudioSource>();
        enemyBase = GetComponent<Enemy>();

        if (bossMusicSource != null)
        {
            bossMusicSource.loop = true;
            bossMusicSource.playOnAwake = false;
            bossMusicSource.Stop();
        }
    }

    void Update()
    {
        if (player == null || mainCameraAudio == null || bossMusicSource == null)
            return;

        if (enemyBase != null && enemyBaseIsDead())
        {
            if (bossMusicSource.isPlaying)
                bossMusicSource.Stop();

            if (!mainCameraAudio.isPlaying)
                mainCameraAudio.Play();

            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= triggerRange && !musicSwitched)
        {
            mainCameraAudio.Stop();
            bossMusicSource.Play();
            musicSwitched = true;
        }
        else if (distance > triggerRange && musicSwitched)
        {
            bossMusicSource.Stop();
            mainCameraAudio.Play();
            musicSwitched = false;
        }
    }

    private bool enemyBaseIsDead()
    {
        // Karena isDead di Enemy bersifat protected,
        // kita gunakan refleksi (jika kamu tidak ingin ubah Enemy.cs)
        var field = typeof(Enemy).GetField("isDead", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return field != null && (bool)field.GetValue(enemyBase);
    }
}
