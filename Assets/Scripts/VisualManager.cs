using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField]
    private TextMeshProUGUI newRecordText;
    // one in pause menu, one on death screen
    private HighScoreDisplay[] highScoreDisplays;
    // one in pause menu, one on death screen
    private TotalScoreDisplay[] totalScoreDisplay;
    // one in pause menu, one on death screen
    private TotalCurrencyDisplay[] totalCurrencyDisplay;

    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private int bannerVerticalOffset;
    [SerializeField]
    private List<GameObject> nonBannerOffsetObjects = new List<GameObject>();
    private GameObject tutorialCanvas;
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
        totalScoreDisplay = GetComponentsInChildren<TotalScoreDisplay>();
        totalCurrencyDisplay = GetComponentsInChildren<TotalCurrencyDisplay>();
        highScoreDisplays = GetComponentsInChildren<HighScoreDisplay>();
        addTrailText.alpha = 0;
        scoreText.text = constants.SceneName;
        tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas");
        newRecordText.gameObject.SetActive(false);
        updateScore(0, false);
    }

    public void applyBannerOffset(){
        List<Transform> objs = new List<Transform>();
        objs.AddRange(transform.GetComponentsInChildren<Transform>());
        objs.Add(newRecordText.transform);
        Transform tutorialCanvas = GameObject.FindGameObjectWithTag("TutorialCanvas").transform;
        if(tutorialCanvas)
            objs.AddRange(tutorialCanvas.GetComponentsInChildren<Transform>());
        for(int i = 0; i < objs.Count; i++){
            Transform child = objs[i];
            RectTransform rect = child.GetComponent<RectTransform>();
            if(rect && !nonBannerOffsetObjects.Contains(child.gameObject)) {
                child.transform.localPosition += new Vector3(0, bannerVerticalOffset, 0);
            }
        }
    }


    public void displayDeathMessage(){
        pauseButton.SetActive(false);
        StartCoroutine("displayDeathMessageCorout");
    }

    private IEnumerator displayDeathMessageCorout()
    {
        Camera.main.GetComponent<DamageCameraEffect>().clearEffect = true;
        if(tutorialCanvas)
            tutorialCanvas.SetActive(false);
        deathGroup.displayAll();
        if(!hscoreSet)
            hscoreText.SetText("One-run Record: " + PlayerPrefs.GetInt("Highscore"+constants.SceneName));
        yield return null;
    }
    public void restartGame()
    {
        deathGroup.hideAll();
        Camera.main.GetComponent<DamageCameraEffect>().clearEffect = false;

    }
    public void updateScore(int newScore, bool newHighScore)
    {
        print("update score: " + newScore );
        foreach(TotalCurrencyDisplay csd in totalCurrencyDisplay){
            print("updating currency for object: " + csd.gameObject.name);
            csd.updateStat();
        }
        foreach(TotalScoreDisplay tsd in totalScoreDisplay){
            print("updating score for object: " + tsd.gameObject.name);
            tsd.setThisRoundScore(newScore);
        }
        foreach(HighScoreDisplay hsd in highScoreDisplays){
            print("updating highscore for object: " + hsd.gameObject.name);
            hsd.setFromGameManager();
        }
        scoreText.SetText("¤" + newScore.ToString());
        if(newHighScore)
            newRecordText.gameObject.SetActive(true);
    }

    public void newHighScore(int newHighScore)
    {
        newRecordText.gameObject.SetActive(true);
        hscoreText.SetText("New Record! Score: " + newHighScore.ToString());
        hscoreSet = true;
        //IF A CELEBRATION EFFECT ENDS UP HERE, CHECK SCORE != 0 
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
        //addTrailObj.transform.position = startAddTrailWorldpos;
        addTrailText.alpha = 0;

        
        yield return null;
    }
   
}
