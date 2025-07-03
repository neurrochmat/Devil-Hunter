using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        // Pastikan SimpleSceneTransition ada
        SimpleSceneTransition.CreateIfNotExists();
    }
    
    // Fungsi ini akan dipanggil oleh tombol "Play"
    public void PlayGame()
    {
        LoadSceneWithTransition("WarpedCaves"); // Ganti sesuai scene game kamu
        Debug.Log("Memulai permainan...");
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
        // Coba gunakan SimpleSceneTransition terlebih dahulu
        if (SimpleSceneTransition.Instance != null)
        {
            SimpleSceneTransition.Instance.LoadScene(sceneName);
        }
        // Fallback ke SceneTransitionManager jika SimpleSceneTransition tidak ada
        else if (SceneTransitionManager.Instance != null)
        {
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
