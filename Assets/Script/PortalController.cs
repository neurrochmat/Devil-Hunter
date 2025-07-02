using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public string nextSceneName; // Diisi di Inspector
    private Animator animator;
    private bool isPortalActive = false;

    [Header("Sound")]
    public AudioClip portalOpenSound; // Drag AudioClip di Inspector
    private AudioSource audioSource;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        if (animator == null) Debug.LogError("Animator tidak ditemukan!");
        if (audioSource == null) Debug.LogError("AudioSource tidak ditemukan!");
    }

    private void Start()
    {
        gameObject.SetActive(false); // Sembunyikan portal di awal
    }

    public void ActivatePortal()
    {
        gameObject.SetActive(true);
        isPortalActive = true;

        if (animator != null)
        {
            animator.SetTrigger("OpenPortal");
        }

        if (audioSource != null && portalOpenSound != null)
        {
            audioSource.PlayOneShot(portalOpenSound); // Mainkan suara portal
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPortalActive && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
