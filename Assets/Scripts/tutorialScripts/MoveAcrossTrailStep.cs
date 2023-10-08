using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class MoveAcrossTrailStep : MonoBehaviour
{
    private TextGroupAlphaControls alphaControls;
    private TrailCollapser collapser;
    private bool hasCollapsed = false;
    [SerializeField]
    private float waitTime = 1f;

    [SerializeField]
    private GameObject videoPlayer;
    private GameManager gameManager;
    private ButtonGroupAlphaControls buttonGroupAlphaControls;
    [SerializeField]
    public float videoFadeInTime = 0.05f; 
    [SerializeField]
    public float gameTimeScaleWhilePlaying = 0.01f; 

    void Awake() {
        alphaControls = GetComponent<TextGroupAlphaControls>();
        collapser = GameObject.FindGameObjectWithTag("Player").GetComponent<TrailCollapser>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        buttonGroupAlphaControls = GetComponent<ButtonGroupAlphaControls>();
        buttonGroupAlphaControls.displayTime = videoFadeInTime;
        alphaControls.displayTime = videoFadeInTime;
        hasCollapsed = false;
    }

    void Update() {
    }

    public void resetTutorial(){
        print("resetting move across trail step");
        PlayerPrefs.SetInt(TutorialManager.MOVE_ACROSS_TRAIL_COMPLETION, 0);
        StopAllCoroutines();
        alphaControls.setVisibleQuickly(false);
        hasCollapsed = false;
    }

    public void MoveAcrossTrail(){
        if(PlayerPrefs.GetInt(TutorialManager.MOVE_ACROSS_TRAIL_COMPLETION) == 1) {
            SendMessageUpwards("StepDone");
            return;
        } 
        StartCoroutine("corout");
    }

    private IEnumerator corout(){
        print("started move across trail corout");
        yield return new WaitForSeconds(waitTime);
        yield return new WaitUntil(() => !gameManager.gamePaused);
        print("done waiting");
        alphaControls.displayAll();
        gameManager.setGamePaused(true, gameTimeScaleWhilePlaying);
        buttonGroupAlphaControls.displayAll();
        videoPlayer.SetActive(true);
        yield return new WaitUntil(() => Input.touchCount > 0);
        buttonGroupAlphaControls.hideAll();
        gameManager.setGamePaused(false);
        videoPlayer.SetActive(false);
        yield return new WaitUntil(() => collapser.collapseTriggered);
        PlayerPrefs.SetInt(TutorialManager.MOVE_ACROSS_TRAIL_COMPLETION, 1);
        hasCollapsed = true;
        yield return new WaitForSeconds(1f);
        alphaControls.hideAll();
        SendMessageUpwards("StepDone");
    }
}
