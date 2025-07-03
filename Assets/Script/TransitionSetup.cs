using UnityEngine;
using UnityEngine.UI;

public class TransitionSetup : MonoBehaviour
{
    void Awake()
    {
        // Cek apakah ada SceneTransitionManager di scene
        if (FindObjectOfType<SceneTransitionManager>() == null)
        {
            Debug.LogWarning("SceneTransitionManager tidak ditemukan di scene. Membuat prefab SceneTransitionManager baru.");
            SetupTransitionManager();
        }
    }

    void SetupTransitionManager()
    {
        // Buat GameObject untuk TransitionManager
        GameObject transitionManager = new GameObject("SceneTransitionManager");
        
        // Tambahkan script SceneTransitionManager
        SceneTransitionManager manager = transitionManager.AddComponent<SceneTransitionManager>();
        
        // Buat Canvas untuk panel fade
        GameObject canvas = new GameObject("Canvas");
        canvas.transform.SetParent(transitionManager.transform);
        
        // Tambahkan komponen Canvas
        Canvas canvasComponent = canvas.AddComponent<Canvas>();
        canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasComponent.sortingOrder = 999; // Memastikan berada di atas semua UI
        
        // Tambahkan Canvas Scaler
        CanvasScaler scaler = canvas.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Buat Image untuk fade panel
        GameObject image = new GameObject("Image");
        image.transform.SetParent(canvas.transform);
        
        // Tambahkan komponen Image
        Image imageComponent = image.AddComponent<Image>();
        imageComponent.color = Color.black;
        imageComponent.color = new Color(0, 0, 0, 0); // Mulai transparan
        
        // Atur RectTransform untuk menutupi layar
        RectTransform rectTransform = image.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        Debug.Log("SceneTransitionManager berhasil disetup secara dinamis!");
    }
}
