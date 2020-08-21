using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LLManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Text currentScoreText;
    public Text scoreBreakdownText;
    public Text levelText;
    int timeBonus;
    int livesBonus;
    int points;

    void Start()
    {
        //gets name of level just completed
        if(PlayerPrefs.HasKey("CurrentLevel"))
        {
            levelText.text = PlayerPrefs.GetString("CurrentLevel") + " Complete";
        }
        //gets current total score of player 
        if(PlayerPrefs.HasKey("CurrentScore"))
        {
            currentScoreText.text = "Score: " + PlayerPrefs.GetInt("CurrentScore");
        }
        //gets lives bonus from level
        if (PlayerPrefs.HasKey("LifeBonus"))
        {
            livesBonus = PlayerPrefs.GetInt("LifeBonus");
        }
        //gets time bonus from level
        if(PlayerPrefs.HasKey("TimeBonus"))
        {
            timeBonus = PlayerPrefs.GetInt("TimeBonus");
        }
        //gets points earned in level
        if(PlayerPrefs.HasKey("LevelScore"))
        {
            points = PlayerPrefs.GetInt("LevelScore");
        }
        scoreBreakdownText.text = "Points earned: " + points + "\nLives Bonus: " + livesBonus + "\nTime Bonus: " + timeBonus;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Loads scene based on past level completed
    public void OnLevelClick()
    {
        if(PlayerPrefs.GetString("CurrentLevel").Equals("Level 1"))
        {
            SceneManager.LoadScene("Level 2");
        }
        else if(PlayerPrefs.GetString("CurrentLevel").Equals("Level 2"))
        {
            SceneManager.LoadScene("Level 3");
        }
        else if(PlayerPrefs.GetString("CurrentLevel").Equals("Level 3"))
        {
            SceneManager.LoadScene("Level 4");
        }
        else
        { 
            SceneManager.LoadScene("BossFight");
        }
    }
}
