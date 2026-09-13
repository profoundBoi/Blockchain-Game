using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI Panels")]
    public GameObject gameEndPanel;
    public GameObject pausePanel;

    private bool isPaused = false;
    private bool gameEnded = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        if (gameEndPanel != null) gameEndPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    // Called when coin is collected
    public void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;
        StartCoroutine(ShowEndScreen());
    }

    IEnumerator ShowEndScreen()
    {
        yield return new WaitForSeconds(2f);
        if (gameEndPanel != null)
            gameEndPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Pause toggle
    public void TogglePause()
    {
        isPaused = !isPaused;
        if (pausePanel != null)
            pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    // Exit to login screen
    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }

    // Restart current scene
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}