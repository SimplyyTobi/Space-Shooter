using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseScreen;

    public int PlayerScore => playerScore;
    private int playerScore;

    private int highscore;

    // Start is called before the first frame update
    void Start()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);

        scoreText.text = "Score: " + playerScore;
        highscore = PlayerPrefs.GetInt("highscore", 0);
        UpdateHighscoreText();
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        UpdateScoreText();

        //Update and store highscore if current score exceeds highscore
        if (playerScore > highscore)
        {
            highscore = playerScore;
            PlayerPrefs.SetInt("highscore", highscore);
            UpdateHighscoreText();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + playerScore;
    }

    private void UpdateHighscoreText()
    {
        highscoreText.text = "Highscore: " + highscore;
    }

    public void DisplayGameOverScreen()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
    }

    public void DisplayPauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(true);
        }
    }

    public void HidePauseScreen()
    {
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
    }
}
