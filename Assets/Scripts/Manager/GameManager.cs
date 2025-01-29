using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private UIManager uiManager;
    private PlayerHealth playerHealth;
    private SpawnManager spawnManager;
    private Asteroid asteroid;

    [SerializeField] private bool isPaused = false;

    [Header("Difficulty Settings")]
    [SerializeField] private int difficultyThreshold = 100;
    [SerializeField] private int difficultyLevel = 0;
    [SerializeField] private float enemySpawnRateDecreasePerLevel = 0.65f;
    [SerializeField] private float asteroidSpeedIncrease = 2.5f;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager (script) not found in GameManager!");
        }
        
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += HandlePlayerDeath;
        }
        else
        {
            Debug.LogError("PlayerHealth (script) not found in GameManager!");
        }

        spawnManager = FindObjectOfType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager (script) not found in GameManager!");
        }

        asteroid = FindObjectOfType<Asteroid>();
        if (asteroid != null)
        {
            Debug.LogError("Asteroid (script) not found in GameManager!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        CheckForDifficultyIncrease();
    }
    private void HandlePlayerDeath()
    {
        uiManager.DisplayGameOverScreen();
        Time.timeScale = 0;
    }

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
        //Checks if the player score has crossed the current difficultyThreshold
        int playerScore = uiManager.PlayerScore;
        while (playerScore >= difficultyThreshold)
        {
            difficultyThreshold += 100;
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty()
    {
        spawnManager.IncreaseEnemySpawnRate(enemySpawnRateDecreasePerLevel);
        //asteroid.IncreaseMoveSpeed(asteroidSpeedIncrease);
            //Needs fix: Error when this script is called and there is no asteroid currently instantiated!

        difficultyLevel++;
        Debug.Log("Difficulty increase!");
    }

    #endregion
}
