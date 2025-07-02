using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Fungsi ini akan dipanggil oleh tombol "Play"
    public void PlayGame()
    {
        SceneManager.LoadScene("NamaSceneGameAnda"); // Ganti sesuai scene game kamu
        Debug.Log("Memulai permainan...");
    }

    // Fungsi ini akan dipanggil oleh tombol "Options"
    public void OpenOptions()
    {
        SceneManager.LoadScene("options"); // Ganti dengan nama scene opsi kamu
        Debug.Log("Berpindah ke scene Options...");
    }


    // Fungsi ini akan dipanggil oleh tombol "Credits"
    public void ShowCredits()
    {
        SceneManager.LoadScene("credit"); // Ganti sesuai nama scene credits
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
        SceneManager.LoadScene("MainMenu"); // Ganti sesuai nama scene Main Menu
        Debug.Log("Kembali ke Main Menu...");
    }
}
