using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }

        scoreText.text = "Current High Score: " + PlayerPrefs.GetInt("HighScore");

        PlayerPrefs.SetInt("CurrentScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
  
    }

    //maybe invoke level one scene change
    public void OnStartClick()
    {
        SceneManager.LoadScene("LEVEL 1");
    }

    public void OnQuitClick()
    {
        Application.Quit();
    }

}
