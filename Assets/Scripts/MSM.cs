using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MSM : MonoBehaviour
{

    //Makes main scene manager a singleton
    private static MSM _instance = null;

    public static MSM Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    int levelScore;
    int currentScore;
    int livesBonus;
    int timeBonus;
    private int startLives = 3;
    private int lives;
    public Text ScoreText;
    public Text LivesText;
    public Text TimeText;
    public PlayerScript player;
    int timePassed;
    int levelTime;
    float startTime;
    string sceneName;

    AudioSource audioSource;
    public AudioClip coin;
    public AudioClip death;
    public AudioClip gunshot;

    // Start is called before the first frame update
    void Start()
    {
        levelScore = 0;
        lives = startLives;
        startTime = 0.0f;
        timePassed = 0;
        sceneName = SceneManager.GetActiveScene().name; //gets name of current scene
        SetTimeLimit();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        ScoreText.text = "Score: " + levelScore;
        LivesText.text = "Lives: " + lives;

        //Decrements timer
        UpdateTimer();

        //Player dies when there are no more lives
        if (lives <= 0) {
            OnDeath();
        }

        //can quit the game at any time
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    //Updates the timer text
    private void UpdateTimer()
    {
        timePassed = levelTime - (int)(Time.timeSinceLevelLoad - startTime);

        //player dies if time expires before level is completed
        if (timePassed <= 0)
        {
            OnDeath();
        }

        //sets format of timer text
        int min = timePassed / 60;
        int sec = timePassed % 60;

        if(sec > 9)
        {
            TimeText.text = min + ":" + sec;
        }
        else
        {
            TimeText.text = min + ":0" + sec;
        }
    }


    //Initializes the time limit based on the level
    private void SetTimeLimit()
    {
        if(sceneName.Equals("Level 1"))
        {
            levelTime = 45;
        }
        else if (sceneName.Equals("Level 2"))
        {
            levelTime = 75;
        }
        else if (sceneName.Equals("Level 3"))
        {
            levelTime = 100;
        }
        else if (sceneName.Equals("Level 4"))
        {
            levelTime = 130;
        }
        //bossfight
        else if (sceneName.Equals("BossFight"))
        {
            levelTime = 240;
        }
    }

    //Loads death scene 
    public void OnDeath()
    {
        SceneManager.LoadScene("DeathScene");
    }

    //Decrements number of player's lives
    public void LostLife()
    {
        DeathSound();
        lives--;
    }

    //Resets doors, rocks, buttons, and levers
    public void ResetEverything()
    {
        LeverScript[] levers = FindObjectsOfType<LeverScript>();

        for(int i = 0; i < levers.Length; i++)
        {
            levers[i].ResetLever();
        }

        ButtonScript[] buttons = FindObjectsOfType<ButtonScript>();

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].ResetButton();
        }

        RockScript[] rocks = FindObjectsOfType<RockScript>();

        for(int i = 0; i < rocks.Length; i++)
        {
            rocks[i].ResetRock();
        }
        
    }

    //Increments timer
    public void AddTime()
    {
        levelTime += 5;
    }

    //Increments score of player
    public void IncrementScore()
    {
        levelScore += 5;
    }


    //loads next scene based on current level
    public void NextLevel()
    { 
        //updates name of level completed
        if(PlayerPrefs.HasKey("CurrentLevel"))
        {
            PlayerPrefs.SetString("CurrentLevel", sceneName);
        }
        else
        {
            PlayerPrefs.SetString("CurrentLevel", "Level One");
        }

        //Loads end scene if boss fight is completed and intermediate scene otherwise
        if (sceneName.Equals("BossFight"))
        {
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            SceneManager.LoadScene("LoadingScene");
        }
    }

    //Updates all of the player prefs based on current level
    public void setScore()
    {
        //sets score of level for player
        PlayerPrefs.SetInt("LevelScore", levelScore);

        //sets life bonus
        livesBonus = lives * 2;
        PlayerPrefs.SetInt("LifeBonus", livesBonus);

        //sets time bonus
        timeBonus = timePassed / 5;
        PlayerPrefs.SetInt("TimeBonus", timeBonus);

        //updates current score of player for the entire game
        currentScore = PlayerPrefs.GetInt("CurrentScore");
        currentScore += levelScore + livesBonus + timeBonus;
        PlayerPrefs.SetInt("CurrentScore", currentScore);

        //loads next scene
        NextLevel();

    }

    public void DeathSound()
    {
        audioSource.PlayOneShot(death);
    }

    public void GunSound()
    {
        audioSource.PlayOneShot(gunshot);
    }

    public string getScene()
    {
        return sceneName;
    }
}