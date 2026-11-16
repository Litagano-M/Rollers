using System;
using System.Collections;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float timeElapsedInSeconds;
    public bool playerDead;
    public bool gameStarted;
    public bool gameOver;
    public GameObject player;
    public GameObject spawnManagerGameObject;
    public GameObject respawnPoint;
    public GameObject titleScreen;
    public GameObject inGameUI;
    public GameObject gameOverScreen;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI timeSurvivedText;
    public TextMeshProUGUI finalScoreText;
    public Button restartButton;
    
    public float DifficultyFactor
    {
        get
        {
            return timeElapsedInSeconds / 30;
        }
    }

    private int lives;
    private int score;
    private int respawnTime = 3;
    private int spawnInvulnerabilityTime = 3;
    private bool respawning;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameStarted = false;
        gameOver = false;
        playerController.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!gameOver)
        {
            timeElapsedInSeconds += Time.deltaTime;
            UpdateUI();
        }

        if (!gameOver && playerDead && !respawning)
        {
            StartCoroutine(RespawnSequence());
        }
    }

    public void StartGame()
    {
        titleScreen.SetActive(false);
        inGameUI.SetActive(true);

        playerDead = false;
        respawning = false;
        gameOver = false;
        gameStarted = true;

        lives = 3;
        score = 0;
        timeElapsedInSeconds = 0;
        UpdateUI();

        spawnManagerGameObject.SetActive(true);
        playerController.enabled = true;

        Debug.Log("Game Start");
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateUI();
    }

    public void AddLives(int livesToAdd)
    {
        lives += livesToAdd;
        UpdateUI();
    }

    public void DestroyPlayer()
    {
        playerController.Kill();

        playerDead = true;
        AddLives(-1);

        if (lives == 0)
        {
            GameOver();
        }
    }

    private void RespawnPlayer()
    {
        Debug.Log("Respawned player");
        playerController.Respawn(respawnPoint.transform.position);
        playerDead = false;
        respawning = false;
    }

    private IEnumerator RespawnSequence()
    {
        respawning = true;
        Debug.Log("Respawning player...");
        yield return new WaitForSeconds(respawnTime);
        RespawnPlayer();

        playerController.SetInvulnerability(true);
        yield return new WaitForSeconds(spawnInvulnerabilityTime);
        playerController.SetInvulnerability(false);
    }

    private void GameOver()
    {
        gameOver = true;
        inGameUI.SetActive(false);
        timeSurvivedText.text = $"Survived for {(int)timeElapsedInSeconds / 60:D2}:{(int)timeElapsedInSeconds % 60:D2}";
        finalScoreText.text = "Final Score: " + score;
        gameOverScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        livesText.text = "Lives: " + lives;
        timeText.text = $"Time: {(int)timeElapsedInSeconds / 60:D2}:{(int)timeElapsedInSeconds % 60:D2}";
    }
}
