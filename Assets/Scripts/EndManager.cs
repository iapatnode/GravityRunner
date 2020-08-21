using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text highScoreText;
    public Text gameScoreText;
    int currentHighScore;
    int gameScore;
    
    void Start()
    {
        //Sets high score
        if(PlayerPrefs.HasKey("HighScore"))
        {
            currentHighScore = PlayerPrefs.GetInt("HighScore");
        }
        if(PlayerPrefs.HasKey("CurrentScore"))
        {
            gameScore = PlayerPrefs.GetInt("CurrentScore");
        }

        //should I put this in start or update?
        if(gameScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", gameScore);
        }

        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore");
        gameScoreText.text = "Game Score: " + gameScore;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnMenuClick()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
