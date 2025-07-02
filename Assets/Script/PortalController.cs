using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public string nextSceneName; // Diisi di Inspector
    private Animator animator;
    private bool isPortalActive = false;

    private void Awake()
    {
        // Inisialisasi animator di Awake agar tetap terbaca walau object awalnya nonaktif
        animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("Animator tidak ditemukan pada GameObject Portal!");
        }
    }

    private void Start()
    {
        gameObject.SetActive(false); // Sembunyikan portal di awal permainan
    }

    public void ActivatePortal()
    {
        gameObject.SetActive(true);  // Aktifkan portal
        isPortalActive = true;

        if (animator != null)
        {
            animator.SetTrigger("OpenPortal"); // Mainkan animasi portal muncul
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPortalActive && other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName); // Pindah ke scene berikutnya
        }
    }
}
