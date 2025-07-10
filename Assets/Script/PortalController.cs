using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

        // Pastikan SimpleSceneTransition tersedia ketika portal diaktifkan
        SimpleSceneTransition.CreateIfNotExists();
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
            // Gunakan SimpleSceneTransition terlebih dahulu jika tersedia
            if (SimpleSceneTransition.Instance != null)
            {
                SimpleSceneTransition.Instance.LoadScene(nextSceneName);
            }
            // Fallback ke SceneTransitionManager jika SimpleSceneTransition tidak ada
            else if (SceneTransitionManager.Instance != null)
            {
                SceneTransitionManager.Instance.LoadScene(nextSceneName);
            }
            // Fallback jika tidak ada transition manager
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
    }
}
