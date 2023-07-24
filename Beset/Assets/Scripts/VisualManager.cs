using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VisualManager : MonoBehaviour
{
    public GameObject deathMsgObj;
    private TextMeshProUGUI deathMsg;
    public GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    public GameObject hscoreObj;
    private TextMeshProUGUI hscoreText;
    public GameObject retryButton;
    public float deathDisplayTime;
    // Start is called before the first frame update
    void Start()
    {

        deathMsg = deathMsgObj.GetComponent<TextMeshProUGUI>();
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        hscoreText = hscoreObj.GetComponent<TextMeshProUGUI>();
        deathMsg.alpha = 0;
        hscoreText.alpha = 0;
        retryButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void displayDeathMessage(){
        print("player dead!");
        deathMsg.alpha = 1;
        hscoreText.SetText("Record: " + PlayerPrefs.GetInt("Highscore"+constants.SceneName).ToString());
        hscoreText.alpha = 1;
        retryButton.SetActive(true);
        //LeanTween.value(0, 1, deathDisplayTime).setOnUpdate(;
    }
    public void restartGame()
    {
        deathMsg.alpha = 0;

    }
    public void updateScore(int newScore)
    {
        scoreText.SetText(newScore.ToString());

    }
    public void newHighScore(int newHighScore)
    {
        hscoreText.SetText("Record: " + newHighScore.ToString());
        //IF AN EFFECT GOES HERE, CHECK SCORE != 0 

    }
}
