using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TotalScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string mode = "Balanced";

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        setTextForScore();
    }

    void Update() {
        setTextForScore();
    }

    private void setTextForScore(){
        text.text = "Total " + mode + " Score: " + intWithCommas(PlayerPrefs.GetInt("TotalScore" + mode, 0));
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
