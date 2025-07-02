using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public GameObject gameOverPanel; // Assign via Inspector
    public string sceneToReload; // Nama scene yang akan di-reload (set di Inspector)

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (gameOverPanel != null)
        {
            canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            gameOverPanel.SetActive(false); // Panel awalnya disembunyikan
        }
    }

    public void ShowGameOver()
    {
        StartCoroutine(ShowGameOverWithFade());
    }

    private IEnumerator ShowGameOverWithFade()
    {
        yield return new WaitForSeconds(1f); // Delay sebelum muncul

        gameOverPanel.SetActive(true);

        if (canvasGroup == null)
        {
            canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                Debug.LogWarning("CanvasGroup tidak ditemukan pada gameOverPanel!");
                yield break;
            }
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void RestartGame()
    {
        if (!string.IsNullOrEmpty(sceneToReload))
        {
            SceneManager.LoadScene(sceneToReload); // Reload scene yang dipilih
        }
        else
        {
            Debug.LogWarning("Scene untuk respawn belum diset di GameManager.");
        }
    }
}
