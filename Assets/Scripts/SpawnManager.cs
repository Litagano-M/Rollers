using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;

    private float spawnBoxSize = 15f;
    private float firstSpawnDelay = 3f;
    private float startingSpawnRateInSeconds = 2f;
    [SerializeField] private float difficultyAdjustedSpawnRate;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        difficultyAdjustedSpawnRate = startingSpawnRateInSeconds;
        Invoke(nameof(SpawnEnemy), firstSpawnDelay);
    }

    private void Update()
    {
        difficultyAdjustedSpawnRate = startingSpawnRateInSeconds / (1 + Mathf.Floor(gameManager.DifficultyFactor));
    }

    private void SpawnEnemy()
    {
        if (gameManager.gameStarted && !gameManager.gameOver && !gameManager.playerDead)
        {
            Vector3 spawnPos = GenerateRandomSpawnPos();
            int enemyIndex = Random.Range(0, enemies.Length);

            Instantiate(enemies[enemyIndex], spawnPos, enemies[enemyIndex].transform.rotation);
        }

        if (gameManager.gameStarted && !gameManager.gameOver)
        {
            Invoke(nameof(SpawnEnemy), difficultyAdjustedSpawnRate);
        }
    }

    private Vector3 GenerateRandomSpawnPos()
    {
        float xPos = Random.Range(-spawnBoxSize, spawnBoxSize);
        float zPos = Random.Range(-spawnBoxSize, spawnBoxSize);
        return new Vector3(xPos, 0.5f, zPos);
    }
}
