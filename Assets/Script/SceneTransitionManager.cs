using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [SerializeField] private float fadeTime = 1f;
    private Animator animator;
    private string sceneToLoad;
    
    // Referensi untuk mengelola fade
    private Image fadePanel;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Dapatkan komponen yang dibutuhkan
            animator = GetComponent<Animator>();
            
            // Cari atau tambahkan CanvasGroup
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
                Debug.Log("CanvasGroup ditambahkan ke SceneTransitionManager");
            }
            
            // Konfigurasi CanvasGroup agar tidak menghalangi interaksi UI
            // Saat alpha = 0, UI lain masih bisa diklik
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
            
            // Cari Image untuk fade panel
            fadePanel = GetComponentInChildren<Image>();
            if (fadePanel == null)
            {
                Debug.LogError("Image untuk panel fade tidak ditemukan! Pastikan ada Image di bawah SceneTransitionManager.");
            }
            else 
            {
                // Set warna image ke hitam dengan alpha 0
                Color panelColor = Color.black;
                panelColor.a = 0;
                fadePanel.color = panelColor;
            }
            
            // Set nilai awal CanvasGroup
            canvasGroup.alpha = 0;
            
            // Verifikasi bahwa animator ditemukan
            if (animator == null)
            {
                Debug.LogWarning("Animator tidak ditemukan pada SceneTransitionManager! Akan menggunakan fade manual.");
            }
            else
            {
                Debug.Log("SceneTransitionManager berhasil diinisialisasi dengan Animator");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Pastikan panel mulai transparan
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            // Pastikan interaksi UI tidak diblokir di awal
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = 0;
            fadePanel.color = panelColor;
        }
        
        // Tambahkan listener untuk memastikan pengaturan yang tepat setelah scene baru dimuat
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDestroy()
    {
        // Bersihkan listener saat objek dihancurkan
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset pengaturan UI setelah scene dimuat
        if (canvasGroup != null)
        {
            // Pastikan interaksi UI tidak diblokir setelah scene dimuat
            canvasGroup.blocksRaycasts = false;
            Debug.Log("Scene dimuat: " + scene.name + " - UI interactions diaktifkan");
        }
    }

    public void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        Debug.Log("Memulai transisi scene ke: " + sceneToLoad);

        // Set canvas group untuk memblokir raycast selama transisi
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }
        
        // SELALU LAKUKAN FADE MANUAL terlepas dari animator untuk memastikan transisi berjalan
        // Fade out - dari transparan ke hitam
        Debug.Log("Melakukan fade out manual");
        float fadeOutTime = 0;
        
        // Pastikan alpha mulai dari 0
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
        
        // Animasikan alpha dari 0 ke 1
        while (fadeOutTime < fadeTime)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, fadeOutTime / fadeTime);
            }
            
            // Juga ubah warna Image jika ada
            if (fadePanel != null)
            {
                Color panelColor = fadePanel.color;
                panelColor.a = Mathf.Lerp(0, 1, fadeOutTime / fadeTime);
                fadePanel.color = panelColor;
            }
            
            fadeOutTime += Time.deltaTime;
            yield return null;
        }
        
        // Pastikan benar-benar hitam
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
        
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = 1;
            fadePanel.color = panelColor;
        }
        
        // Trigger animator jika tersedia (tetapi tetap gunakan fade manual di atas)
        if (animator != null)
        {
            animator.SetTrigger("FadeOut");
            Debug.Log("Trigger FadeOut animator dijalankan");
        }

        // Tunggu sedikit untuk efek fade out
        yield return new WaitForSeconds(0.2f);
        
        // Load the new scene
        SceneManager.LoadScene(sceneToLoad);
        Debug.Log("Scene " + sceneToLoad + " dimuat");
        
        // Tunggu sedikit untuk memastikan scene sudah dimuat
        yield return new WaitForSeconds(0.1f);
        
        // Trigger animator jika tersedia (tetapi tetap gunakan fade manual di bawah)
        if (animator != null)
        {
            animator.SetTrigger("FadeIn");
            Debug.Log("Trigger FadeIn animator dijalankan");
        }
        
        // SELALU LAKUKAN FADE MANUAL untuk memastikan transisi berjalan
        Debug.Log("Melakukan fade in manual");
        
        // Pastikan alpha mulai dari 1 (hitam)
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
        }
        
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = 1;
            fadePanel.color = panelColor;
        }
        
        // Fade in - dari hitam ke transparan
        float fadeInTime = 0;
        while (fadeInTime < fadeTime)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, fadeInTime / fadeTime);
            }
            
            // Juga ubah warna Image jika ada
            if (fadePanel != null)
            {
                Color panelColor = fadePanel.color;
                panelColor.a = Mathf.Lerp(1, 0, fadeInTime / fadeTime);
                fadePanel.color = panelColor;
            }
            
            fadeInTime += Time.deltaTime;
            yield return null;
        }
        
        // Pastikan benar-benar transparan
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false; // Nonaktifkan blocksRaycasts agar UI dapat diklik kembali
        }
        
        if (fadePanel != null)
        {
            Color panelColor = fadePanel.color;
            panelColor.a = 0;
            fadePanel.color = panelColor;
        }
        
        Debug.Log("Transisi selesai, UI interactions diaktifkan kembali");
    }
}
