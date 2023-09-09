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
    // one in pause menu, one on death screen
    private TotalScoreDisplay[] totalScoreDisplay;
    [SerializeField]
    private GameObject pauseButton;
    [SerializeField]
    private int bannerVerticalOffset;
    [SerializeField]
    private List<GameObject> nonBannerOffsetObjects = new List<GameObject>();
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
        addTrailText.alpha = 0;
        scoreText.text = constants.SceneName;
    }

    public void applyBannerOffset(){
        for(int i = 0; i < transform.childCount; i++){
            Transform child = transform.GetChild(i);
            RectTransform rect = child.GetComponent<RectTransform>();
            if(rect && !nonBannerOffsetObjects.Contains(child.gameObject)) {
                child.transform.localPosition += new Vector3(0, bannerVerticalOffset, 0);
                //rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + bannerVerticalOffset);
            }
        }
    }

    public void displayDeathMessage(){
        Camera.main.GetComponent<DamageCameraEffect>().clearEffect = true;
        deathGroup.displayAll();
        if(!hscoreSet)
            hscoreText.SetText("Record: " + PlayerPrefs.GetInt("Highscore"+constants.SceneName));
    }
    public void restartGame()
    {
        deathGroup.hideAll();
        Camera.main.GetComponent<DamageCameraEffect>().clearEffect = false;

    }
    public void updateScore(int newScore)
    {
        foreach(TotalScoreDisplay tsd in totalScoreDisplay){
            tsd.setThisRoundScore(newScore);
        }
        scoreText.SetText(newScore.ToString());
    }

    public void newHighScore(int newHighScore)
    {
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
        addTrailObj.transform.position = startAddTrailWorldpos;
        addTrailText.alpha = 0;

        
        yield return null;
    }
   
}
