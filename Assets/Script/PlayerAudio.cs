using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip runSound;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioClip[] attackSounds; // 3 tipe suara attack

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            Debug.LogError("AudioSource tidak ditemukan di Player!");
    }

    public void PlayRun()
    {
        if (runSound != null && (!audioSource.isPlaying || audioSource.clip != runSound))
        {
            audioSource.clip = runSound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void StopRun()
    {
        if (audioSource.isPlaying && audioSource.clip == runSound)
        {
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    public void PlayJump()
    {
        if (jumpSound != null)
            audioSource.PlayOneShot(jumpSound);
    }

    public void PlayDamage()
    {
        if (damageSound != null)
            audioSource.PlayOneShot(damageSound);
    }

    public void PlayDeath()
    {
        if (deathSound != null)
            audioSource.PlayOneShot(deathSound);
    }

    public void PlayAttack()
    {
        if (attackSounds.Length > 0)
        {
            int index = Random.Range(0, attackSounds.Length);
            if (attackSounds[index] != null)
                audioSource.PlayOneShot(attackSounds[index]);
        }
    }
}
