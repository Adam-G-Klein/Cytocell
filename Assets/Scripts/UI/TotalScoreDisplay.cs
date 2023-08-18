using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    private DifficultyConstants constants;
    private int scoreAtStartRound;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        constants = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DifficultyConstants>();
        scoreAtStartRound = PlayerPrefs.GetInt("TotalScore" + constants.SceneName, 0);
        setTextForScore();
    }

    public void setThisRoundScore(int thisRoundScore){
        PlayerPrefs.SetInt("TotalScore" + constants.SceneName, scoreAtStartRound + thisRoundScore);
        setTextForScore();
    }

    private void setTextForScore(){
        text.text = "Total " + constants.SceneName+ " Score: " + intWithCommas(PlayerPrefs.GetInt("TotalScore" + constants.SceneName, 0));
    }

    private string intWithCommas(int num){
        string numStr = num.ToString();
        string newStr = "";
        for(int i = 0; i < numStr.Length; i++){
            if(i % 3 == 0 && i != 0){
                newStr = "," + newStr;
            }
            newStr = numStr[numStr.Length - 1 - i] + newStr;
        }
        return newStr;
    }


}
