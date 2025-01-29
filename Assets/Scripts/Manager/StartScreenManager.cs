using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        if (Time.timeScale == 0)    //is set to 0 if you go back to start screen due to pausing -> needs to reset to 1
        {
            Time.timeScale = 1;     
        }
        SceneManager.LoadScene(1);  //Main Game scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
