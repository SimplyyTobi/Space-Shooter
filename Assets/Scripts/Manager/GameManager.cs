using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Normal,
        BossFight
    }
    private GameState currentState;

    private UIManager uiManager;
    private PlayerHealth playerHealth;
    private SpawnManager spawnManager;

    [SerializeField] private bool isPaused = false;

    [Header("Difficulty Settings")]
    [SerializeField] private int difficultyThreshold = 100;
    [SerializeField] private int difficultyLevel = 0;
    [SerializeField] private float enemySpawnRateDecreasePerLevel = 0.65f;
    [SerializeField] private int bossThreshold = 1000;
    private bool isBossActive;

    private void Awake()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager (script) not found on GameManager!");
        }

        spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager (script) not found on GameManager!");
        }

        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += HandlePlayerDeath;
        }
        else
        {
            Debug.LogError("PlayerHealth (script) not found on GameManager!");
        }
    }

    private void Start()
    {
        isBossActive = false;
        SetGameState(GameState.Normal);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        CheckForDifficultyIncrease();
        CheckForBoss();
    }

    #region Game State Management
    private void SetGameState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Normal:
                spawnManager.StartSpawning();
                break;

            case GameState.BossFight:
                spawnManager.StopSpawning();
                isBossActive = true;

                IBoss boss = spawnManager.SpawnBoss();
                if (boss != null)
                {
                    boss.OnBossDefeated += HandleBossDefeat;
                }
                break;
        }
    }

    #endregion

    #region Event Handling
    private void HandlePlayerDeath()
    {
        uiManager.DisplayGameOverScreen();
        Time.timeScale = 0;
    }

    private void HandleBossDefeat()
    {
        isBossActive = false;
        SetGameState(GameState.Normal);
    }
    #endregion

    #region Menu Options
    public void RestartGame()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToStartScreen()
    {
        SceneManager.LoadScene(0);  //Start Screen scene
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            uiManager.DisplayPauseScreen();
        }
        else
        {
            Time.timeScale = 1;
            uiManager.HidePauseScreen();
        }
    }

    #endregion

    #region Difficulty
    private void CheckForDifficultyIncrease()
    {
        int playerScore = uiManager.PlayerScore;

        //Checks if the player score has crossed the current difficultyThreshold
        while (playerScore >= difficultyThreshold)
        {
            IncreaseDifficulty();
            difficultyThreshold += 100;
        }
    }

    private void IncreaseDifficulty()
    {
        spawnManager.IncreaseEnemySpawnRate(enemySpawnRateDecreasePerLevel);

        difficultyLevel++;
        Debug.Log("Difficulty increase!");
    }

    private void CheckForBoss()
    {
        int playerScore = uiManager.PlayerScore;

        if (playerScore >= bossThreshold && !isBossActive)
        {
            bossThreshold += 100;
            SetGameState(GameState.BossFight);
        }
    }

    #endregion
}
