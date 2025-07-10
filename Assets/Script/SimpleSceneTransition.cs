using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleSceneTransition : MonoBehaviour
{
    public static SimpleSceneTransition Instance;
    
    [SerializeField] private float fadeTime = 1f;
    private Image fadeImage;
    private Canvas canvas;
    
    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void SetupUI()
    {
        // Setup Canvas
        canvas = GetComponentInChildren<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas tidak ditemukan pada SceneTransition");
            return;
        }
        
        // Setup Image
        fadeImage = GetComponentInChildren<Image>();
        if (fadeImage == null)
        {
            Debug.LogError("Image tidak ditemukan pada SceneTransition");
            return;
        }
        
        // Setup awal
        fadeImage.color = new Color(0, 0, 0, 0); // Mulai transparan
        
        Debug.Log("SimpleSceneTransition berhasil diinisialisasi");
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }
    
    private IEnumerator FadeAndLoad(string sceneName)
    {
        Debug.Log("Memulai transisi ke scene: " + sceneName);
        
        // Fade Out (dari transparan ke hitam)
        float elapsedTime = 0;
        Color startColor = fadeImage.color;
        startColor.a = 0;
        Color endColor = startColor;
        endColor.a = 1;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeTime);
            yield return null;
        }
        
        fadeImage.color = endColor; // Pastikan benar-benar hitam
        
        // Load scene baru
        SceneManager.LoadScene(sceneName);
        yield return null; // Tunggu satu frame
        
        // Fade In (dari hitam ke transparan)
        elapsedTime = 0;
        
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(endColor, startColor, elapsedTime / fadeTime);
            yield return null;
        }
        
        fadeImage.color = startColor; // Pastikan benar-benar transparan
        
        Debug.Log("Transisi selesai");
    }
    
    public static void CreateIfNotExists()
    {
        if (Instance != null) return;
        
        // Cek apakah prefab sudah ada di scene
        SimpleSceneTransition existing = FindAnyObjectByType<SimpleSceneTransition>();
        if (existing != null)
        {
            Instance = existing;
            return;
        }
        
        // Buat transisi baru
        GameObject transitionObj = new GameObject("SimpleSceneTransition");
        
        // Buat canvas
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.transform.SetParent(transitionObj.transform);
        
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Buat image
        GameObject imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(canvasObj.transform);
        
        Image image = imageObj.AddComponent<Image>();
        image.color = Color.black;
        image.color = new Color(0, 0, 0, 0); // Start transparent
        
        // Set ukuran image untuk menutupi seluruh layar
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Tambahkan script
        SimpleSceneTransition transition = transitionObj.AddComponent<SimpleSceneTransition>();
        // Tetapkan instance
        Instance = transition;
        
        // Pastikan tidak dihancurkan saat pindah scene
        DontDestroyOnLoad(transitionObj);
        
        Debug.Log("SimpleSceneTransition dibuat secara otomatis");
    }
}
