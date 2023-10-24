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
        text = GetComponent<TextMeshProUGUI>();
        constants = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DifficultyConstants>();
        scoreAtStartRound = PlayerPrefs.GetInt("TotalScore" + constants.SceneName, 0);
        setTextForScore();
    }

    public void setThisRoundScore(int thisRoundScore){
        print("constnats null? " + (constants == null));
        if(constants == null){
            Start();
        }
        PlayerPrefs.SetInt("TotalScore" + constants.SceneName, scoreAtStartRound + thisRoundScore);
        setTextForScore();
    }

    private void setTextForScore(){
        text.text =  TextUtils.intWithCommas(PlayerPrefs.GetInt("TotalScore" + constants.SceneName, 0));
    }

    


}
