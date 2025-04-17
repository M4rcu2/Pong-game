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

    [HideInInspector] public string scorer = "";
    [HideInInspector] public int leftScore;
    [HideInInspector] public int rightScore;

    private void Awake()
    {
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
        SpawnBallWithDelay();
    }

    public void AddPointToRight()
    {
        scorer = "right";
        rightScore++;
        rightScoreText.text = rightScore.ToString();
        SpawnBallWithDelay();
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
}