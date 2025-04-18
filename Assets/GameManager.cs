using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball Data")]
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public float ballLaunchDelay = 0.5f;

    [Header("Score UI")]
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    [Header("End Screen UI")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI winText;

    [Header("Pause Menu UI")]
    public GameObject pauseMenuPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip scoreSound;

    [HideInInspector] public string scorer = "";
    [HideInInspector] public int leftScore;
    [HideInInspector] public int rightScore;

    private const int MaxScore = 10;
    private bool _isPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        endScreenPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
    }

    private void Start()
    {
        SpawnBallWithDelay();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;
        
        if (_isPaused) ResumeGame();
        else PauseGame();
    }

    public void AddPointToLeft()
    {
        scorer = "left";
        audioSource.PlayOneShot(scoreSound);
        leftScore++;
        leftScoreText.text = leftScore.ToString();
        StartCoroutine(AnimateScoreText(leftScoreText));
        CheckForWin();
    }

    public void AddPointToRight()
    {
        scorer = "right";
        audioSource.PlayOneShot(scoreSound);
        rightScore++;
        rightScoreText.text = rightScore.ToString();
        StartCoroutine(AnimateScoreText(rightScoreText));
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (leftScore >= MaxScore)
            ShowEndScreen("Player 1 Wins!");
        else if (rightScore >= MaxScore)
            ShowEndScreen("Player 2 Wins!");
        else
            SpawnBallWithDelay();
    }

    private void ShowEndScreen(string message)
    {
        Time.timeScale = 0f;
        winText.text = message;
        endScreenPanel.SetActive(true);
    }

    private void SpawnBallWithDelay()
    {
        StartCoroutine(SpawnBallCoroutine());
    }

    private IEnumerator SpawnBallCoroutine()
    {
        yield return new WaitForSeconds(ballLaunchDelay);
        Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
    }
    
    private static IEnumerator AnimateScoreText(TextMeshProUGUI text, float popFactor = 1.5f, float popDuration = 0.2f)
    {
        var originalSize = text.fontSize;
        var targetSize   = originalSize * popFactor;
        var halfDuration = popDuration * 0.5f;
        var t = 0f;

        // Grow
        while (t < halfDuration)
        {
            text.fontSize = Mathf.Lerp(originalSize, targetSize, t / halfDuration);
            t += Time.deltaTime;
            yield return null;
        }

        // Shrink
        t = 0f;
        while (t < halfDuration)
        {
            text.fontSize = Mathf.Lerp(targetSize, originalSize, t / halfDuration);
            t += Time.deltaTime;
            yield return null;
        }

        text.fontSize = originalSize;
    }

    // BUTTON HANDLERS

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuScene");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        _isPaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        _isPaused = false;
    }
}
