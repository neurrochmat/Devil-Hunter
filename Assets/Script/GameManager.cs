using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    public GameObject gameOverPanel; // Assign via Inspector
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
        // Sembunyikan panel Game Over dan reset fade
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Respawn player
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        if (player != null)
        {
            player.Respawn();
        }
    }
}
