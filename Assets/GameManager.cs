using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Ball data")]
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public float ballLaunchDelay = 0.5f;
    
    [Header("Score UI")]
    public TextMeshProUGUI leftScoreText;
    public TextMeshProUGUI rightScoreText;

    [Header("End Screen UI")]
    public GameObject endScreenPanel;
    public TextMeshProUGUI winText;

    [HideInInspector] public string scorer = "";
    [HideInInspector] public int leftScore;
    [HideInInspector] public int rightScore;

    private const int MaxScore = 10;

    private void Awake()
    {
        endScreenPanel.SetActive(false);
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }
    
    private void Start()
    {
        SpawnBallWithDelay();
    }

    public void AddPointToLeft()
    {
        scorer = "left";
        leftScore++;
        leftScoreText.text = leftScore.ToString();
        CheckForWin();
    }

    public void AddPointToRight()
    {
        scorer = "right";
        rightScore++;
        rightScoreText.text = rightScore.ToString();
        CheckForWin();
    }

    private void CheckForWin()
    {
        if (leftScore >= MaxScore)
        {
            ShowEndScreen("Player 1 Wins!");
        }
        else if (rightScore >= MaxScore)
        {
            ShowEndScreen("Player 2 Wins!");
        }
        else
        {
            SpawnBallWithDelay();
        }
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

    // BUTTON CALLS
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
