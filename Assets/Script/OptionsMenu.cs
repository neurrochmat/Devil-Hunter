using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer audioMixer;           // Drag AudioMixer di Inspector
    public Slider volumeSlider;             // Drag UI Slider di Inspector

    [Header("Panel Options")]
    public GameObject optionsPanel;         // Drag Panel Options (untuk Show/Hide)

    private const string VOLUME_KEY = "MasterVolume";

    void Start()
    {
        // Ambil volume tersimpan (default = 0 dB)
        float savedVolume = PlayerPrefs.GetFloat(VOLUME_KEY, 0f);
        audioMixer.SetFloat(VOLUME_KEY, savedVolume);
        volumeSlider.value = savedVolume;
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat(VOLUME_KEY, volume);
        PlayerPrefs.SetFloat(VOLUME_KEY, volume);
        Debug.Log("Volume diatur ke: " + volume + " dB");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
        Debug.Log("Options menu dibuka.");
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        Debug.Log("Options menu ditutup.");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
