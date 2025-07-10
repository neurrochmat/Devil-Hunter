using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Boss3Enemy : BossEnemy
{
    [Header("Scene Transition")]
    [Tooltip("Data konfigurasi untuk scene credit")]
    public CreditSceneData creditSceneData;
    
    // Deklarasi ulang variabel yang diperlukan
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Transform player;
    protected PlayerHealth playerHealth;
    
    private void Awake()
    {
        // Dapatkan komponen-komponen yang dibutuhkan
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (anim == null)
            Debug.LogError("Boss3Enemy membutuhkan komponen Animator!");
            
        if (rb == null)
            Debug.LogError("Boss3Enemy membutuhkan komponen Rigidbody2D!");
            
        if (GetComponent<Collider2D>() == null)
            Debug.LogError("Boss3Enemy membutuhkan komponen Collider2D!");
    }
    
    // Default values jika CreditSceneData tidak disediakan
    private string creditsSceneName = "credit";
    private float delayBeforeTransition = 2.0f;
    
    // Karena Start() di Enemy dideklarasikan sebagai private, kita tidak bisa menggunakan base.Start()
    // Sebagai gantinya, kita buat implementasi sendiri untuk Boss3Enemy
    private void Start()
    {
        // Jalankan fungsi validasi yang akan mengatur semua komponen
        ValidateSetup();
        
        // Load default values dari CreditSceneData jika tersedia
        if (creditSceneData != null)
        {
            creditsSceneName = creditSceneData.creditSceneName;
            delayBeforeTransition = creditSceneData.delayBeforeTransition;
        }
        
        // Pastikan health boss diatur ke nilai maksimum
        SendMessage("SetMaxHealth", SendMessageOptions.DontRequireReceiver);
        
        // Log informasi untuk debug
        Debug.Log($"[Boss3Enemy] {gameObject.name} diinisialisasi dengan health maksimum. Credit scene: {creditsSceneName}, delay: {delayBeforeTransition}s");
    }
    
    // Method tambahan untuk mengatur health ke nilai maksimum
    private void SetMaxHealth()
    {
        // Coba akses variabel health dengan SendMessage
        SendMessage("ResetHealth", SendMessageOptions.DontRequireReceiver);
    }
    
    protected override void Die()
    {
        // Panggil fungsi Die dari kelas induk (BossEnemy)
        base.Die();
        
        // Mulai proses transisi ke scene credits dengan delay
        StartCoroutine(TransitionToCreditsAfterDelay());
    }
    
    private IEnumerator TransitionToCreditsAfterDelay()
    {
        Debug.Log($"[Boss3Enemy] Boss mati, menunggu {delayBeforeTransition} detik sebelum transisi ke scene '{creditsSceneName}'");
        
        // Tunggu beberapa detik sebelum transisi
        yield return new WaitForSeconds(delayBeforeTransition);
        
        // Cek dan gunakan SimpleSceneTransition jika tersedia
        if (SimpleSceneTransition.Instance != null)
        {
            Debug.Log("[Boss3Enemy] Menggunakan SimpleSceneTransition untuk pindah ke scene credits");
            SimpleSceneTransition.Instance.LoadScene(creditsSceneName);
        }
        // Fallback ke SceneTransitionManager jika SimpleSceneTransition tidak ada
        else if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("[Boss3Enemy] Menggunakan SceneTransitionManager untuk pindah ke scene credits");
            SceneTransitionManager.Instance.LoadScene(creditsSceneName);
        }
        // Fallback jika tidak ada transition manager
        else
        {
            Debug.Log("[Boss3Enemy] Tidak ada transition manager ditemukan, menggunakan SceneManager langsung");
            SceneManager.LoadScene(creditsSceneName);
        }
    }
    
    // Fungsi khusus untuk debug Boss3Enemy
    public void ValidateSetup()
    {
        // Cek komponen yang diperlukan
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (anim == null || rb == null || GetComponent<Collider2D>() == null)
        {
            Debug.LogError($"[Boss3Enemy] {gameObject.name} kehilangan komponen yang diperlukan!");
            return;
        }
        
        // Tidak bisa mengakses pointA dan pointB karena private
        // Alih-alih mengecek secara langsung, kita hanya bisa memberikan peringatan umum
        Debug.Log($"[Boss3Enemy] {gameObject.name}: Pastikan Point A dan Point B dikonfigurasi di Inspector untuk patrol area!");
        
        // Cek portal controller
        if (portalController == null)
        {
            Debug.LogWarning($"[Boss3Enemy] {gameObject.name} tidak memiliki PortalController! Portal tidak akan diaktifkan saat boss mati.");
        }
        
        // Cek credit scene data
        if (creditSceneData == null)
        {
            Debug.LogWarning($"[Boss3Enemy] {gameObject.name} tidak memiliki CreditSceneData! Akan menggunakan default (scene: {creditsSceneName}, delay: {delayBeforeTransition}s)");
        }
        
        // Cari player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) 
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
            if (playerHealth == null)
                Debug.LogWarning($"[Boss3Enemy] Player tidak memiliki komponen PlayerHealth!");
        }
        else
        {
            Debug.LogError($"[Boss3Enemy] Player tidak ditemukan! Pastikan Player memiliki tag 'Player'.");
        }
        
        Debug.Log($"[Boss3Enemy] {gameObject.name} telah dikonfigurasi dengan benar!");
    }
    
    // Override fungsi OnEnable untuk melakukan validasi otomatis
    private void OnEnable() 
    {
        // Jalankan validasi saat GameObject diaktifkan
        ValidateSetup();
        
        // Tambahkan delay sebelum mencoba memulai pergerakan
        // Ini membantu jika ada komponen lain yang masih diinisialisasi
        Invoke("StartPatrol", 0.5f);
    }
    
    // Method untuk memulai pola patrol boss (mengatasi masalah variabel private di Enemy)
    public void StartPatrol()
    {
        // Metode ini bisa dipanggil dari Inspector untuk memulai patrol boss secara manual jika boss tidak bergerak
        if (anim == null || rb == null) 
        {
            ValidateSetup();
        }
        
        // Pastikan animator diatur dengan benar
        if (anim != null)
        {
            anim.SetBool("isRunning", true);
        }
        
        // Log informasi untuk debug
        Debug.Log($"[Boss3Enemy] {gameObject.name} memulai patrol.");
    }
    
    // Method untuk mengatasi masalah Boss tidak bergerak
    // Bisa ditambahkan sebagai event di animator atau dipanggil dari Inspector
    public void FixMovement()
    {
        if (rb != null)
        {
            // Coba berikan kecepatan awal ke kanan untuk memulai gerakan
            rb.linearVelocity = new Vector2(2f, rb.linearVelocity.y);
            
            if (anim != null)
            {
                anim.SetBool("isRunning", true);
            }
            
            Debug.Log($"[Boss3Enemy] {gameObject.name} dipaksa bergerak dengan FixMovement()");
        }
    }
}
