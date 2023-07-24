using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class VisualManager : MonoBehaviour
{
    public GameObject scoreObj;
    private TextMeshProUGUI scoreText;
    public GameObject hscoreObj;
    private TextMeshProUGUI hscoreText;
    private XPBarController xpBar;
    public float deathDisplayTime;

    public GameObject addTrailObj;
    private TextMeshProUGUI addTrailText;
    private Vector3 startAddTrailWorldpos;
    public float endAddTrailPos;
    public float endAlpha;
    private ButtonGroupAlphaControls deathGroup;
    private bool hscoreSet = false;
    private DifficultyConstants constants;
    // Start is called before the first frame update
    void Start()
    {
        deathGroup = GameObject.FindGameObjectWithTag("DeathGroup").GetComponent<ButtonGroupAlphaControls>();
        scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        hscoreText = hscoreObj.GetComponent<TextMeshProUGUI>();
        constants = GameObject.FindGameObjectWithTag("GameManager").GetComponent<DifficultyConstants>();

        addTrailText = addTrailObj.GetComponent<TextMeshProUGUI>();
        xpBar = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<XPBarController>();
        startAddTrailWorldpos = addTrailObj.transform.position;
        addTrailText.alpha = 0;
    }

    public void displayDeathMessage(){
        print("player dead!");
        deathGroup.displayAll();
        if(!hscoreSet)
            hscoreText.SetText("Record: " + PlayerPrefs.GetInt("Highscore"+constants.SceneName));
        //LeanTween.value(0, 1, deathDisplayTime).setOnUpdate(;
    }
    public void restartGame()
    {
        deathGroup.hideAll();

    }
    public void updateScore(int newScore)
    {
        scoreText.SetText(newScore.ToString());

    }
    public void newHighScore(int newHighScore)
    {
        hscoreText.SetText("New Record! Score: " + newHighScore.ToString());
        hscoreSet = true;
        //IF AN EFFECT GOES HERE, CHECK SCORE != 0 

    }
    public void displayLevelText()
    {
        StartCoroutine("displayCorout");
    }
    private IEnumerator displayCorout()
    {
        float time = xpBar.XPDisplayLevelUpTime;
        int movid = LeanTween.moveLocalY(addTrailObj, endAddTrailPos, time).setEaseInOutQuad().id;
        int alid = LeanTween.value(1, endAlpha, time).setOnUpdate((float val) => {addTrailText.alpha = val; }).setEaseInQuad().id;
        yield return new WaitForSeconds(time);
        //they should be done by now anyways
        LeanTween.cancel(alid);
        LeanTween.cancel(movid);
        addTrailObj.transform.position = startAddTrailWorldpos;
        addTrailText.alpha = 0;

        
        yield return null;
    }
   
}
