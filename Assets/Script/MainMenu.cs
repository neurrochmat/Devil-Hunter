using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("MainMenu Start() dipanggil - Memastikan SimpleSceneTransition ada");
        
        // Pastikan SimpleSceneTransition ada
        SimpleSceneTransition.CreateIfNotExists();
        
        // Verifikasi setelah pembuatan
        if (SimpleSceneTransition.Instance != null) {
            Debug.Log("SimpleSceneTransition berhasil dibuat/ditemukan");
        } else {
            Debug.LogError("KESALAHAN: SimpleSceneTransition.Instance masih null setelah CreateIfNotExists()!");
        }
        
        // Cek scene saat ini
        Debug.Log("Scene saat ini: " + SceneManager.GetActiveScene().name);
    }
    
    // Fungsi ini akan dipanggil oleh tombol "Play"
    public void PlayGame()
    {
        Debug.Log("PlayGame() dipanggil - Mencoba memulai permainan...");
        // Verifikasi keberadaan SimpleSceneTransition
        if (SimpleSceneTransition.Instance != null) {
            Debug.Log("SimpleSceneTransition ditemukan, akan digunakan untuk transisi");
        } else {
            Debug.LogWarning("SimpleSceneTransition.Instance adalah null!");
            // Mencoba membuat instance baru jika tidak ada
            SimpleSceneTransition.CreateIfNotExists();
            Debug.Log("CreateIfNotExists() dipanggil, Instance sekarang: " + (SimpleSceneTransition.Instance != null ? "valid" : "masih null"));
        }
        
        LoadSceneWithTransition("WarpedCaves"); // Ganti sesuai scene game kamu
    }

    // Fungsi ini akan dipanggil oleh tombol "Options"
    public void OpenOptions()
    {
        LoadSceneWithTransition("options"); // Ganti dengan nama scene opsi kamu
        Debug.Log("Berpindah ke scene Options...");
    }

    // Fungsi ini akan dipanggil oleh tombol "Credits"
    public void ShowCredits()
    {
        LoadSceneWithTransition("credit"); // Ganti sesuai nama scene credits
        Debug.Log("Menampilkan Credits...");
    }

    // Fungsi ini akan dipanggil oleh tombol "Quit"
    public void QuitGame()
    {
        Debug.Log("Keluar dari permainan!");
        Application.Quit();
    }

    // Fungsi tambahan untuk kembali ke Main Menu dari scene lain
    public void BackToMainMenu()
    {
        LoadSceneWithTransition("MainMenu"); // Ganti sesuai nama scene Main Menu
        Debug.Log("Kembali ke Main Menu...");
    }
    
    // Fungsi untuk memuat scene dengan transisi
    private void LoadSceneWithTransition(string sceneName)
    {
        Debug.Log("LoadSceneWithTransition dipanggil untuk scene: " + sceneName);
        
        // Verifikasi bahwa scene valid dalam build settings
        bool sceneExists = false;
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) 
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneNameFromPath.Equals(sceneName)) 
            {
                sceneExists = true;
                Debug.Log("Scene '" + sceneName + "' ditemukan di build settings dengan index: " + i);
                break;
            }
        }
        
        if (!sceneExists) 
        {
            Debug.LogError("KESALAHAN: Scene '" + sceneName + "' tidak ditemukan dalam build settings!");
            return;
        }
        
        // Coba gunakan SimpleSceneTransition terlebih dahulu
        if (SimpleSceneTransition.Instance != null)
        {
            Debug.Log("Menggunakan SimpleSceneTransition untuk memuat scene: " + sceneName);
            SimpleSceneTransition.Instance.LoadScene(sceneName);
        }
        // Fallback ke SceneTransitionManager jika SimpleSceneTransition tidak ada
        else if (SceneTransitionManager.Instance != null)
        {
            Debug.Log("Menggunakan SceneTransitionManager untuk memuat scene: " + sceneName);
            SceneTransitionManager.Instance.LoadScene(sceneName);
        }
        // Fallback tanpa transisi jika tidak ada manager
        else
        {
            Debug.LogWarning("Tidak ada transition manager yang ditemukan. Memuat scene tanpa transisi.");
            SceneManager.LoadScene(sceneName);
        }
    }
}
