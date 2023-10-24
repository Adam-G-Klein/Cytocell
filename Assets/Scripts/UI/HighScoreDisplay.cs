using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    private DifficultyConstants constants;
    private GameManager gameManager;
    private int scoreAtStartRound;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        GameObject gameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        constants = gameManagerGO.GetComponent<DifficultyConstants>();
        gameManager = gameManagerGO.GetComponent<GameManager>();

        setFromGameManager();
    }

    public void setFromGameManager(){
        if(gameManager == null) Start();
        int highScore = gameManager.getHighScore();
        // Gamemanager uses -1 so that we don't display new record at 0
        text.text =  TextUtils.intWithCommas(Mathf.Max(highScore, 0));
    }

    


}
